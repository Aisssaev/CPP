using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KPP_Lab_6
{
    public partial class Form1 : Form
    {
        int N = 1; // Розмірність системи
        int i = 0;
        int j = 0;
        int Change;
        double[,] A = new double[6, 6]; // Матриця А
        double[] B = new double[6]; // Вектор правої частини СЛАР
        double[] X = new double[6]; // Вектор розв'язку СЛАР


        public Form1()
        {
            InitializeComponent();
        }

        private void Kramer(int N)
        {
            double a1 = A[1, 1], a2 = A[1, 2], a3 = A[1, 3], a4 = A[2, 1], a5 = A[2, 2],
                a6 = A[2, 3], a7 = A[3, 1], a8 = A[3, 2], a9 = A[3, 3], b1 = B[1], b2 = B[2], b3 = B[3];
            double Det = 0, Detx = 0,Dety = 0,Detz = 0; // оголошуємо змінні для визначників
           
               if(N == 1)   // обраховуємо визначники відповідного порядку
            {
                        Det = a1;
                        Detx = b1;
                    }
               if(N == 2)  
            {
                Det= a1 * a5 - a2 * a4;
                Detx= b1 * a5 - a2 * b2;
                Dety= a1 * b2 - a4 * b1;
            }
             if(N == 3)
            {
            Det= a1 * a5 * a9 + a2 * a6 * a7 + a8 * a4 * a3 - a3 * a5 * a7 - a2 * a4 * a9 - a8 * a6 * a1;
            Detx= b1 * a5 * a9 + a2 * a6 * b3 + a8 * b2 * a3 - a3 * a5 * b3 - a2 * b2 * a9 - a8 * a6 * b1;
            Dety= a1 * b2 * a9 + b1 * a6 * a7 + b3 * a4 * a3 - a3 * b2 * a7 - b1 * a4 * a9 - b3 * a6 * a1;
            Detz= a1 * a5 * b3 + a2 * b2 * a7 + a8 * a4 * b1 - b1 * a5 * a7 - a2 * a4 * b3 - a8 * b2 * a1;
            }

            if (Det == 0)
                MessageBox.Show("Визначник матриці = 0");
            else
            {
                X[1]= Detx / Det;   // Обчислюємо і записуємо розв'язок
                X[2]= Dety / Det;
                X[3]= Detz / Det;
            }



         
        }

        private void Decomp(int N, ref int Change)
        {
            int i, j, k;
            double R, L, U;
            Change = 1;
            R = Math.Abs(A[1, 1]);
            switch (toolStripComboBox1.SelectedIndex)
            { // add
                case 0:     // Метод LU - перетворень
                    {
                        for (j = 2; j <= N; j++)
                        {
                            if (Math.Abs(A[1,1]) >= R)
                            {
                                Change = j;
                                R = Math.Abs(A[j,1]);
                            }
                        }
                        if (R <= 1e-7)
                        {
                            MessageBox.Show("Система є виродженою");
                            return;
                        }
                        if (Change != 1)
                        {
                            for (i = 1; i <= N; i++)
                            {
                                R = A[Change,i];
                                A[Change,i] = A[1,i];
                                A[1,i] = R;
                            }
                        }
                        for (i = 3; i <= N; i++) // Замінити 3 на 2 (інакше помилка) 
                        {
                            A[1,i] = A[1,i] / A[1,1];
                        }
                        //Якщо ти це не стер, то подумай скільки в тебе IQ?
                        for (i = 2; i <= N; i++)
                        {
                            for (k = i; k <= N; k++)
                            {
                                R = 0;
                                for (j = 1; j <= (i - 1); j++)
                                {
                                    R = R + A[k,j] * A[j,i];
                                }
                                A[k,i] = A[k,i] - R;
                            }
                            if (Math.Abs(A[i, i]) <= 1e-7)
                            {
                                MessageBox.Show("Система є виродженою");
                                return;
                            }
                            for (k = i + 1; k <= N; k++)
                            {
                                R = 0;
                                for (j = 1; j <= (i - 1); j++)
                                {
                                    R = R + A[i,j] * A[j,k];
                                }
                                A[i,k] = (A[i,k] - R) / A[i,i];
                            }

                        }
                    }
                    break;
                case 1:
                    { // add    // Прямий хід методу Гауса
                        for (i = 1; i <= N - 1; i++)
                        {
                            k = i;
                            R = Math.Abs(A[i, i]);
                            for (j = i + 1; j <= N; j++)
                                if (Math.Abs(A[j, i]) >= R)
                                {

                                    k = j;
                                    R = Math.Abs(A[j,i]);
                                }
                            if (R <= 1e-7)
                            {

                                MessageBox.Show("Система є виродженою");
                                return;
                            }
                            if (k != i)
                            {
                                R = B[k];
                                B[k] = B[i];
                                B[i] = R;

                                for (j = i; j <= N; j++)
                                {

                                    R = A[k,j];
                                    A[k,j] = A[i,j];
                                    A[i,j] = R;
                                }
                            }
                            R = A[i,i];
                            B[i] = B[i] / R;

                            for (j = 1; j <= N; j++)
                                A[i,j] = A[i,j] / R;
                            for (k = i + 1; k <= N; k++)
                            {
                                R = A[k,i];
                                B[k] = B[k] - R * B[i];
                                A[k,i] = 0;
                                for (j = i + 1; j <= N; j++)
                                    A[k,j] = A[k,j] - R * A[i,j];
                            }
                        }
                        if (Math.Abs(A[N,N]) <= 1e-7)
                        {

                            MessageBox.Show("Система є виродженою");
                            return;
                        }
                    }
                    break;

            }


            // Виведення результату зведення матриці А у таблицю С
            if (N>1)
            for (i = 0; i < N; i++)
                for (j = 0; j < N; j++)
                {
                    C_Matrix_dgv[j,i].Value = Convert.ToString(A[i + 1,j + 1]);
                }   

        } // Завершення тіла методу Decomp
        private void Solve(int Change, int N)
        {
            int i = 0, j = 0;
            double R;
            switch (toolStripComboBox1.SelectedIndex)    // Перевіряємо обраний метод
            { // add
                case 0:
                    {
                        if (Change != 1)
                        {
                            R = B[Change];
                            B[Change] = B[1];
                            B[1] = R;
                        }
                        B[1] = B[1] / A[1,1];
                        for (i = 2; i <= N; i++)
                        {
                            R = 0;
                            for (j = 1; j <= i - 1; j++)
                            {
                                R = R + A[i,j] * B[j];
                            }
                            B[i] = (B[i] - R) / A[i,i];
                        }
                        X[N] = B[N];
                    }
                    break;
                case 1:
                    {
                        if (N == 1)
                        {

                            if (Math.Abs(A[1,1]) < 1e-7)
                            {
                                MessageBox.Show("Система є виродженою");   //add
                                return;
                            }
                            else X[1] = B[1] / A[1,1]; break;
                        }
                        X[N] = B[N] / A[N,N];
                    }
                    break;
            }

            for (i = 1; i <= N - 1; i++)    // Зворотній хід методу Гауса
            {
                R = 0;
                for (j = N + 1 - i; j <= N; j++)
                {
                    R = R + A[N - i,j] * X[j];
                }
                X[N - i] = B[N - i] - R;
            }
        }// Завершення тіла методу Solve

        private void BCreateGrid_Click(object sender, EventArgs e)
        {
            bool exc_A = false;
            bool exc_B = false;
            for (i = 1; i <= N; i++)
                for (j = 1; j <= N; j++)    // Зчитуємо та перевіряємо введені дані
                {
                    try
                    {
                        A[i, j] = Convert.ToDouble(A_Matrix_dgv[j - 1, i - 1].Value);
                    }
                    catch
                    {
                        A_Matrix_dgv[j - 1, i - 1].Style.ForeColor = Color.Red;
                        exc_A = true;
                    }
                }
            for (j = 0; j < N; j++)
            {
                try
                {
                    B[j + 1] = Convert.ToDouble(B_vector_dgv[0, j].Value);
                }
                catch
                {
                    B_vector_dgv[0, j].Style.ForeColor = Color.Red;
                    exc_B = true;
                }
            }
            if (exc_A || exc_B)
            {
                MessageBox.Show("Помилка введення!");
                return;
            }

            if (toolStripComboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Вкажіть правильний метод"); // add
                toolStripComboBox1.BackColor = Color.Red;
                return;
            }
            switch (toolStripComboBox1.SelectedIndex)    // Перевірка обраного методу
            {
                case 0: label5.Text = "Згенерована LU-матриця"; break;
                case 1: label5.Text = "Згенерована східчаста матриця"; break; // add
            }
            if (toolStripComboBox1.SelectedIndex == 2)
            {
                if (N <= 3)
                    Kramer(N);
                else
                {
                    MessageBox.Show("Максимальний розмір системи для методу Крамера N = 3");
                    return; 
                }
             }
            else
            {
                Decomp(N, ref Change); // Виклик методу Decomp
                Solve(Change, N); // Виклик методу Solve
            }
            
                for (i = 0; i < N; i++)
                    X_vector_dgv[0, i].Value = X[i + 1].ToString("e");
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            X_vector_dgv.ReadOnly = true; // Заборона введення даних у стовпець розв'язків
                                          // Заборона додавати рядки у гріди матриці А та векторів В та Х.
            A_Matrix_dgv.AllowUserToAddRows = false;
            B_vector_dgv.AllowUserToAddRows = false;
            X_vector_dgv.AllowUserToAddRows = false;
            // Кількість стовпців і рядків матриці та векторів встановимо = 1
            A_Matrix_dgv.ColumnCount = 1;
            A_Matrix_dgv.RowCount = 1;
            X_vector_dgv.ColumnCount = 1;
            X_vector_dgv.RowCount = 1;
            B_vector_dgv.ColumnCount = 1;
            B_vector_dgv.RowCount = 1;
        }

        private void NUD_rozmir_ValueChanged(object sender, EventArgs e)
        {
            N = Convert.ToInt16(NUD_rozmir.Value);  // Встановлюємо розмірність таблиці
            A_Matrix_dgv.RowCount = N;
            A_Matrix_dgv.ColumnCount = N;
            X_vector_dgv.RowCount = N;
            B_vector_dgv.RowCount = N;
            C_Matrix_dgv.RowCount = N;
            C_Matrix_dgv.ColumnCount = N;
        }

        private void BClear_Click(object sender, EventArgs e)
        {
            for (i = 0; i < N; i++)     // Очищаємо таблиці
                for (j = 0; j < N; j++)
                {
                    A_Matrix_dgv[j, i].Value = "";
                    C_Matrix_dgv[j, i].Value = "";
                }
            for (j = 0; j < N; j++)
            {
                B_vector_dgv[0, j].Value = "";
                X_vector_dgv[0, j].Value = "";
            }
        }

        private void BClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void A_Matrix_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            A_Matrix_dgv.CurrentCell.Style.ForeColor = Color.Black; // Змінюємо колір на чорний
        }

        private void B_vector_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            B_vector_dgv.CurrentCell.Style.ForeColor = Color.Black;
        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            toolStripComboBox1.BackColor = Color.White;
        }
    }
}
