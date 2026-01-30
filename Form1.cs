using Npgsql;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace StudentManagementSystem
{
    public partial class Form1 : Form
    {
        private int selectedRowIndex = -1;
        private readonly string connectionString =
            "Host=localhost;Port=5432;Database=studentrecord;Username=postgres;Password=password";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadStudents();
            dataGridView1.CellClick += DataGridView1_CellClick;
        }

        #region DataGridView Button Columns
        private void AddUpdateButtonColumn()
        {
            if (!dataGridView1.Columns.Contains("btnUpdate"))
            {
                var btn = new DataGridViewButtonColumn
                {
                    HeaderText = "Action",
                    Name = "btnUpdate",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    DisplayIndex = dataGridView1.Columns.Count
                };
                dataGridView1.Columns.Add(btn);
            }
        }

        private void AddDeleteButtonColumn()
        {
            if (!dataGridView1.Columns.Contains("btnDelete"))
            {
                var btn = new DataGridViewButtonColumn
                {
                    HeaderText = "Action",
                    Name = "btnDelete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    DisplayIndex = dataGridView1.Columns.Count
                };
                dataGridView1.Columns.Add(btn);
            }
        }
        #endregion

        #region DataGridView Events
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // EDIT BUTTON
            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnUpdate")
            {
                selectedRowIndex = e.RowIndex;
                LoadSelectedRowToTextBoxes(e.RowIndex);
            }

            // DELETE BUTTON
            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                string email = dataGridView1.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                var confirm = MessageBox.Show(
                    "Are you sure you want to delete this record?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (confirm == DialogResult.Yes)
                {
                    DeleteStudent(email);
                    LoadStudents();
                }
            }
        }

        private void LoadSelectedRowToTextBoxes(int rowIndex)
        {
            textBox1.Text = dataGridView1.Rows[rowIndex].Cells["Name"].Value?.ToString();
            textBox2.Text = dataGridView1.Rows[rowIndex].Cells["Email"].Value?.ToString();
            textBox3.Text = dataGridView1.Rows[rowIndex].Cells["Age"].Value?.ToString();
            textBox4.Text = dataGridView1.Rows[rowIndex].Cells["CGPA"].Value?.ToString();
        }
        #endregion

        #region Validation
        private bool ValidateInputs(out int age, out decimal cgpa)
        {
            age = 0;
            cgpa = 0;

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Name is required.");
                textBox1.Focus();
                return false;
            }

            if (!Regex.IsMatch(textBox1.Text.Trim(), @"^[A-Za-z ]+$"))
            {
                MessageBox.Show("Name must contain only alphabets.");
                textBox1.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Email is required.");
                textBox2.Focus();
                return false;
            }

            if (!Regex.IsMatch(textBox2.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Invalid email format.");
                textBox2.Focus();
                return false;
            }

            if (!int.TryParse(textBox3.Text.Trim(), out age) || age < 18 || age > 60)
            {
                MessageBox.Show("Enter valid age (18-60).");
                textBox3.Focus();
                return false;
            }

            if (!decimal.TryParse(textBox4.Text.Trim(), out cgpa) || cgpa < 0 || cgpa > 4)
            {
                MessageBox.Show("Enter valid CGPA (0-4).");
                textBox4.Focus();
                return false;
            }

            if (decimal.Round(cgpa, 2) != cgpa)
            {
                MessageBox.Show("CGPA can have only 1 or 2 digits after decimal.");
                textBox4.Focus();
                return false;
            }

            return true;
        }
        #endregion

        #region CRUD Operations
        private void btninsert_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs(out int age, out decimal cgpa)) return;

            try
            {
                using (var con = new NpgsqlConnection(connectionString))
                {
                    con.Open();

                    using (var cmdCheck = new NpgsqlCommand(
                        "SELECT COUNT(*) FROM students WHERE LOWER(email)=LOWER(@Email)", con))
                    {
                        cmdCheck.Parameters.AddWithValue("@Email", textBox2.Text.Trim());
                        if (Convert.ToInt32(cmdCheck.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Email already exists.");
                            textBox2.Focus();
                            return;
                        }
                    }

                    using (var cmd = new NpgsqlCommand(
                        "INSERT INTO students(name,email,age,cgpa) VALUES(@Name,@Email,@Age,@Cgpa)", con))
                    {
                        cmd.Parameters.AddWithValue("@Name", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", textBox2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@Cgpa", cgpa);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Record inserted successfully ✅");
                ClearTextBoxes();
                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnupdate_Click(object sender, EventArgs e)
        {
            if (selectedRowIndex < 0)
            {
                MessageBox.Show("Select a record from the grid first.");
                return;
            }

            if (!ValidateInputs(out int age, out decimal cgpa)) return;

            try
            {
                using (var con = new NpgsqlConnection(connectionString))
                {
                    using (var cmd = new NpgsqlCommand(
                        "UPDATE students SET name=@Name, age=@Age, cgpa=@Cgpa WHERE LOWER(email)=LOWER(@Email)", con))
                    {
                        cmd.Parameters.AddWithValue("@Name", textBox1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Age", age);
                        cmd.Parameters.AddWithValue("@Cgpa", cgpa);
                        cmd.Parameters.AddWithValue("@Email", textBox2.Text.Trim());

                        con.Open();
                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Record updated successfully ✅");
                            LoadStudents();
                            ClearTextBoxes();
                        }
                        else
                        {
                            MessageBox.Show("No record found with this email ❌");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void DeleteStudent(string email)
        {
            try
            {
                using (var con = new NpgsqlConnection(connectionString))
                {
                    using (var cmd = new NpgsqlCommand(
                        "DELETE FROM students WHERE LOWER(email)=LOWER(@Email)", con))
                    {
                        cmd.Parameters.AddWithValue("@Email", email.Trim());
                        con.Open();
                        int rows = cmd.ExecuteNonQuery();

                        MessageBox.Show(rows > 0
                            ? "Record deleted successfully ✅"
                            : "Record not found ❌");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void LoadStudents()
        {
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();
                var da = new NpgsqlDataAdapter("SELECT * FROM students", con);
                var dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                AddUpdateButtonColumn();
                AddDeleteButtonColumn();
            }
        }
        #endregion

        private void ClearTextBoxes()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox1.Focus();
        }
    }
}
