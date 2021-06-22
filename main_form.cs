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
    public partial class main_form : Form
    {
        private MySqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;

        public main_form()
        {
            InitializeComponent();
        }

        DB db = new DB();
        // ПОДКЛЮЧЕНИЕ К БД И ВЫВОД ДАННЫХ В DataGridView
        private void LoadData()
        {
            try
            {

                dataAdapter = new MySqlDataAdapter("SELECT `dish_name` AS `Название` FROM `dish`" , db.GetConnection());
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];

                //ДОБАВЛЕНИЕ КНОПОК РЕДАКТИРОВАНИЯ ДАННЫХ
                dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Name = "sost", HeaderText = "Показать состав", Width = 70 });


            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //При открытии формы отображаются блюда
        private void main_form_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //ПРИ НАЖАТИИ НА КНОПКУ 
                if (e.ColumnIndex == 1)
                {
                   
                    string name = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string Sql = ("SELECT product.product_name as 'Состав' FROM product, recipe, dish WHERE recipe.dish_id = dish.id_dish and recipe.product_id = product.id_product AND dish.dish_name = '"+name+"'");
                   
                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView2.DataSource = dataSet.Tables[0];

                }
                

            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            if (textBox1.Text != "")
            {
                //ПРОХОДИТ ПО ВСЕМ ЯЧЕЙКАМ И ИЩЕТ СОВПАДЕНИЕ
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Selected = false;
                   for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null)
                        if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBox1.Text.ToLower()))
                        {
                            dataGridView1.Rows[i].Selected = true;
                            break;
                        }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            sign_in sign_in = new sign_in();
            sign_in.Show();
        }
    }
}
