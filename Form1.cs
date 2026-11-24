using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using WindowsFormsApplication2; // Подключаем библиотеку для Access

namespace WindowsFormsApplication1
{
    public partial class FormLogin : Form
    {
        // Конструктор формы
        public FormLogin()
        {
            InitializeComponent();
        }

        // Кнопка "Войти"
        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text.Trim();
            string password = textBox2.Text.Trim();

            // 1. Проверка на пустоту
            if (login == "" || password == "")
            {
                MessageBox.Show("Введите логин и пароль!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Строка подключения (путь к твоей базе!)
            string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source= E:\bd\Users.mdb";

            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                try
                {
                    dbConnection.Open();

                    // 3. Запрос
                    string query = "SELECT COUNT(*) FROM Users WHERE Login = ? AND Password = ?";

                    OleDbCommand dbCommand = new OleDbCommand(query, dbConnection);

                    dbCommand.Parameters.AddWithValue("?", login);
                    dbCommand.Parameters.AddWithValue("?", password);

                    // 4. Выполняем
                    int count = (int)dbCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Добро пожаловать!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // ПЕРЕХОД НА ДРУГУЮ ФОРМУ (проверь название своей второй формы!)
                        FormOrders f = new FormOrders();
                        f.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox2.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка подключения к БД: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Кнопка "Выход" (если она у тебя есть, она должна называться button2)
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Пустые методы, которые ты создавала двойным кликом по элементам (оставь их пустыми)
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void FormLogin_Load(object sender, EventArgs e) { }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}