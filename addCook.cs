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
    public partial class addCook : Form
    {
        private MySqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        DB db = new DB();

        public addCook()
        {
            InitializeComponent();
        }

        private void LoadData()
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

        private void LoadData1()
        {
            try
            {
                dataAdapter = new MySqlDataAdapter("SELECT `id_product` AS `Номер продукта`,`product_name` AS `Название` FROM `product` ", db.GetConnection());
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView2.DataSource = dataSet.Tables[0];
                //ДОБАВЛЕНИЕ КНОПОК РЕДАКТИРОВАНИЯ ДАННЫХ
                dataGridView2.Columns.Add(new DataGridViewButtonColumn() { Name = "insert", HeaderText = "Добавить", Width = 70 });

            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addCook_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.ClearSelection();
            LoadData1();
            dataGridView2.ClearSelection();
        }

        
        private void addCook_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            cook cook = new cook();
            cook.Show();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = dataGridView1.CurrentCell.RowIndex;
            try
            {
                //ПРИ НАЖАТИИ НА КНОПКУ 
                if (e.ColumnIndex == 2)
                {
                    string id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string id_p = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string Sql = ("INSERT INTO `recipe` (`product_id`, `dish_id`) VALUES ('" + id_p + "', '" + id + "')");

                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    MessageBox.Show("Продукт добавлен", "Успешно!");
                    ReolaData1();
                }
            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception)
            {
                MessageBox.Show("Продукт уже добаввлен", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReolaData()
        {
            try
            {
                dataAdapter = new MySqlDataAdapter("SELECT `id_product` AS `Номер продукта`,`product_name` AS `Название` FROM `product` ", db.GetConnection());
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView2.DataSource = dataSet.Tables[0];
            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.ClearSelection();
            ReolaData();
            dataGridView2.ClearSelection();
        }

 
        public string id;

        private void ReolaData1()
        {
            try
            {
                int i = dataGridView1.CurrentCell.RowIndex;
                id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                string Sql = ("SELECT product.id_product AS'Номер продукта',product.product_name as 'Состав' ,recipe.amount_per_serving as 'На порцию' FROM product, recipe, dish WHERE recipe.dish_id = dish.id_dish and recipe.product_id = product.id_product AND dish.dish_name = '" + name + "'");

                dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView3.DataSource = dataSet.Tables[0]; ;

            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            ReolaData1();
        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentCell.RowIndex;
            int j = dataGridView3.CurrentCell.RowIndex;

            try
            {
                if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                {
                    string name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string name1 = dataGridView3.Rows[j].Cells[0].Value.ToString();
                    string Sql = ("DELETE FROM recipe WHERE recipe.product_id = "+name1+" AND recipe.dish_id = "+name);
                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    ReolaData1();
                }
            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView3.RowCount; i++)
                {
                    string num = dataGridView3.Rows[i].Cells[0].Value.ToString();
                    string amount = dataGridView3.Rows[i].Cells[2].Value.ToString();
                    string Sql = ("UPDATE recipe SET recipe.amount_per_serving='" + amount + "' WHERE recipe.dish_id = '" + id + "' AND recipe.product_id='" + num + "'");
                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                }
                MessageBox.Show("Данные изменены", "Успешно!");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
               
    }

}