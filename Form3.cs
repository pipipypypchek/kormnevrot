using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form3 : Form
    {
        string connStr = @"Provider=Microsoft.ACE.OLEDB.12.0;
                           Data Source=" + Application.StartupPath + @"\Orders.accdb";

        public Form3()
        {
            InitializeComponent();

            // Привязываем кнопки вручную (чтобы не лезть в дизайнер)
            this.button1.Click += new EventHandler(button1_Click);
            this.button3.Click += new EventHandler(button3_Click);
            this.button4.Click += new EventHandler(button4_Click);

            // Сразу загружаем данные при открытии (код один и тот же, поэтому дублируем)
            LoadTheData();
        }

        // Наш метод загрузки. Он простой, студия его "съест"
        private void LoadTheData()
        {
            try
            {
                var dt = new DataTable();
                using (var conn = new OleDbConnection(connStr))
                {
                    conn.Open();
                    string query = "SELECT [Время] AS [Дата], [Клиент], 'Пицца' AS [Пицца], [Сумма] FROM [Orders]";
                    new OleDbDataAdapter(query, conn).Fill(dt);
                }

                dataGridView1.DataSource = dt;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                decimal total = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Сумма"] != DBNull.Value)
                        total += Convert.ToDecimal(row["Сумма"]);
                }
                label3.Text = "Итого: " + total.ToString("C");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка базы: " + ex.Message);
            }
        }

        // Твой исходный метод, оставляем пустым
        private void Form3_Load(object sender, EventArgs e)
        {
        }

        // Кнопка Сформировать
        private void button1_Click(object sender, EventArgs e)
        {
            LoadTheData();
        }

        // Кнопка Обновить
        private void button3_Click(object sender, EventArgs e)
        {
            LoadTheData();
        }

        // Кнопка Назад
        private void button4_Click(object sender, EventArgs e)
        {
            WindowsFormsApplication2.FormOrders ordersForm = new WindowsFormsApplication2.FormOrders();
            ordersForm.Show();
            this.Close();
        }
    }
}