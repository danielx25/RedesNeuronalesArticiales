namespace RedesNeuronalesArtificiales
{
    partial class Form1
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
            this.pruebaboton = new System.Windows.Forms.Button();
            this.prueba1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pruebaboton
            // 
            this.pruebaboton.Location = new System.Drawing.Point(535, 164);
            this.pruebaboton.Name = "pruebaboton";
            this.pruebaboton.Size = new System.Drawing.Size(75, 23);
            this.pruebaboton.TabIndex = 0;
            this.pruebaboton.Text = "button1";
            this.pruebaboton.UseVisualStyleBackColor = true;
            // 
            // prueba1
            // 
            this.prueba1.AutoSize = true;
            this.prueba1.Location = new System.Drawing.Point(463, 164);
            this.prueba1.Name = "prueba1";
            this.prueba1.Size = new System.Drawing.Size(46, 17);
            this.prueba1.TabIndex = 1;
            this.prueba1.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 460);
            this.Controls.Add(this.prueba1);
            this.Controls.Add(this.pruebaboton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button pruebaboton;
        private System.Windows.Forms.Label prueba1;
    }
}