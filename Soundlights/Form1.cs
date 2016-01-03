using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.Runtime.InteropServices;

using NAudio.Wave;
using NAudio.Dsp;

namespace Soundlights
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct UnionStruct
    {
        [FieldOffset(0)]
        public byte[] bytes;
        [FieldOffset(0)]
        public float[] floats;
    }

    public partial class Soundlights : Form
    {
        const int RGB_FADE_EFFECT = 0;
        const int FADE_EFFECT = 1;

        bool connected = false; // глобальный флаг состояния соединения с устройством

        IWaveIn waveIn;

        const int log2N = 12; // степень двойки для БПФ
        const int size = 1 << log2N; // размер буфера крайтный степени двойки

        float gain = 1f; // уровень усиления
        const float targetAmplitude = 1; // требуемый уровень амплитуды
        const float upRegulatorRatio = 0.05f; // скорость повышения уровня АРУ
        const float downRegulatorRatio = 0.38f; // скорость понижения уровня АРУ

        const float inNoiseLevel = 1e-4f;//6e-5f; // входной уровень шума
        const float outNoiseLevel = 9e-5f;//2e-5f; // выходной уровень шума

        const int numBands = 3; // количество каналов
        const double downtimeInSeconds = 5.0f; // время ожидания fading'а

        float[] values = new float[numBands]; // значения яркости каналов
        float[] valuesScale = new float[numBands]; // значения корректировки яркости каналов
        float[] outputScale = new float[numBands] { 1000f, 1000f, 1000f }; //

        int steps = 255; // количество шагов переливания цвета
        int step = 0; // текущий шаг

        Random rnd = new Random();
        Random rnd_step = new Random();

        int[,] colors = new int[2, numBands]; // массив из двух цветов для RGB fading'а

        // значения границ частотного диапазона для вычисления амплитуды каналов
        int[] bandDefs = new int[numBands - 1] {
            300, 1000
        };

        Stopwatch stopWatch = new Stopwatch();

        public Soundlights()
        {
            InitializeComponent();
        }

        // Инициализация начальных значений переменных
        private void Soundlights_Load(object sender, EventArgs e)
        {
            getPortNames();

            portListBox.SelectedIndex = 0;
            agcCheckBox.Checked = true;

            components_visibility(connected);

            valuesScale[0] = (float)((redBar.Value + 10) / 10.0);
            valuesScale[1] = (float)((greenBar.Value + 10) / 10.0);
            valuesScale[2] = (float)((blueBar.Value + 10) / 10.0);

            serialPortInit();

            componentsTimer.Tick += ComponentsTimer_Tick;
            componentsTimer.Start();

            rgbTimer.Tick += RgbTimer_Tick; ;
            rgbTimer.Start();
        }

        private void ComponentsTimer_Tick(object sender, EventArgs e)
        {
            components_visibility(connected);
        }

        // Плавное переливание цветом, генерация случайного нового цвета и количество шагов переливания
        private void RgbTimer_Tick(object sender, EventArgs e)
        {
            if (step > steps)
            {
                step = 0;

                for (int i = 0; i < numBands; i++)
                {
                    colors[0, i] = colors[1, i];
                    colors[1, i] = rnd.Next(0, 255);

                    steps = rnd_step.Next(40, 256);
                }
            }

            step++;
        }

        // Управление отображением омпонентов в зависимости от состояния соединения с устройством
        private void components_visibility(bool is_connected)
        {
            portListBox.Enabled = !is_connected;
            agcCheckBox.Visible = is_connected;
            fadeCheckBox.Visible = is_connected;

            connectBtn.Text = is_connected ? "Отключиться" : "Подключиться";

            redBar.Visible = is_connected;
            greenBar.Visible = is_connected;
            blueBar.Visible = is_connected;
        }

        // настройка параметров порта
        void serialPortInit()
        {
            connectedPort.BaudRate = 19200;

            connectedPort.Parity = Parity.None;
            connectedPort.StopBits = StopBits.One;
            connectedPort.Handshake = Handshake.None;

            connectedPort.ReadTimeout = 200;
            connectedPort.WriteTimeout = 200;
        }

        // Получение списка последовательных портов и добавление их в список подключаемых портов на форме
        void getPortNames()
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                portListBox.Items.Add(s);
            }

            if (portListBox.Items.Count == 0)
            {
                MessageBox.Show("Устройство не было сопряжено с системой или Bluetooth отключен. Прочтите инструкцию для создания связи  компьютера с устройством", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
            }
        }

        // При клике по списку доступных портов обновляем этот список (очищаем и загружаем новый список доступных портов)
        private void portListBox_Click(object sender, EventArgs e)
        {
            portListBox.Items.Clear();

            getPortNames();
        }

        // Если выбрали нужный порт, присваиваем порту выбранное имя
        private void portListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (connectedPort.IsOpen)
            {
                connectedPort.Close();
            }

            connectedPort.PortName = portListBox.Text;
        }

        // Обработка нажатия кнопки подключения/отключения
        private void connectBtn_Click(object sender, EventArgs e)
        {
            if (!connected)
            {
                try
                {
                    connectedPort.Open();
                }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Последовательный порт " + connectedPort.PortName + " недоступен.", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                if (connectedPort.IsOpen)
                {
                    connected = true;

                    waveIn = new WasapiLoopbackCapture();

                    waveIn.DataAvailable += WaveIn_DataAvailable; ;
                    waveIn.RecordingStopped += WaveIn_RecordingStopped; ;

                    waveIn.StartRecording();
                }
            }
            else
            {
                waveIn.StopRecording();

                if (connectedPort.IsOpen)
                {
                    connectedPort.Write("set rgb 0 0 0\n\r");
                    connectedPort.Close();
                }

                connected = false;
            }
        }

        //Функция пропорционально переносит значение (x) из диапазона (inMin..inMax) в новый диапазон(outMin..outMax)
        int map(int x, int inMin, int inMax, int outMin, int outMax)
        {
            return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        // Функция ограничивает вещественное число v максимальным значением 255 и переводит его в байт
        byte limit(float v)
        {
            return (byte)(v > 255 ? 255 : v);
        }

        // Процедура распределяющая процедуры по цветовым эффектам
        private void light_effects(float[] ch, int effect_number)
        {
            for (int i = 0; i < numBands; i++)
            {
                switch (effect_number)
                {
                    case RGB_FADE_EFFECT:
                        ch[i] = map(step, 0, steps, colors[0, i], colors[1, i]);
                        break;
                }
            }
        }

        // Функция, возвращающая коэффициент окна Блэкмана
        private float blackmanWindow(int n, int size)
        {
            return (float)(0.42 - 0.5 * Math.Cos(2 * Math.PI * n / (size - 1)) + 0.08 * Math.Cos(4 * Math.PI * n / (size - 1)));
        }
        
        // Событие записи для объекта Wave - анализ сигнала с управлением автоподстройкой и световыми эффектами
        private void WaveIn_DataAvailable(object sender, WaveInEventArgs e)
        {
            int samplesRecorded = e.BytesRecorded / (waveIn.WaveFormat.BitsPerSample / 8); // размер полученного буфера
            int numChannels = waveIn.WaveFormat.Channels;
            float sampleRate = waveIn.WaveFormat.SampleRate;

            UnionStruct u = new UnionStruct();
            u.bytes = e.Buffer;

            Complex[] data = new Complex[size]; // буфер для БПФ

            float currentAmplitude = 0;

            // готовим сигнал к БПФ
            int j = 0;

            for (int i = 0; i < samplesRecorded && j < size; i += numChannels)
            {
                float left = u.floats[i];
                float right = u.floats[i + (numChannels - 1)];

                float avg = (left + right) / 2;

                if (Math.Abs(avg) > currentAmplitude)
                {
                    currentAmplitude = Math.Abs(avg);
                }

                //float value = avg * (float)FastFourierTransform.BlackmannHarrisWindow(j, size);
                float value = avg * blackmanWindow(j, size);

                data[j].X = value;
                data[j].Y = 0;

                j++;
            }

            if (currentAmplitude > inNoiseLevel)
            {
                stopWatch.Reset();

                if (agcCheckBox.Checked)
                {
                    float regulatorRatio = currentAmplitude * gain < targetAmplitude ? upRegulatorRatio : downRegulatorRatio;

                    gain += regulatorRatio * (targetAmplitude / currentAmplitude - gain);

                    for (int i = 0; i < j; i++)
                    {
                        data[i].X *= gain;
                    }
                }
                // остаток наполняем нулями
                while (j < size)
                {
                    data[j].X = 0;
                    data[j].Y = 0;

                    j++;
                }

                FastFourierTransform.FFT(true, log2N, data); // выполняем быстрое преобразование Фурье

                int maxI = -1; // индекс макисмальной амплитуды
                float max = -1; // значение максимальной амплитуды

                int band = 0;

                for (int i = 0; i < numBands; i++)
                {
                    values[i] = 0;
                }

                for (int i = 0; i < size / 2; i++)
                {
                    float currentFreq = sampleRate * i / size;

                    if (band < numBands - 1 && currentFreq > bandDefs[band])
                    {
                        band++;
                    }

                    float amp = data[i].X * data[i].X + data[i].Y * data[i].Y;

                    if (amp > outNoiseLevel)
                    {
                        values[band] += amp;
                    }

                    if (amp > max)
                    {
                        max = amp;
                        maxI = i;
                    }
                }

                for (int i = 0; i < numBands; i++)
                {
                    values[i] = (float)Math.Sqrt(values[i]) * valuesScale[i];
                }
            }
            else
            {
                stopWatch.Stop();
                double seconds = stopWatch.Elapsed.TotalSeconds;
                stopWatch.Start();

                if (seconds > downtimeInSeconds && fadeCheckBox.Checked)
                {
                    light_effects(values, RGB_FADE_EFFECT);
                }
                else
                {
                    for (int i = 0; i < numBands; i++)
                    {
                        values[i] = 0;
                    }
                }
            }

            try
            {
                connectedPort.Write("set rgb " + limit(values[0]) + " " + limit(values[1]) + " " + limit(values[2]) + "\n");
            }
            catch (TimeoutException)
            {
                MessageBox.Show("Последовательный порт " + connectedPort.PortName +
                        " стал недоступен. Попробуйте подключиться ещё раз", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                connected = false;

                if (connectedPort.IsOpen)
                {
                    connectedPort.Close();
                }

                waveIn.StopRecording();
            }
        }

        // Освобождение памяти от объекта Wave
        private void WaveIn_RecordingStopped(object sender, StoppedEventArgs e)
        {
            waveIn.Dispose();
            waveIn = null;
        }

        // Передача значения НЧ регулятора в значение корректировки НЧ канала
        private void redBar_ValueChanged(object sender, EventArgs e)
        {
            valuesScale[0] = (float)((redBar.Value + 10) / 10.0) * 1000;
        }

        // Передача значения СЧ регулятора в значение корректировки СЧ канала
        private void greenBar_ValueChanged(object sender, EventArgs e)
        {
            valuesScale[1] = (float)((greenBar.Value + 10) / 10.0) * 1000;
        }

        // Передача значения ВЧ регулятора в значение корректировки ВЧ канала
        private void blueBar_ValueChanged(object sender, EventArgs e)
        {
            valuesScale[2] = (float)((blueBar.Value + 10) / 10.0) * 1000;
        }

        // Добавление подсказки со значением НЧ регулятора
        private void redBar_Scroll(object sender, EventArgs e)
        {
            barsToolTip.SetToolTip(redBar, redBar.Value.ToString());
        }

        // Добавление подсказки со значением СЧ регулятора 
        private void greenBar_Scroll(object sender, EventArgs e)
        {
            barsToolTip.SetToolTip(greenBar, greenBar.Value.ToString());
        }

        // Добавление подсказки со значением ВЧ регулятора
        private void blueBar_Scroll(object sender, EventArgs e)
        {
            barsToolTip.SetToolTip(blueBar, blueBar.Value.ToString());
        }

        private void Soundlights_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connected)
            {
                waveIn.StopRecording();

                if (connectedPort.IsOpen)
                {
                    connectedPort.Write("set rgb 0 0 0\n");
                    connectedPort.Close();
                }
            }
        }

        private void Soundlights_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon.Visible = true;
            }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                notifyIcon.Visible = false;
            }
        }
    }
}
