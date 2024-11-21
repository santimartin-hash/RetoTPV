using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Reto
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void EntrarBtn_Click(object sender, EventArgs e)
        {
            string userType = string.Empty;
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\RETODESIN.accdb;Persist Security Info=False;";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Tipo FROM usuarios WHERE NombreUsuario = ? AND Contraseña = ?";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NombreUsuario", textBoxUsuario.Text);
                        command.Parameters.AddWithValue("@Contraseña", textBoxContraseña.Text);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            userType = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrectos.");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al conectar con la base de datos: " + ex.Message);
                    return;
                }
            }

            Form2 form2 = new Form2(userType);
            form2.Show();
           this.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}