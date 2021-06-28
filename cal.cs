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
    public partial class cal : Form
    {
        private MySqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        DB db = new DB();

        public cal()
        {
            InitializeComponent();
        }

        private void ReolaData()
        {
            try
            {
                dataAdapter = new MySqlDataAdapter("SELECT `id_dish` AS `Номер блюда`, `dish_name` AS `Название` FROM `dish`", db.GetConnection());
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];

            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string id;
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentCell.RowIndex;
            id = dataGridView1.Rows[i].Cells[0].Value.ToString();
            string name = dataGridView1.Rows[i].Cells[1].Value.ToString();
            string Sql = ("SELECT product.id_product AS'Номер продукта',product.product_name as 'Состав' ,recipe.amount_per_serving as 'На порцию' FROM product, recipe, dish WHERE recipe.dish_id = dish.id_dish and recipe.product_id = product.id_product AND dish.dish_name = '" + name + "'");

            dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
            dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            dataGridView2.DataSource = dataSet.Tables[0];
        }

        private void cal_Load(object sender, EventArgs e)
        {
            ReolaData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                int c = Convert.ToInt32(textBox1.Text);
                int i = dataGridView1.CurrentCell.RowIndex;
                id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                string Sql = ("SELECT product.id_product AS'Номер продукта', product.product_name as 'Состав' , (recipe.amount_per_serving *" + c + ") as 'На порцию' FROM product, recipe, dish WHERE recipe.dish_id = dish.id_dish and recipe.product_id = product.id_product AND dish.dish_name = '" + name + "'");

                dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView2.DataSource = dataSet.Tables[0];
            }
            else { MessageBox.Show("Введите количество порций"); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                int j= dataGridView1.CurrentCell.RowIndex;
                string dish_id = dataGridView1.Rows[j].Cells[0].Value.ToString();
                string c = textBox1.Text;

                try
                {
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        string id = dataGridView2.Rows[i].Cells[0].Value.ToString();
                        string Sql = ("UPDATE product set product.quantity = (product.quantity - (SELECT(recipe.amount_per_serving * " + c + ") FROM recipe WHERE recipe.dish_id=" + dish_id + " AND recipe.product_id = " + id + ")) WHERE product.id_product = " + id);
                        dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                        dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                    }
                    MessageBox.Show("Продукты списаны со склада", "Успешно!");
                }
                catch (Exception)
                {

                    MessageBox.Show("Недостаточно продуктов для приготовления", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            { 
                MessageBox.Show("Введите количество порций"); 
            }
        }

        
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView3.ColumnHeadersVisible = false;
            dataGridView3.RowHeadersVisible = false;
            

            if (textBox1.Text != "")
            {
                int i = dataGridView1.CurrentCell.RowIndex;
                string dish_id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string c = textBox1.Text;

                try
                {
                   
                    string Sql = ("SELECT SUM(recipe.amount_per_serving * product.price)*"+c+" as 'стоимость' FROM recipe,product WHERE recipe.product_id = product.id_product AND recipe.dish_id ="+dish_id);

                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView3.DataSource = dataSet.Tables[0];
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите количество порций");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            if (textBox2.Text != "")
            {
                //ПРОХОДИТ ПО ВСЕМ ЯЧЕЙКАМ И ИЩЕТ СОВПАДЕНИЕ
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Selected = false;
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        if (dataGridView1.Rows[i].Cells[j].Value != null)
                            if (dataGridView1.Rows[i].Cells[j].Value.ToString().Contains(textBox2.Text.ToLower()))
                            {
                                dataGridView1.Rows[i].Selected = true;
                                break;
                            }
                }
                int rowsCount = dataGridView2.Rows.Count;
                for (int j = 0; j < rowsCount; j++)
                {
                    dataGridView2.Rows.Remove(dataGridView2.Rows[0]);
                }
            }
        }
    }
}
