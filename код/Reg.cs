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
    public partial class Reg : Form
    {
        public Reg()
        {
            InitializeComponent();
        }

        private MySqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        DB db = new DB();

        private void button1_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == false & radioButton2.Checked == false & radioButton3.Checked == false)
            {
                MessageBox.Show("Введите все данные");
            }
            else
            {
                if ((textBox1.Text.Equals("")) || (textBox2.Text.Equals("")) || (textBox3.Text.Equals("")) || (textBox4.Text.Equals("")) || (textBox5.Text.Equals("")) || (textBox6.Text.Equals("")))
                {
                    MessageBox.Show("Введите все данные");
                }
                else
                {

                    string name = textBox1.Text.ToLower();
                    string surname = textBox2.Text.ToLower();
                    string patronymic = textBox3.Text.ToLower();
                    string login = textBox4.Text.ToLower();
                    string pass = textBox5.Text;
                    string phone_number = textBox6.Text;
                    string role = "";
                    if (radioButton1.Checked)
                    {
                        role = "1";
                    }
                    else if (radioButton2.Checked)
                    {
                        role = "3";
                    }
                    else if (radioButton3.Checked)
                    {
                        role = "2";
                    }

                    try
                    {
                        string Sql = ("INSERT INTO `client` (`name`, `surname`, `patronymic`, `login`, `pass`, `phone_number`, `role_id`) VALUES ('" + name + "', '" + surname + "', '" + patronymic + "', '" + login + "', '" + pass + "', '" + phone_number + "', '" + role + "')");
                        dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                        dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        MessageBox.Show("Пользователь зарегистрирован", "Успешно!");
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }
        }
    }
