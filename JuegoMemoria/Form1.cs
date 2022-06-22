using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace JuegoMemoria
{
    public partial class Form1 : Form
    {        
        private Label firstClicked = null;
        private Label secondClicked = null;
        private int Movimientos = 0;
        private int cantMovimientos = 20;
        private int contAcertados = 0;
        private Random random = new Random();
        private List<string> palabras = new List<string>();

    public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();
        }
        

        private void AssignIconsToSquares()
        {
            lecturaArchivoPalabras();
            // El TableLayoutPanel tiene 20 labels y 20 palabras

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(palabras.Count);
                    iconLabel.Text = palabras[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    palabras.RemoveAt(randomNumber);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }
                
                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;



                if (firstClicked.Text == secondClicked.Text)
                {
                    cantMovimientos--;
                    contAcertados++;
                    listBoxAcertados.Items.Add($"{contAcertados} ==> " + firstClicked.Text);
                    firstClicked = null;
                    secondClicked = null;
                    Movimientos++;
                    contMov.Text = Movimientos.ToString();
                    cantMove.Text = cantMovimientos.ToString();
                    if (cantMovimientos == 0)
                    {
                        MessageBox.Show("Se te acabaron los movimientos", "Haz Perdido!");
                        this.Close();
                    }
                    if (contAcertados == 10)
                    {
                        CheckForWinner();
                    }
                    return;
                }
                else
                {
                    cantMovimientos--;
                    cantMove.Text = cantMovimientos.ToString();
                    if (cantMovimientos == 0)
                    {
                        MessageBox.Show("Se te acabaron los movimientos", "Haz Perdido!");
                        this.Close();
                    }
                    Movimientos++;
                    contMov.Text = Movimientos.ToString();
                }

                timer1.Start();
            }
        }
        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            MessageBox.Show("Acertaste todos los iconos!", "FELICITACIONES");
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void lecturaArchivoPalabras()
        {
            string path = Directory.GetCurrentDirectory();
            FileStream fileStream = new FileStream(path+ "/palabras.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fileStream);
            string linea;
            while ((linea = sr.ReadLine()) != null)
            {
                palabras.Add(linea.Substring(1));
            }
        }
    }
}
