using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;   // <-- добавил только это

namespace WindowsFormsApplication2
{
    public partial class FormOrders : Form
    {
        // <-- добавлено: путь к базе
        string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;
                           Data Source=" + Application.StartupPath + @"\Orders.accdb";

        public FormOrders()
        {
            InitializeComponent();
            LoadData();   // <-- добавлено
        }

        // <-- добавлено: загрузка данных в таблицу
        void LoadData()
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    new OleDbDataAdapter("SELECT * FROM [Orders]", conn).Fill(dt);
                }
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных:\n" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowsFormsApplication3.Form3 form3 = new WindowsFormsApplication3.Form3();
            form3.Show();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        // <-- добавлено: кнопка "Удалить" (button2)
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для удаления.");
                return;
            }

            string client = Convert.ToString(dataGridView1.CurrentRow.Cells["Клиент"].Value);
            string address = Convert.ToString(dataGridView1.CurrentRow.Cells["Адрес"].Value);
            string time = Convert.ToString(dataGridView1.CurrentRow.Cells["Время"].Value);

            try
            {
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    var cmd = new OleDbCommand(
                        "DELETE FROM [Orders] WHERE [Клиент]=? AND [Адрес]=? AND [Время]=?", conn);
                    cmd.Parameters.AddWithValue("", client);
                    cmd.Parameters.AddWithValue("", address);
                    cmd.Parameters.AddWithValue("", time);
                    cmd.ExecuteNonQuery();
                }

                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления:\n" + ex.Message);
            }
        }

        // <-- добавлено: кнопка "Добавить" (button3)
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            try
            {
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    var cmd = new OleDbCommand(
                        "INSERT INTO [Orders] ([Клиент],[Адрес],[Время],[Сумма]) VALUES (?,?,?,?)",
                        conn);

                    cmd.Parameters.AddWithValue("", textBox1.Text.Trim()); // Клиент
                    cmd.Parameters.AddWithValue("", textBox2.Text.Trim()); // Адрес
                    cmd.Parameters.AddWithValue("", textBox3.Text.Trim()); // Время
                    cmd.Parameters.AddWithValue("", textBox4.Text.Trim()); // Сумма
                    cmd.ExecuteNonQuery();
                }

                LoadData();

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                textBox1.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка добавления:\n" + ex.Message);
            }
        }
    }
}