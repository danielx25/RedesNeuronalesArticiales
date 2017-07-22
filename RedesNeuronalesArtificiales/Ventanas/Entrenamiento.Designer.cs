namespace RedesNeuronalesArtificiales.Ventanas
{
    partial class Entrenamiento
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
            this.pestannas = new System.Windows.Forms.TabControl();
            this.pestannaEntrenamiento = new System.Windows.Forms.TabPage();
            this.barraDeProgreso = new System.Windows.Forms.ProgressBar();
            this.mapaResultado = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.botonLimpiar = new System.Windows.Forms.Button();
            this.agregarNeurona = new System.Windows.Forms.Button();
            this.entradaNeurona = new System.Windows.Forms.TextBox();
            this.textoGruposNeuronas = new System.Windows.Forms.Label();
            this.gruposDeNeuronas = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.titulo = new System.Windows.Forms.Label();
            this.entradaLimiteCiclos = new System.Windows.Forms.TextBox();
            this.limiteCiclos = new System.Windows.Forms.Label();
            this.botonEntrenar = new System.Windows.Forms.Button();
            this.entradaBeta = new System.Windows.Forms.TextBox();
            this.entradaAlfa = new System.Windows.Forms.TextBox();
            this.beta = new System.Windows.Forms.Label();
            this.alfa = new System.Windows.Forms.Label();
            this.pestannaValidacion = new System.Windows.Forms.TabPage();
            this.pestannas.SuspendLayout();
            this.pestannaEntrenamiento.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pestannas
            // 
            this.pestannas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pestannas.Controls.Add(this.pestannaEntrenamiento);
            this.pestannas.Controls.Add(this.pestannaValidacion);
            this.pestannas.Location = new System.Drawing.Point(12, 12);
            this.pestannas.Name = "pestannas";
            this.pestannas.SelectedIndex = 0;
            this.pestannas.Size = new System.Drawing.Size(879, 541);
            this.pestannas.TabIndex = 0;
            // 
            // pestannaEntrenamiento
            // 
            this.pestannaEntrenamiento.Controls.Add(this.barraDeProgreso);
            this.pestannaEntrenamiento.Controls.Add(this.mapaResultado);
            this.pestannaEntrenamiento.Controls.Add(this.panel2);
            this.pestannaEntrenamiento.Controls.Add(this.panel1);
            this.pestannaEntrenamiento.Location = new System.Drawing.Point(4, 22);
            this.pestannaEntrenamiento.Name = "pestannaEntrenamiento";
            this.pestannaEntrenamiento.Padding = new System.Windows.Forms.Padding(3);
            this.pestannaEntrenamiento.Size = new System.Drawing.Size(871, 515);
            this.pestannaEntrenamiento.TabIndex = 0;
            this.pestannaEntrenamiento.Text = "Entrenamiento";
            this.pestannaEntrenamiento.UseVisualStyleBackColor = true;
            this.pestannaEntrenamiento.Enter += new System.EventHandler(this.pestannaEntrenamiento_Enter);
            // 
            // barraDeProgreso
            // 
            this.barraDeProgreso.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.barraDeProgreso.Location = new System.Drawing.Point(7, 486);
            this.barraDeProgreso.Name = "barraDeProgreso";
            this.barraDeProgreso.Size = new System.Drawing.Size(858, 23);
            this.barraDeProgreso.TabIndex = 18;
            // 
            // mapaResultado
            // 
            this.mapaResultado.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mapaResultado.Location = new System.Drawing.Point(323, 6);
            this.mapaResultado.MinimumSize = new System.Drawing.Size(20, 20);
            this.mapaResultado.Name = "mapaResultado";
            this.mapaResultado.Size = new System.Drawing.Size(542, 474);
            this.mapaResultado.TabIndex = 17;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.Controls.Add(this.botonLimpiar);
            this.panel2.Controls.Add(this.agregarNeurona);
            this.panel2.Controls.Add(this.entradaNeurona);
            this.panel2.Controls.Add(this.textoGruposNeuronas);
            this.panel2.Controls.Add(this.gruposDeNeuronas);
            this.panel2.Location = new System.Drawing.Point(6, 201);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(311, 279);
            this.panel2.TabIndex = 16;
            // 
            // botonLimpiar
            // 
            this.botonLimpiar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.botonLimpiar.Location = new System.Drawing.Point(231, 252);
            this.botonLimpiar.Name = "botonLimpiar";
            this.botonLimpiar.Size = new System.Drawing.Size(75, 23);
            this.botonLimpiar.TabIndex = 9;
            this.botonLimpiar.Text = "Limpiar";
            this.botonLimpiar.UseVisualStyleBackColor = true;
            this.botonLimpiar.Click += new System.EventHandler(this.botonLimpiar_Click);
            // 
            // agregarNeurona
            // 
            this.agregarNeurona.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.agregarNeurona.Location = new System.Drawing.Point(107, 253);
            this.agregarNeurona.Name = "agregarNeurona";
            this.agregarNeurona.Size = new System.Drawing.Size(118, 23);
            this.agregarNeurona.TabIndex = 6;
            this.agregarNeurona.Text = "Agregar Neurona";
            this.agregarNeurona.UseVisualStyleBackColor = true;
            this.agregarNeurona.Click += new System.EventHandler(this.agregarNeurona_Click);
            // 
            // entradaNeurona
            // 
            this.entradaNeurona.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entradaNeurona.Location = new System.Drawing.Point(12, 255);
            this.entradaNeurona.Name = "entradaNeurona";
            this.entradaNeurona.Size = new System.Drawing.Size(89, 20);
            this.entradaNeurona.TabIndex = 8;
            this.entradaNeurona.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.entradaNeurona_KeyPress);
            // 
            // textoGruposNeuronas
            // 
            this.textoGruposNeuronas.AutoSize = true;
            this.textoGruposNeuronas.Location = new System.Drawing.Point(9, 6);
            this.textoGruposNeuronas.Name = "textoGruposNeuronas";
            this.textoGruposNeuronas.Size = new System.Drawing.Size(105, 13);
            this.textoGruposNeuronas.TabIndex = 7;
            this.textoGruposNeuronas.Text = "Grupos de Neuronas";
            // 
            // gruposDeNeuronas
            // 
            this.gruposDeNeuronas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gruposDeNeuronas.FormattingEnabled = true;
            this.gruposDeNeuronas.Location = new System.Drawing.Point(12, 27);
            this.gruposDeNeuronas.Name = "gruposDeNeuronas";
            this.gruposDeNeuronas.Size = new System.Drawing.Size(294, 212);
            this.gruposDeNeuronas.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label36);
            this.panel1.Controls.Add(this.label35);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.titulo);
            this.panel1.Controls.Add(this.entradaLimiteCiclos);
            this.panel1.Controls.Add(this.limiteCiclos);
            this.panel1.Controls.Add(this.botonEntrenar);
            this.panel1.Controls.Add(this.entradaBeta);
            this.panel1.Controls.Add(this.entradaAlfa);
            this.panel1.Controls.Add(this.beta);
            this.panel1.Controls.Add(this.alfa);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(311, 197);
            this.panel1.TabIndex = 10;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(8, 145);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(93, 13);
            this.label36.TabIndex = 15;
            this.label36.Text = "Fecha de Termino";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(8, 119);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(80, 13);
            this.label35.TabIndex = 14;
            this.label35.Text = "Fecha de Inicio";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(107, 138);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 13;
            this.dateTimePicker2.Value = new System.DateTime(2017, 3, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(107, 112);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 12;
            this.dateTimePicker1.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // titulo
            // 
            this.titulo.AutoSize = true;
            this.titulo.Location = new System.Drawing.Point(9, 9);
            this.titulo.Name = "titulo";
            this.titulo.Size = new System.Drawing.Size(159, 13);
            this.titulo.TabIndex = 7;
            this.titulo.Text = "Configuración del entrenamiento";
            // 
            // entradaLimiteCiclos
            // 
            this.entradaLimiteCiclos.Location = new System.Drawing.Point(107, 81);
            this.entradaLimiteCiclos.Name = "entradaLimiteCiclos";
            this.entradaLimiteCiclos.Size = new System.Drawing.Size(199, 20);
            this.entradaLimiteCiclos.TabIndex = 6;
            this.entradaLimiteCiclos.Text = "500";
            this.entradaLimiteCiclos.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.entradaLimiteCiclos_KeyPress);
            // 
            // limiteCiclos
            // 
            this.limiteCiclos.AutoSize = true;
            this.limiteCiclos.Location = new System.Drawing.Point(8, 88);
            this.limiteCiclos.Name = "limiteCiclos";
            this.limiteCiclos.Size = new System.Drawing.Size(79, 13);
            this.limiteCiclos.TabIndex = 5;
            this.limiteCiclos.Text = "Limite de ciclos";
            // 
            // botonEntrenar
            // 
            this.botonEntrenar.Location = new System.Drawing.Point(107, 164);
            this.botonEntrenar.Name = "botonEntrenar";
            this.botonEntrenar.Size = new System.Drawing.Size(116, 25);
            this.botonEntrenar.TabIndex = 4;
            this.botonEntrenar.Text = "Entrenar";
            this.botonEntrenar.UseVisualStyleBackColor = true;
            this.botonEntrenar.Click += new System.EventHandler(this.botonEntrenar_Click);
            // 
            // entradaBeta
            // 
            this.entradaBeta.Location = new System.Drawing.Point(107, 53);
            this.entradaBeta.Name = "entradaBeta";
            this.entradaBeta.Size = new System.Drawing.Size(199, 20);
            this.entradaBeta.TabIndex = 3;
            this.entradaBeta.Text = "0.00001";
            this.entradaBeta.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.entradaBeta_KeyPress);
            // 
            // entradaAlfa
            // 
            this.entradaAlfa.Location = new System.Drawing.Point(107, 25);
            this.entradaAlfa.Name = "entradaAlfa";
            this.entradaAlfa.Size = new System.Drawing.Size(200, 20);
            this.entradaAlfa.TabIndex = 2;
            this.entradaAlfa.Text = "0.001";
            this.entradaAlfa.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.entradaAlfa_KeyPress);
            // 
            // beta
            // 
            this.beta.AutoSize = true;
            this.beta.Location = new System.Drawing.Point(8, 60);
            this.beta.Name = "beta";
            this.beta.Size = new System.Drawing.Size(29, 13);
            this.beta.TabIndex = 1;
            this.beta.Text = "Beta";
            // 
            // alfa
            // 
            this.alfa.AutoSize = true;
            this.alfa.Location = new System.Drawing.Point(8, 32);
            this.alfa.Name = "alfa";
            this.alfa.Size = new System.Drawing.Size(25, 13);
            this.alfa.TabIndex = 0;
            this.alfa.Text = "Alfa";
            // 
            // pestannaValidacion
            // 
            this.pestannaValidacion.Location = new System.Drawing.Point(4, 22);
            this.pestannaValidacion.Name = "pestannaValidacion";
            this.pestannaValidacion.Padding = new System.Windows.Forms.Padding(3);
            this.pestannaValidacion.Size = new System.Drawing.Size(871, 515);
            this.pestannaValidacion.TabIndex = 1;
            this.pestannaValidacion.Text = "Validación";
            this.pestannaValidacion.UseVisualStyleBackColor = true;
            // 
            // Entrenamiento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 565);
            this.Controls.Add(this.pestannas);
            this.Name = "Entrenamiento";
            this.Text = "Entrenamiento";
            this.pestannas.ResumeLayout(false);
            this.pestannaEntrenamiento.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl pestannas;
        private System.Windows.Forms.TabPage pestannaEntrenamiento;
        private System.Windows.Forms.TabPage pestannaValidacion;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label titulo;
        private System.Windows.Forms.TextBox entradaLimiteCiclos;
        private System.Windows.Forms.Label limiteCiclos;
        private System.Windows.Forms.Button botonEntrenar;
        private System.Windows.Forms.TextBox entradaBeta;
        private System.Windows.Forms.TextBox entradaAlfa;
        private System.Windows.Forms.Label beta;
        private System.Windows.Forms.Label alfa;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button agregarNeurona;
        private System.Windows.Forms.TextBox entradaNeurona;
        private System.Windows.Forms.Label textoGruposNeuronas;
        private System.Windows.Forms.ListBox gruposDeNeuronas;
        private System.Windows.Forms.WebBrowser mapaResultado;
        private System.Windows.Forms.ProgressBar barraDeProgreso;
        private System.Windows.Forms.Button botonLimpiar;
    }
}