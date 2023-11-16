using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBaseConnect;

namespace ООО_СПОРТ
{

    public partial class log_in : Form
    {
        DataBase dataBase = new DataBase();
        private string text = String.Empty;
        private Timer timer;
        private int count = 0;
        int time = 180;
        public log_in()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            button1.Visible = false;
            textBox1.Visible = false;
            button2.Visible = false;
            label4.Visible = false;
        }

        private void log_in_Load(object sender, EventArgs e)
        {
            textBox_pwd.PasswordChar = '•';
        }

        private void button_Enter_Click(object sender, EventArgs e)
        {
            count++;
            var login = textBox_login.Text;
            var pwd = textBox_pwd.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string queryString = $"select ID, Login, Password, Role, ФИО from Users " +
                $"where login = '{login}' and pwd = '{pwd}'";

            SqlCommand command = new SqlCommand(queryString, dataBase.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)
            {

                MessageBox.Show("Вы успешно вошли", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (table.Rows[0].ItemArray[3].ToString() == "Администратор")
                {
                    Administrator administrator = new Administrator($"{table.Rows[0].ItemArray[3].ToString()}, {table.Rows[0].ItemArray[4].ToString()}");
                    this.Hide();
                    administrator.ShowDialog();
                    this.Show();
                    count = 0;
                }
                label3.Text = $"Количество попыток {count}/3";
                if (count == 1)
                {
                    button1.Visible = true;
                    button_Enter.Visible = false;
                    textBox1.Visible = true;
                    textBox_login.Enabled = false;
                    textBox_pwd.Enabled = false;
                    button2.Visible = true;
                    textBox_login.Text = "Введите CAPTCHA!!!";
                    textBox_pwd.Text = "Введите CAPTCHA!!!";
                    pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);
                }
                if (count == 2)
                {
                    label4.Visible = true;
                    timer1.Start();
                    textBox_login.Enabled = false;
                    textBox_pwd.Enabled = false;
                    button_Enter.Enabled = false;

                }
                if (count == 3)
                {
                    Application.Exit();
                }
            }
        }

        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();

            //Создадим изображение
            Bitmap result = new Bitmap(Width, Height);

            //Вычислим позицию текста
            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);

            //Добавим различные цвета
            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };

            //Укажем где рисовать
            Graphics g = Graphics.FromImage(result);

            //Пусть фон картинки будет серым
            g.Clear(Color.Gray);

            //Сгенерируем текст
            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            //Нарисуем сгенирируемый текст
            g.DrawString(text,
                         new Font("Arial", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));

            //Добавим немного помех
            /////Линии из углов
            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));
            ////Белые точки
            //for (int i = 0; i < Width; ++i)
            //    for (int j = 0; j < Height; ++j)
            //        if (rnd.Next() % 20 == 0)
            //            result.SetPixel(i, j, Color.White);

            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            pictureBox1.Image = this.CreateImage(pictureBox1.Width, pictureBox1.Height);


        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == this.text)
            {
                MessageBox.Show("Верно!");
                textBox_login.Enabled = true;
                textBox_pwd.Enabled = true;
                textBox_login.Text = "";
                textBox_pwd.Text = "";
                button_Enter.Visible = true;
                button1.Visible = false;
                textBox1.Visible = false;
                button2.Visible = false;
                pictureBox1.Visible = false;
            }
            else
            {
                MessageBox.Show("ERROR!!!");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            time--;
            label4.Text = $"Осталось: {time}";
            if (time == 0)
            {
                timer1.Stop();
                textBox_login.Enabled = true;
                textBox_pwd.Enabled = true;
                button_Enter.Enabled = true;
                MessageBox.Show("Время закончилось!!!");
            }
        }
    }
}
