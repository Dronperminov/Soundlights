namespace Soundlights
{
    partial class Soundlights
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Soundlights));
            this.portListBox = new System.Windows.Forms.ComboBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.agcCheckBox = new System.Windows.Forms.CheckBox();
            this.fadeCheckBox = new System.Windows.Forms.CheckBox();
            this.redBar = new System.Windows.Forms.TrackBar();
            this.greenBar = new System.Windows.Forms.TrackBar();
            this.blueBar = new System.Windows.Forms.TrackBar();
            this.componentsTimer = new System.Windows.Forms.Timer(this.components);
            this.rgbTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.connectedPort = new System.IO.Ports.SerialPort(this.components);
            this.barsToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.redBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueBar)).BeginInit();
            this.SuspendLayout();
            // 
            // portListBox
            // 
            this.portListBox.FormattingEnabled = true;
            this.portListBox.Location = new System.Drawing.Point(12, 12);
            this.portListBox.Name = "portListBox";
            this.portListBox.Size = new System.Drawing.Size(94, 21);
            this.portListBox.Sorted = true;
            this.portListBox.TabIndex = 0;
            this.toolTip1.SetToolTip(this.portListBox, "Выбор порта для подключения к устройству");
            this.portListBox.SelectedIndexChanged += new System.EventHandler(this.portListBox_SelectedIndexChanged);
            this.portListBox.Click += new System.EventHandler(this.portListBox_Click);
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(12, 39);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(94, 23);
            this.connectBtn.TabIndex = 1;
            this.connectBtn.Text = "Подключиться";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // agcCheckBox
            // 
            this.agcCheckBox.AutoSize = true;
            this.agcCheckBox.Checked = true;
            this.agcCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.agcCheckBox.Location = new System.Drawing.Point(12, 73);
            this.agcCheckBox.Name = "agcCheckBox";
            this.agcCheckBox.Size = new System.Drawing.Size(48, 17);
            this.agcCheckBox.TabIndex = 2;
            this.agcCheckBox.Text = "АРУ";
            this.toolTip1.SetToolTip(this.agcCheckBox, "Автоматическая регулировка громкости");
            this.agcCheckBox.UseVisualStyleBackColor = true;
            // 
            // fadeCheckBox
            // 
            this.fadeCheckBox.AutoSize = true;
            this.fadeCheckBox.Checked = true;
            this.fadeCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fadeCheckBox.Location = new System.Drawing.Point(66, 73);
            this.fadeCheckBox.Name = "fadeCheckBox";
            this.fadeCheckBox.Size = new System.Drawing.Size(50, 17);
            this.fadeCheckBox.TabIndex = 3;
            this.fadeCheckBox.Text = "Fade";
            this.toolTip1.SetToolTip(this.fadeCheckBox, "Включение световых эффектов");
            this.fadeCheckBox.UseVisualStyleBackColor = true;
            // 
            // redBar
            // 
            this.redBar.Location = new System.Drawing.Point(147, -1);
            this.redBar.Minimum = -10;
            this.redBar.Name = "redBar";
            this.redBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.redBar.Size = new System.Drawing.Size(45, 104);
            this.redBar.TabIndex = 4;
            this.redBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.redBar, "Уровень усиления красного канала");
            this.redBar.Scroll += new System.EventHandler(this.redBar_Scroll);
            this.redBar.ValueChanged += new System.EventHandler(this.redBar_ValueChanged);
            // 
            // greenBar
            // 
            this.greenBar.Location = new System.Drawing.Point(181, -1);
            this.greenBar.Minimum = -10;
            this.greenBar.Name = "greenBar";
            this.greenBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.greenBar.Size = new System.Drawing.Size(45, 104);
            this.greenBar.TabIndex = 5;
            this.greenBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.greenBar, "Уровень усиления зелёного канала");
            this.greenBar.Scroll += new System.EventHandler(this.greenBar_Scroll);
            this.greenBar.ValueChanged += new System.EventHandler(this.greenBar_ValueChanged);
            // 
            // blueBar
            // 
            this.blueBar.Location = new System.Drawing.Point(215, -1);
            this.blueBar.Minimum = -10;
            this.blueBar.Name = "blueBar";
            this.blueBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.blueBar.Size = new System.Drawing.Size(45, 104);
            this.blueBar.TabIndex = 6;
            this.blueBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.toolTip1.SetToolTip(this.blueBar, "Уровень усиления синего канала");
            this.blueBar.Scroll += new System.EventHandler(this.blueBar_Scroll);
            this.blueBar.ValueChanged += new System.EventHandler(this.blueBar_ValueChanged);
            // 
            // componentsTimer
            // 
            this.componentsTimer.Interval = 1;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Цветомузыка";
            this.notifyIcon.Visible = true;
            this.notifyIcon.Click += new System.EventHandler(this.notifyIcon_Click);
            // 
            // Soundlights
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 102);
            this.Controls.Add(this.blueBar);
            this.Controls.Add(this.greenBar);
            this.Controls.Add(this.redBar);
            this.Controls.Add(this.fadeCheckBox);
            this.Controls.Add(this.agcCheckBox);
            this.Controls.Add(this.connectBtn);
            this.Controls.Add(this.portListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Soundlights";
            this.Text = "Soundlights";
            this.Deactivate += new System.EventHandler(this.Soundlights_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Soundlights_FormClosing);
            this.Load += new System.EventHandler(this.Soundlights_Load);
            ((System.ComponentModel.ISupportInitialize)(this.redBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blueBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox portListBox;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.CheckBox agcCheckBox;
        private System.Windows.Forms.CheckBox fadeCheckBox;
        private System.Windows.Forms.TrackBar redBar;
        private System.Windows.Forms.TrackBar greenBar;
        private System.Windows.Forms.TrackBar blueBar;
        private System.Windows.Forms.Timer componentsTimer;
        private System.Windows.Forms.Timer rgbTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.IO.Ports.SerialPort connectedPort;
        private System.Windows.Forms.ToolTip barsToolTip;
    }
}

