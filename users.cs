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
    public partial class users : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        private MySqlDataAdapter dataAdapter = null;
        private DataSet dataSet = null;

        public users()
        {
            InitializeComponent();
            random = new Random();
            buttonProd.Visible = true;
        }


        DB db = new DB();
        // ПОДКЛЮЧЕНИЕ К БД И ВЫВОД ДАННЫХ В DataGridView
        private void LoadData()
        {
            try
            {
                dataAdapter = new MySqlDataAdapter("SELECT `id` AS `Номер пользователя`,`surname` AS `Фамилия`, `name`AS `Имя`,`patronymic`AS `Отчество`,`login`AS `Логин`,`phone_number`AS `Номер телефона` ,`role_name`AS `Роль` FROM `client` , `role` WHERE `role_id`=`id_role`", db.GetConnection());
                dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dataGridView1.DataSource = dataSet.Tables[0];

                //ДОБАВЛЕНИЕ КНОПОК РЕДАКТИРОВАНИЯ ДАННЫХ
                dataGridView1.Columns.Add(new DataGridViewButtonColumn() { Name = "delete", HeaderText = "УДАЛИТЬ", Width = 70 });


            }
            //СООБЩЕНИЕ ОБ ОШИБКЕ
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //ОБНОВЛЕНИЕ БАЗЫ ДАННЫХ
        private void ReolaData()
        {
            try
            {
                dataAdapter = new MySqlDataAdapter("SELECT `id` AS `Номер пользователя`,`surname` AS `Фамилия`, `name`AS `Имя`,`patronymic`AS `Отчество`,`login`AS `Логин`,`phone_number`AS `Номер телефона` ,`role_name`AS `Роль` FROM `client` , `role` WHERE `role_id`=`id_role`", db.GetConnection());
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
                    buttonProd.Visible = true;

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

        private void Reset()
        {
            DisableButton();
            label1.Text = "ПОЛЬЗОВАТЕЛИ";
            panelTitleBar.BackColor = Color.FromArgb(0, 150, 136);
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            buttonProd.Visible = true;

        }

        private void buttonProd_Click_1(object sender, EventArgs e)
        {
             if (activeForm != null)
                activeForm.Close();
            Reset();
            ActivateButton(sender);
            ReolaData();
        }

        private void buttonNewProd_Click_1(object sender, EventArgs e)
        {
            OpenChildForm(new Reg(), sender);
        }

        private void button2_Click_1(object sender, EventArgs e)
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string id = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    string name = dataGridView1.Rows[i].Cells[2].Value.ToString().ToLower();
                    string surname = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    string patronymic = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    string login = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    string phone_number = dataGridView1.Rows[i].Cells[5].Value.ToString();

                    string Sql = ("UPDATE `client` SET `surname` = '" + surname + "', `name` = '" + name + "', `patronymic` = '" + patronymic + "', `login` = '" + login + "', `phone_number` = '" + phone_number + "' WHERE `id` = '" + id + "'");
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

            ReolaData();
        }

        private void users_Load(object sender, EventArgs e)
        {
            //ПРИ ОТКРЫТИИ ФОРМЫ СРАЗУ ОТОБРАЖАЕТСЯ ТАБЛИЦА
           
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                }

                dataGridView1.AllowUserToAddRows = false;
                LoadData();
                dataGridView1.ClearSelection();
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //ПРИ НАЖАТИИ НА КНОПКУ "УДАЛИТЬ"
                if (e.ColumnIndex == 7)
                {
                    //СООБЩЕНИЕ ОБ УДАЛЕНИИ
                    if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                    {
                        string id_cl = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                        string Sql = ("DELETE FROM client WHERE id = " + "'" + id_cl + "'");
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
    }
}

