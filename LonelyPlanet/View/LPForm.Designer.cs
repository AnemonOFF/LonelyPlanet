namespace LonelyPlanet
{
    partial class LPForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LPForm));
            this.mainMenu = new LonelyPlanet.View.MainMenu();
            this.SuspendLayout();
            // 
            // mainMenu
            // 
            this.mainMenu.BackColor = System.Drawing.Color.Transparent;
            this.mainMenu.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mainMenu.BackgroundImage")));
            this.mainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(854, 480);
            this.mainMenu.TabIndex = 0;
            // 
            // LPForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 480);
            this.Controls.Add(this.mainMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(870, 519);
            this.Name = "LPForm";
            this.Text = "Lonely Planet: Mars";
            this.ResumeLayout(false);

        }

        #endregion

        private View.MainMenu mainMenu;
    }
}

