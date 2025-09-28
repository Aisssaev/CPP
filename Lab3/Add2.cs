using System;
using System.Windows.Forms;

namespace SimpleWinFormsApp
{
    class Program : Form
    {
        private Button button1;
        private TextBox textBox1;

        public Program()
        {
            // Налаштування форми
            this.Text = "Приклад WinForms";
            this.Width = 300;
            this.Height = 200;

        
            textBox1 = new TextBox();
            textBox1.Location = new System.Drawing.Point(50, 30);
            textBox1.Width = 200;
            textBox1.TextChanged += TextBox1_TextChanged;

            // Створюємо Button
            button1 = new Button();
            button1.Text = "Натисни мене";
            button1.Location = new System.Drawing.Point(50, 70);
            button1.Click += Button1_Click;

            // Додаємо елементи на форму
            this.Controls.Add(textBox1);
            this.Controls.Add(button1);
        }

    
        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ви натиснули кнопку!\nТекст у полі: " + textBox1.Text);
        }

    
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = "Ви ввели: " + textBox1.Text;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new Program());
        }
    }
}
