using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Algorithms
{
    

    public partial class Form1 : Form
    {
        private delegate int[] AsyncEratosphene(int max);
        private delegate long[] AsyncFibonacci(int n);
        private delegate int AsyncEvklid(int number1, int number2);
        private string eratospheneResult;
        private string fibonacciResult;
        private string evklidResult;

        //Методы для рассчета алгоритмов
        //В каждом методе предусмотрена искусственная задержка на 8 сек для демонстрации работы многопоточности
        #region Eratosphene
        private int[] Eratosphene (int max)
        {
            bool[] simpleNumbers = new bool[max + 1];
            for (int i = 2; i <= max; i++) simpleNumbers[i] = true;

            for (int i = 2; i <= max; i++)
            {
                if (simpleNumbers[i])
                {
                    for (int j = i * 2; j <= max; j += i)
                        simpleNumbers[j] = false;
                }
            }
            int numberOfTrue = 0;
            for (int i = 2; i <= max; i++)
            {
                if (simpleNumbers[i] == true)
                {
                    numberOfTrue++;
                }

            }
            int[] result = new int[numberOfTrue];
            int k = 0;
            for (int i = 2; i <= max; i++)
            {
                if (simpleNumbers[i] == true)
                {
                    result[k] = i;
                    k++;
                }
            }
            System.Threading.Thread.Sleep(8000);
            return result;
        }
        #endregion
        #region Fibonacci
        private long[] Fibonacci(int n)
        {
            long[] result = new long[n];
            result[0] = 0;
            result[1] = 1;
            for (int j = 2; j < n; j++)
            {
                result[j] = result[j - 1] + result[j - 2];
            }
            System.Threading.Thread.Sleep(8000);
            return result;
        }
        #endregion
        #region Evklid
        private int simpleEvklid(int number1, int number2)
        {
            while ((number1 != 0) && (number2 != 0))
            {
                if (number1 > number2)
                    number1 -= number2;
                else
                    number2 -= number1;
            }
            System.Threading.Thread.Sleep(9000);
            return Math.Max(number1, number2);
        }
        #endregion

        //Методы для возврата в основной поток
        private void CallBackEratosphene(IAsyncResult ar)
        {
            AsyncEratosphene eratospheneDelegate = (AsyncEratosphene)ar.AsyncState;
            string str = String.Format("Сумма чисел равна: {0}", string.Join(",", eratospheneDelegate.EndInvoke(ar)));
            this.eratospheneResult = str;
            MethodInvoker mi = new MethodInvoker(this.UpdateUI);
            this.BeginInvoke(mi);
        }
        private void CallBackFibonacci(IAsyncResult ar)
        {
            AsyncFibonacci FibonacciDelegate = (AsyncFibonacci)ar.AsyncState;
            string str = String.Format("Последовательность фибоначчи равна: {0}", string.Join(",", FibonacciDelegate.EndInvoke(ar)));
            this.fibonacciResult = str;
            MethodInvoker mi2 = new MethodInvoker(this.UpdateUI2);
            this.BeginInvoke(mi2);
        }
        private void CallBackEvklid(IAsyncResult ar)
        {
            AsyncEvklid evklidiDelegate = (AsyncEvklid)ar.AsyncState;
            string str = String.Format("Наибольший общий делитель равен: {0}", evklidiDelegate.EndInvoke(ar));
            this.evklidResult = str;
            MethodInvoker mi3 = new MethodInvoker(this.UpdateUI3);
            this.BeginInvoke(mi3);
        }

        //Методы для передачи результатов в соответствующие RichTextBox
        private void UpdateUI()
        {
            this.richTextBox2.Text = this.eratospheneResult;
        }
        private void UpdateUI2()
        {
            this.richTextBox3.Text = this.fibonacciResult;        
        }
        private void UpdateUI3()
        {
            this.richTextBox4.Text = this.evklidResult;
        }

        //Запуск расчета алгоритмов
        private void button1_Click(object sender, EventArgs e)
        {
            int max;
            try
            {
                max = (int)numericUpDown1.Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Введен неверный тип данных. Введите число!");
                numericUpDown1.Value = 0;
                return;
            }

            AsyncEratosphene eratospheneDelegate = new AsyncEratosphene(Eratosphene);
            AsyncCallback cb1 = new AsyncCallback(CallBackEratosphene);
            IAsyncResult asyncResult = eratospheneDelegate.BeginInvoke(max, cb1, eratospheneDelegate);

            

                    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int n;
            try
            {
                n = (int)numericUpDown2.Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Введен неверный тип данных. Введите число!");
                numericUpDown2.Value = 0;
                return;
            }

            AsyncFibonacci FibonacciDelegate = new AsyncFibonacci(Fibonacci);
            AsyncCallback cb2 = new AsyncCallback(CallBackFibonacci);
            IAsyncResult asyncResult = FibonacciDelegate.BeginInvoke(n, cb2, FibonacciDelegate);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int number1, number2;
            try
            {
                number1 = (int)numericUpDown3.Value;
                number2 = (int)numericUpDown4.Value;
            }
            catch (Exception)
            {
                MessageBox.Show("Введен неверный тип данных. Введите число!");
                numericUpDown3.Value = 0;
                numericUpDown4.Value = 0;
                return;
            }

            AsyncEvklid evklidiDelegate = new AsyncEvklid(simpleEvklid);
            AsyncCallback cb3 = new AsyncCallback(CallBackEvklid);
            IAsyncResult asyncResult = evklidiDelegate.BeginInvoke(number1, number2, cb3, evklidiDelegate);
        }
        
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
