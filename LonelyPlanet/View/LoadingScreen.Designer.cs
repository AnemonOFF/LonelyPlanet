namespace LonelyPlanet.View
{
    partial class LoadingScreen
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingScreen));
            this.LoadingBG = new AxWMPLib.AxWindowsMediaPlayer();
            this.loadingProgress = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingBG)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadingBG
            // 
            this.LoadingBG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadingBG.Enabled = true;
            this.LoadingBG.Location = new System.Drawing.Point(0, 0);
            this.LoadingBG.Name = "LoadingBG";
            this.LoadingBG.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("LoadingBG.OcxState")));
            this.LoadingBG.Size = new System.Drawing.Size(854, 480);
            this.LoadingBG.TabIndex = 0;
            this.LoadingBG.TabStop = false;
            // 
            // loadingProgress
            // 
            this.loadingProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.loadingProgress.Location = new System.Drawing.Point(0, 457);
            this.loadingProgress.Name = "loadingProgress";
            this.loadingProgress.Size = new System.Drawing.Size(854, 23);
            this.loadingProgress.TabIndex = 1;
            // 
            // LoadingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadingProgress);
            this.Controls.Add(this.LoadingBG);
            this.Name = "LoadingScreen";
            this.Size = new System.Drawing.Size(854, 480);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingBG)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer LoadingBG;
        private System.Windows.Forms.ProgressBar loadingProgress;
    }
}
