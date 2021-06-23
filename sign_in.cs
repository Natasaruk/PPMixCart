using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Амперия
{
    public partial class sign_in : Form
    {
        public sign_in()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string loginU = textBox1.Text;
            string passU = textBox2.Text;

            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT * FROM `client` WHERE `login`= @uL and `pass`=@uP and `role_id`=1", db.GetConnection());
            command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginU;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passU;
            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {//Форма админа
                this.Hide();
                users users = new users();
                users.Show();
            }
                                                                                   
            else
            {
                command = new MySqlCommand("SELECT * FROM `client` WHERE `login`= @uL and `pass`=@uP and `role_id`=2", db.GetConnection());
                command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginU;
                command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passU;
                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {//Форма повара
                    this.Hide();
                    cook cook = new cook();
                    cook.Show();
                }
                else
                {
                    command = new MySqlCommand("SELECT * FROM `client` WHERE `login`= @uL and `pass`=@uP and `role_id`=3", db.GetConnection());
                    command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginU;
                    command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passU;
                    adapter.SelectCommand = command;
                    adapter.Fill(table);

                    if (table.Rows.Count > 0)
                    {//Форма повара
                        this.Hide();
                        storekeeper storekeeper = new storekeeper();
                        storekeeper.Show();
                    }
                    else MessageBox.Show("Вы ввели неверный логин или пароль. Пожалуйста, проверьте ещё раз введенные данные");
                }
            }
                
        }

    }
}
