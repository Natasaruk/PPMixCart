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
    public partial class newProd : Form
    {
        private MySqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        DB db = new DB();
        public newProd()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text.Equals("")) || (textBox2.Text.Equals("")) || (textBox3.Text.Equals("")) || (comboBox1.Text.Equals("")))
            {
                MessageBox.Show("Введите все данные");
            }
            else 
            {
                int unit;
                string name = textBox1.Text;
                string quan = textBox2.Text;
                string pr = textBox3.Text;
                if (comboBox1.SelectedIndex == 0)
                {
                    unit = 1;
                }
                else unit = 2;

                try
                {
                    string Sql = ("INSERT INTO `product` ( `product_name`, `quantity`, `price`, `unit_id`) VALUES ('" + name + "', '" + quan + "', '" + pr + "', '" + unit + "')");
                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    MessageBox.Show("Продукт добавлен", "Успешно!");
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    comboBox1.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
