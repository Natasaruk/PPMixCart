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
                dataGrid.DataSource = dataSet.Tables[0];

                //ДОБАВЛЕНИЕ КНОПОК РЕДАКТИРОВАНИЯ ДАННЫХ
                dataGrid.Columns.Add(new DataGridViewButtonColumn() { Name = "del", HeaderText = "Удалить", Width = 70 });


            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ReolaData()
        {
            try
            {
                dataAdapter = new MySqlDataAdapter("SELECT `id_dish` AS `Номер блюда`, `dish_name` AS `Название` FROM `dish`", db.GetConnection());
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGrid.DataSource = dataSet.Tables[0];


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
            dataGrid.ClearSelection();
            LoadData1();
            dataGridView2.ClearSelection();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                MessageBox.Show("Введите название блюда");
            }
            else
            {

                string name = textBox1.Text.ToLower();

                try
                {

                    string Sql = ("INSERT INTO `dish` ( `dish_name`) VALUES ('" + name + "')");
                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    MessageBox.Show("Блюдо добавлено", "Успешно!");
                    textBox1.Text = "";
                    ReolaData();


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void dataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                //ПРИ НАЖАТИИ НА КНОПКУ "УДАЛИТЬ"
                if (e.ColumnIndex == 2)
                {
                    //СООБЩЕНИЕ ОБ УДАЛЕНИИ
                    if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                    {
                        string name = dataGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
                        string Sql = ("DELETE FROM dish WHERE dish.id_dish = " + "'" + name + "'");
                        dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                        dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        ReolaData();

                    }
                }

            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addCook_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
        }



        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int i = dataGrid.CurrentCell.RowIndex;


            MessageBox.Show(i.ToString());


            try
            {
                //ПРИ НАЖАТИИ НА КНОПКУ 
                if (e.ColumnIndex == 2)
                {
                    string id = dataGrid.Rows[i].Cells[0].Value.ToString();
                    string id_p = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string Sql = ("INSERT INTO `recipe` (`product_id`, `dish_id`) VALUES ('" + id_p + "', '" + id + "')");

                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    MessageBox.Show("Продукт добавлен", "Успешно!");
                }
            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           
        }


    }
}
