namespace kretenKreta
{
    partial class WelcomeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeForm));
            nev = new Label();
            btnLogout = new Button();
            exit = new Button();
            lblVersion = new Label();
            listJegy = new ListBox();
            SuspendLayout();
            // 
            // nev
            // 
            nev.AutoSize = true;
            nev.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 238);
            nev.Location = new Point(12, 9);
            nev.Name = "nev";
            nev.Size = new Size(50, 30);
            nev.TabIndex = 0;
            nev.Text = "Nev";
            // 
            // btnLogout
            // 
            btnLogout.Location = new Point(694, 47);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(94, 23);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Kijelentkezés";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click_1;
            // 
            // exit
            // 
            exit.Location = new Point(694, 2);
            exit.Name = "exit";
            exit.Size = new Size(94, 39);
            exit.TabIndex = 2;
            exit.Text = "Bezárás";
            exit.UseVisualStyleBackColor = true;
            exit.Click += exit_Click;
            // 
            // lblVersion
            // 
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(12, 47);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(38, 15);
            lblVersion.TabIndex = 3;
            lblVersion.Text = "verzió";
            // 
            // listJegy
            // 
            listJegy.BackColor = SystemColors.Window;
            listJegy.BorderStyle = BorderStyle.None;
            listJegy.Font = new Font("Arial Rounded MT Bold", 12F, FontStyle.Underline, GraphicsUnit.Point, 0);
            listJegy.FormattingEnabled = true;
            listJegy.ImeMode = ImeMode.Off;
            listJegy.ItemHeight = 18;
            listJegy.Location = new Point(12, 74);
            listJegy.Name = "listJegy";
            listJegy.SelectionMode = SelectionMode.MultiExtended;
            listJegy.Size = new Size(776, 360);
            listJegy.Sorted = true;
            listJegy.TabIndex = 7;
            listJegy.SelectedIndexChanged += listJegy_SelectedIndexChanged;
            // 
            // WelcomeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoValidate = AutoValidate.EnablePreventFocusChange;
            BackColor = SystemColors.Window;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(listJegy);
            Controls.Add(lblVersion);
            Controls.Add(exit);
            Controls.Add(btnLogout);
            Controls.Add(nev);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "WelcomeForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Kretén - Kréta";
            Load += WelcomeForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label nev;
        private Button btnLogout;
        private Button exit;
        private Label lblVersion;
        private ListBox listJegy;
    }
}