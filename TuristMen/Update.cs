using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data;
using System.Data.SqlClient;

namespace TuristMen
{
    public partial class Update : Form
    {
        private SqlConnection sqlConnection = null;
        private int id;

        public Update(SqlConnection connection, int id)
        {
            InitializeComponent();

            sqlConnection = connection;

            this.id = id;
        }

        private async void Update_Load(object sender, EventArgs e)
        {
            SqlCommand getMainInfoCommand = new SqlCommand("SELECT [ordernumber], [name], [surname], [middlename], [age], [phone], [passport], [country] " +
                "FROM [Main] WHERE [Id]=@id", sqlConnection);

            getMainInfoCommand.Parameters.AddWithValue("Id", id);

            SqlDataReader sqlDataReader = null;

            try
            {
                sqlDataReader = await getMainInfoCommand.ExecuteReaderAsync();

                while(await sqlDataReader.ReadAsync())
                {
                    ordernumber_update.Text = Convert.ToString(sqlDataReader["ordernumber"]);
                    name_update.Text = Convert.ToString(sqlDataReader["name"]);
                    surname_update.Text = Convert.ToString(sqlDataReader["surname"]);
                    middlename_update.Text = Convert.ToString(sqlDataReader["middlename"]);
                    age_update.Text = Convert.ToString(sqlDataReader["age"]);
                    phone_update.Text = Convert.ToString(sqlDataReader["phone"]);
                    passport_update.Text = Convert.ToString(sqlDataReader["passport"]);
                    country_update.Text = Convert.ToString(sqlDataReader["country"]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlDataReader != null && !sqlDataReader.IsClosed)
                {
                    sqlDataReader.Close();
                }
            }
        }

        private async void update_button_Click(object sender, EventArgs e)
        {
            SqlCommand updateMainCommand = new SqlCommand("UPDATE [Main] SET [ordernumber]=@ordernumber, " +
                "[name]=@name, " +
                "[surname]=@surname, " +
                "[middlename]=@middlename, " +
                "[age]=@age, " +
                "[phone]=@phone, " +
                "[passport]=@passport, " +
                "[country]=@country " +
                "WHERE [Id]=@id", sqlConnection);

            updateMainCommand.Parameters.AddWithValue("id", id);
            updateMainCommand.Parameters.AddWithValue("ordernumber", ordernumber_update.Text);
            updateMainCommand.Parameters.AddWithValue("name", name_update.Text);
            updateMainCommand.Parameters.AddWithValue("surname", surname_update.Text);
            updateMainCommand.Parameters.AddWithValue("middlename", middlename_update.Text);
            updateMainCommand.Parameters.AddWithValue("age", age_update.Text);
            updateMainCommand.Parameters.AddWithValue("phone", phone_update.Text);
            updateMainCommand.Parameters.AddWithValue("passport", passport_update.Text);
            updateMainCommand.Parameters.AddWithValue("country", country_update.Text);

            try
            {
                await updateMainCommand.ExecuteNonQueryAsync();

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cancellation_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
