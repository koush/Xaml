namespace DependencyObjectCodeGenerator
{
    partial class GeneratorForm
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
            this.myOpenButton = new System.Windows.Forms.Button();
            this.myGeneratedCodeBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // myOpenButton
            // 
            this.myOpenButton.Location = new System.Drawing.Point(13, 13);
            this.myOpenButton.Name = "myOpenButton";
            this.myOpenButton.Size = new System.Drawing.Size(75, 23);
            this.myOpenButton.TabIndex = 1;
            this.myOpenButton.Text = "Open";
            this.myOpenButton.UseVisualStyleBackColor = true;
            this.myOpenButton.Click += new System.EventHandler(this.myOpenButton_Click);
            // 
            // myGeneratedCodeBox
            // 
            this.myGeneratedCodeBox.Location = new System.Drawing.Point(13, 43);
            this.myGeneratedCodeBox.Multiline = true;
            this.myGeneratedCodeBox.Name = "myGeneratedCodeBox";
            this.myGeneratedCodeBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.myGeneratedCodeBox.Size = new System.Drawing.Size(703, 383);
            this.myGeneratedCodeBox.TabIndex = 2;
            // 
            // GeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 438);
            this.Controls.Add(this.myGeneratedCodeBox);
            this.Controls.Add(this.myOpenButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "GeneratorForm";
            this.Text = "DependencyObject Code Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button myOpenButton;
        private System.Windows.Forms.TextBox myGeneratedCodeBox;
    }
}

