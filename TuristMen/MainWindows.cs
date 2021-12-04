using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TuristMen
{
    public partial class MainWindows : Form
    {
        private SqlConnection sqlConnection = null;

        public MainWindows()
        {
            InitializeComponent();
        }

        private async void MainWindows_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["BD"].ConnectionString);

            await sqlConnection.OpenAsync();

            if (sqlConnection.State == ConnectionState.Open) { MessageBox.Show("Подключение к базе данных\nУспешно установленно!"); }
            else { MessageBox.Show("Подключение к базе данных\nНе установленно!"); }

            listView1.GridLines = true;
            listView1.FullRowSelect = true;
            listView1.View = View.Details;

            listView1.Columns.Add("ID");
            listView1.Columns.Add("Номер заказа");
            listView1.Columns.Add("Имя");
            listView1.Columns.Add("Фамилия");
            listView1.Columns.Add("Отчество");
            listView1.Columns.Add("Возраст");
            listView1.Columns.Add("Телефон");
            listView1.Columns.Add("Номер паспорта");
            listView1.Columns.Add("Страна");

            await LoadMainAsync();
        }

        private void MainWindows_FormClosing(object sender, FormClosingEventArgs e){
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed) sqlConnection.Close();
        }

        private async Task LoadMainAsync(){
            SqlDataReader sqlDataReader = null;

            SqlCommand getMainCommand = new SqlCommand("SELECT * FROM [Main]", sqlConnection);

            try{
                sqlDataReader = await getMainCommand.ExecuteReaderAsync();

                while (await sqlDataReader.ReadAsync())
                {
                       ListViewItem item = new ListViewItem(new string[]
                    {
                        Convert.ToString(sqlDataReader["Id"]),
                        Convert.ToString(sqlDataReader["ordernumber"]),
                        Convert.ToString(sqlDataReader["name"]),
                        Convert.ToString(sqlDataReader["surname"]),
                        Convert.ToString(sqlDataReader["middlename"]),
                        Convert.ToString(sqlDataReader["age"]),
                        Convert.ToString(sqlDataReader["phone"]),
                        Convert.ToString(sqlDataReader["passport"]),
                        Convert.ToString(sqlDataReader["country"])
                    });
                    listView1.Items.Add(item);
                }
            }
            catch(Exception ex){
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally{
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }
        }

        private async void toolStripButton1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            await LoadMainAsync();
        }

        private void insert_Click(object sender, EventArgs e)
        {
            Insert insert = new Insert(sqlConnection);

            insert.Show();
        }

        private void update_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Update update = new Update(sqlConnection, Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text));

                update.Show();
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private async void Delete_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count > 0)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить эту строку?", "Удаление строки", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                switch (res)
                {
                    case DialogResult.OK:
                        SqlCommand deleteMainCommand = new SqlCommand("DELETE FROM [Main] WHERE [Id]=@id", sqlConnection);

                        deleteMainCommand.Parameters.AddWithValue("id", Convert.ToInt32(listView1.SelectedItems[0].SubItems[0].Text));

                        try
                        {
                            await deleteMainCommand.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        listView1.Items.Clear();
                        await LoadMainAsync();

                        break;
                }
            }
            else
            {
                MessageBox.Show("Ни одна строка не была выделена!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
