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
    public partial class cook : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        private MySqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;
        DB db = new DB();

        public cook()
        {
            InitializeComponent();
            random = new Random();
            buttonUser.Visible = true;
        }
        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemeColor.ColorList.Count);
            }
            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }

        private void ActivateButton(object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != (Button)btnSender)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    panelTitleBar.BackColor = color;
                    panelLogo.BackColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    buttonUser.Visible = true;

                }
            }
        }

        private void DisableButton()
        {
            foreach (Control previousBtn in panelMenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                }
            }
        }

        private void OpenChildForm(Form childForm, object btnSender)
        {
            if (activeForm != null)
                activeForm.Close();
            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktopPane.Controls.Add(childForm);
            this.panelDesktopPane.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            label1.Text = childForm.Text;
        }

        private void buttonUser_Click(object sender, EventArgs e)
        {
            if (activeForm != null)
                activeForm.Close();
            Reset();
            ActivateButton(sender);
        }
        private void Reset()
        {
            DisableButton();
            label1.Text = "БЛЮДА";
            panelTitleBar.BackColor = Color.FromArgb(0, 150, 136);
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            buttonUser.Visible = true;
            
        }

        private void buttonReg_Click(object sender, EventArgs e)
        {
            
            OpenChildForm(new addCook(), sender);
        }

        private void cook_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Form ifrm = Application.OpenForms[0];
            //ifrm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenChildForm(new cal(), sender);
        }

        private void button2_Click(object sender, EventArgs e)
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
        private void LoadData()
        {
            try
            {

                dataAdapter = new MySqlDataAdapter("SELECT `id_dish` AS `Номер блюда`, `dish_name` AS `Название` FROM `dish`", db.GetConnection());
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
        private void ReolaData()
        {
            try
            {
                dataAdapter = new MySqlDataAdapter("SELECT `id_dish` AS `Номер блюда`, `dish_name` AS `Название` FROM `dish`", db.GetConnection()); dataSet = new DataSet();
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

        
        private void cook_Load(object sender, EventArgs e)
        {
            LoadData();
            dataGridView1.ClearSelection();
        }
        public string id;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //ПРИ НАЖАТИИ НА КНОПКУ 
                if (e.ColumnIndex == 2)
                {
                    id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string name = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string Sql = ("SELECT product.id_product AS'Номер продукта',product.product_name as 'Состав' ,recipe.amount_per_serving as 'На порцию' FROM product, recipe, dish WHERE recipe.dish_id = dish.id_dish and recipe.product_id = product.id_product AND dish.dish_name = '" + name + "'");

                    dataAdapter = new MySqlDataAdapter(Sql, db.GetConnection());
                    dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    dataGridView2.DataSource = dataSet.Tables[0];
                   


                }
                
                dataGridView1.ClearSelection();


            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReolaData();

            dataGridView2.Refresh();
        }

        

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    string num = dataGridView2.Rows[i].Cells[0].Value.ToString();
                    string amount = dataGridView2.Rows[i].Cells[2].Value.ToString();
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

