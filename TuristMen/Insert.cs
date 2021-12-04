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
    public partial class Insert : Form
    {
        private SqlConnection sqlConnection = null;

        public Insert(SqlConnection connection)
        {
            InitializeComponent();

            sqlConnection = connection;
        }

        private async void add_Click(object sender, EventArgs e)
        {
            SqlCommand insertMainCommand = new SqlCommand("INSERT INTO [Main] " +
                "(ordernumber, name, surname, middlename, age, phone, passport, country)" +
                "VALUES(@ordernumber, @name, @surname, @middlename, @age, @phone, @passport, @country)", sqlConnection);

            insertMainCommand.Parameters.AddWithValue("ordernumber", ordernumber_insert.Text);
            insertMainCommand.Parameters.AddWithValue("name", name_insert.Text);
            insertMainCommand.Parameters.AddWithValue("surname", surname_insert.Text);
            insertMainCommand.Parameters.AddWithValue("middlename", middlename_insert.Text);
            insertMainCommand.Parameters.AddWithValue("age", age_insert.Text);
            insertMainCommand.Parameters.AddWithValue("phone", phone_insert.Text);
            insertMainCommand.Parameters.AddWithValue("passport", passport_insert.Text);
            insertMainCommand.Parameters.AddWithValue("country", country_insert.Text);

            try
            {
                await insertMainCommand.ExecuteNonQueryAsync();

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
