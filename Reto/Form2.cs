using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Reto
{
    public partial class Form2 : Form
    {
        private int currentUserId;
        private string currentUserType;

        public Form2(int userId, string userType)
        {
            InitializeComponent();
            //gestion Usuario
            currentUserId = userId;
            currentUserType = userType;
            ConfigureButtons(userType);
            //gestion caja
            InitializeTPV();
            InitializeDataGridView();
            listViewItems.MouseClick += ListViewItems_MouseClick;
            listViewItems.Click += ListViewItems_Click;
            listViewItems.OwnerDraw = true;
            listViewItems.DrawItem += ListViewItems_DrawItem;
            //gestion reservas
            dateTimePicker.MinDate = DateTime.Today;
            dateTimePicker.ValueChanged += new EventHandler(DateTimePickerOrComboBox_Changed);
            comboBoxTipoReserva.SelectedIndexChanged += new EventHandler(DateTimePickerOrComboBox_Changed);
            comboBoxTipoReserva.DropDownStyle = ComboBoxStyle.DropDownList;
            mesa1.Click += new EventHandler(Mesa_Click);
            mesa2.Click += new EventHandler(Mesa_Click);
            mesa3.Click += new EventHandler(Mesa_Click);
            mesa4.Click += new EventHandler(Mesa_Click);
            mesa5.Click += new EventHandler(Mesa_Click);
            //gestion Almacen
            InitializeDataGridViewAlmacen();
            LoadProductsToDataGridViewAlmacen();
            panelAlmacen.Click += new EventHandler(panelAlmacen_Click);
            dataGridViewAlmacen.CellClick += new DataGridViewCellEventHandler(DataGridViewAlmacen_CellClick);
            LoadCategoriesToComboBoxCategoriasDisponibles();
            comboBoxCategoriasDisponibles.SelectedIndexChanged += ComboBoxCategoriasDisponibles_SelectedIndexChanged;
            comboBoxCategoriasDisponibles.SelectedIndex = 0;
            //gestion Usuarios
            InitializeDataGridViewUsuarios();
            panelUsuarios.Click += new EventHandler(panelUsuarios_Click);
            dataGridViewAlmacen.CellClick += new DataGridViewCellEventHandler(DataGridViewAlmacen_CellClick);
            dataGridViewUsuarios.CellClick += new DataGridViewCellEventHandler(DataGridViewUsuarios_CellClick);
            LoadUserTypesToComboBox();
        }




        private void LoadCategoriesToComboBoxCategoriasDisponibles()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string query = "SELECT DISTINCT Categoria FROM Productos"; // Seleccionar categorías distintas

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            comboBoxCategoriasDisponibles.Items.Clear(); // Limpiar el ComboBox antes de agregar nuevas categorías
                            comboBoxCategoriasDisponibles.Items.Add("TODAS"); // Añadir opción "TODAS" al principio

                            while (reader.Read())
                            {
                                string categoria = reader["Categoria"].ToString();
                                comboBoxCategoriasDisponibles.Items.Add(categoria);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar las categorías: " + ex.Message);
                }
            }
        }

        private void ComboBoxCategoriasDisponibles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCategory = comboBoxCategoriasDisponibles.SelectedItem.ToString();
            FilterListViewItemsByCategory(selectedCategory);
        }

        private void FilterListViewItemsByCategory(string category)
        {
            listViewItems.Items.Clear(); // Vaciar la lista

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string query = category == "TODAS" ? "SELECT NombreProducto, Precio, Descripcion, Categoria, [Imagen(URL)], Cantidad FROM Productos" :
                                                 "SELECT NombreProducto, Precio, Descripcion, Categoria, [Imagen(URL)], Cantidad FROM Productos WHERE Categoria = ?";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        if (category != "TODAS")
                        {
                            command.Parameters.AddWithValue("@Categoria", category);
                        }

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            ImageList imageList = new ImageList();
                            imageList.ImageSize = new Size(64, 64);

                            while (reader.Read())
                            {
                                string nombreProducto = reader["NombreProducto"].ToString();
                                string precio = reader["Precio"].ToString();
                                string descripcion = reader["Descripcion"].ToString();
                                string categoriaProducto = reader["Categoria"].ToString();
                                string imagenUrl = reader["Imagen(URL)"].ToString();
                                string cantidad = reader["Cantidad"].ToString();

                                // Crear un diccionario para almacenar todos los datos del producto
                                var productData = new Dictionary<string, string>
                                {
                                    { "NombreProducto", nombreProducto },
                                    { "Precio", precio },
                                    { "Descripcion", descripcion },
                                    { "Categoria", categoriaProducto },
                                    { "Imagen(URL)", imagenUrl },
                                    { "Cantidad", cantidad }
                                };

                                // Descargar imagen y agregarla al ImageList
                                System.Drawing.Image image = DownloadImage(imagenUrl);
                                imageList.Images.Add(nombreProducto, image);

                                // Crear un ListViewItem con el nombre del producto y asignar la imagen
                                ListViewItem item = new ListViewItem(nombreProducto);
                                item.ImageKey = nombreProducto;
                                item.Tag = productData; // Guardar todos los datos en Tag

                                // Deshabilitar y oscurecer el ítem si la cantidad es 0
                                if (int.Parse(cantidad) == 0)
                                {
                                    item.ForeColor = Color.Gray;
                                    item.BackColor = Color.LightGray;
                                }

                                listViewItems.Items.Add(item);
                            }

                            // Configurar ListView para usar ImageList
                            listViewItems.LargeImageList = imageList;
                            listViewItems.View = View.LargeIcon;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los productos: " + ex.Message);
                }
            }
        }

        private void panelAlmacen_Click(object sender, EventArgs e)
        {
            // Deselecciona la fila en el DataGridView
            dataGridViewAlmacen.ClearSelection();
            dataGridViewAlmacen.CurrentCell = null;
            imagenProducto.Image = null;
            textBoxNombreProducto.Text = "";
            textBoxPrecioProducto.Text = "";
            textBoxDescripcionProducto.Text = "";
            comboBoxCategoria.Text = "";
            cantidadProducto.Text = "";
            textBoxImagenProducto.Text = "";

            // Cambia el texto del botón de agregar a "Agregar"
            agregarProductoBtn.Text = "Agregar";


        }
        private void ListViewItems_MouseClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listViewItems.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                item.Selected = true;

                // Obtener el nombre del producto del ítem seleccionado
                string nombreProducto = item.Text;

                // Buscar y seleccionar la fila correspondiente en el DataGridView
                foreach (DataGridViewRow row in dataGridViewProducts.Rows)
                {
                    if (row.Cells["NombreProducto"].Value.ToString() == nombreProducto)
                    {
                        // Deseleccionar todas las celdas primero
                        dataGridViewProducts.ClearSelection();

                        // Seleccionar todas las celdas de la fila, excepto el selector de fila
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Selected = true;
                        }


                        // Asegurarse de que la fila sea visible
                        row.Selected = true;
                        dataGridViewProducts.FirstDisplayedScrollingRowIndex = row.Index;
                        break;
                    }
                }
            }
        }
        private void ConfigureButtons(string userType)
        {
            if (userType == "Admin")
            {
                MenuStockBtn.Visible = true;
                MenuUsuariosBtn.Visible = true;
            }
            else if (userType == "Usuario")
            {
                MenuStockBtn.Visible = false;
                MenuUsuariosBtn.Visible = false;
            }
        }

        private void InitializeTPV()
        {
            // Limpiar el ListView
            listViewItems.Items.Clear();
            // Inicializar ImageList y agregar imágenes
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(64, 64);

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=RETODESIN.accdb;Persist Security Info=False;";

            string query = "SELECT NombreProducto, Precio, Descripcion, Categoria, [Imagen(URL)], Cantidad FROM Productos"; // Seleccionar columnas específicas

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string nombreProducto = reader["NombreProducto"].ToString();
                                string precio = reader["Precio"].ToString();
                                string descripcion = reader["Descripcion"].ToString();
                                string categoria = reader["Categoria"].ToString();
                                string imagenUrl = reader["Imagen(URL)"].ToString();
                                string cantidad = reader["Cantidad"].ToString();

                                // Crear un diccionario para almacenar todos los datos del producto
                                var productData = new Dictionary<string, string>
                        {
                            { "NombreProducto", nombreProducto },
                            { "Precio", precio },
                            { "Descripcion", descripcion },
                            { "Categoria", categoria },
                            { "Imagen(URL)", imagenUrl },
                            { "Cantidad", cantidad }
                        };

                                // Descargar imagen y agregarla al ImageList
                                System.Drawing.Image image = DownloadImage(imagenUrl);
                                imageList.Images.Add(nombreProducto, image);

                                // Crear un ListViewItem con el nombre del producto y asignar la imagen
                                ListViewItem item = new ListViewItem(nombreProducto);
                                item.ImageKey = nombreProducto;
                                item.Tag = productData; // Guardar todos los datos en Tag

                                // Deshabilitar y oscurecer el ítem si la cantidad es 0
                                if (int.Parse(cantidad) == 0)
                                {
                                    item.ForeColor = Color.Gray;
                                    item.BackColor = Color.LightGray;
                                }

                                listViewItems.Items.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los productos: " + ex.Message);
                }
            }
            // Configurar ListView para usar ImageList
            listViewItems.LargeImageList = imageList;
            listViewItems.View = View.LargeIcon;
        }

        private System.Drawing.Image DownloadImage(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                // Agregar un encabezado de agente de usuario
                webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3");

                byte[] data = webClient.DownloadData(url);
                using (var ms = new System.IO.MemoryStream(data))
                {
                    return System.Drawing.Image.FromStream(ms);
                }
            }
        }

        private void InitializeDataGridView()
        {
            // Configurar DataGridView
            dataGridViewProducts.Columns.Add(new DataGridViewImageColumn { HeaderText = "Imagen", Name = "Imagen" });
            dataGridViewProducts.Columns.Add("NombreProducto", "Nombre");
            dataGridViewProducts.Columns.Add("Descripcion", "Descripción");
            dataGridViewProducts.Columns.Add("Categoria", "Categoría");
            dataGridViewProducts.Columns.Add("Cantidad", "Cantidad");
            dataGridViewProducts.Columns.Add("Precio", "Precio");

            // Establecer propiedades de estilo
            dataGridViewProducts.BorderStyle = BorderStyle.None;
            dataGridViewProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewProducts.DefaultCellStyle.SelectionBackColor = Color.SeaGreen;
            dataGridViewProducts.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridViewProducts.BackgroundColor = Color.White;

            dataGridViewProducts.EnableHeadersVisualStyles = false;
            dataGridViewProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.DarkGreen;
            dataGridViewProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;


            // Ajustar el tamaño de las columnas y filas
            dataGridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewProducts.ColumnHeadersHeight = 30;
            dataGridViewProducts.RowTemplate.Height = 70; // Ajustar la altura de las filas

            // No mostrar la fila de "nueva" por defecto
            dataGridViewProducts.AllowUserToAddRows = false;

            // Suscribirse al evento CellClick
            dataGridViewProducts.CellClick += DataGridViewProducts_CellClick;

        }


        private void DataGridViewProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Deseleccionar todas las celdas primero
                dataGridViewProducts.ClearSelection();

                // Seleccionar todas las celdas de la fila, excepto el selector de fila
                foreach (DataGridViewCell cell in dataGridViewProducts.Rows[e.RowIndex].Cells)
                {
                    cell.Selected = true;
                }


                // Obtener el nombre del producto de la fila seleccionada
                string nombreProducto = dataGridViewProducts.Rows[e.RowIndex].Cells["NombreProducto"].Value.ToString();

                // Deseleccionar todos los ítems en el ListView
                foreach (ListViewItem item in listViewItems.Items)
                {
                    item.Selected = false;
                }

                // Buscar y seleccionar el ítem correspondiente en el ListView
                foreach (ListViewItem item in listViewItems.Items)
                {
                    if (item.Text == nombreProducto)
                    {
                        item.Selected = true;
                        item.EnsureVisible(); // Asegurarse de que el ítem sea visible
                        break;
                    }
                }
                productPictureBox.Image = listViewItems.LargeImageList.Images[listViewItems.SelectedItems[0].ImageKey];
                CantidadProductAñadir.Value = 1;
                cantStockDisponible.Text = "Stock disponible: " + ((Dictionary<string, string>)listViewItems.SelectedItems[0].Tag)["Cantidad"];
            }
        }

        private void ListViewItems_Click(object sender, EventArgs e)
        {
            if (listViewItems.SelectedItems.Count > 0)
            {
                var selectedItem = listViewItems.SelectedItems[0];
                var productData = (Dictionary<string, string>)selectedItem.Tag;
                int cantidad = int.Parse(productData["Cantidad"]);

                if (cantidad == 0)
                {


                    // Ocultar los controles
                    productPictureBox.Visible = false;
                    CantidadProductAñadir.Visible = false;
                    AñadirProductoBtn.Visible = false;
                    cantStockDisponible.Visible = false;
                    QuitarProductoBtn.Visible = false;
                    labelcantidad.Visible = false;
                }
                else
                {
                    // Restaurar el fondo original
                    listViewItems.BackColor = SystemColors.Window;

                    // Mostrar los controles
                    productPictureBox.Visible = true;
                    CantidadProductAñadir.Visible = true;
                    AñadirProductoBtn.Visible = true;
                    cantStockDisponible.Visible = true;
                    QuitarProductoBtn.Visible = true;
                    labelcantidad.Visible = true;

                    productPictureBox.Image = listViewItems.LargeImageList.Images[selectedItem.ImageKey];
                    CantidadProductAñadir.Value = 1;
                    cantStockDisponible.Text = "Stock disponible: " + productData["Cantidad"];
                }
            }
        }
        private void ListViewItems_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            // Dibujar el fondo del elemento
            e.DrawBackground();

            // Reducir el tamaño de la imagen
            int imageWidth = 54;
            int imageHeight = 54;

            // Calcular la posición de la imagen en el centro superior
            int imageX = e.Bounds.X + (e.Bounds.Width - imageWidth) / 2;
            int imageY = e.Bounds.Y + 5; // Un pequeño margen superior

            // Check if the image is null
            var image = listViewItems.LargeImageList?.Images[e.Item.ImageKey];
            if (image != null)
            {
                e.Graphics.DrawImage(image, new System.Drawing.Rectangle(imageX, imageY, imageWidth, imageHeight));
            }

            // Calcular la posición del texto más arriba
            int textX = e.Bounds.X + (e.Bounds.Width - TextRenderer.MeasureText(e.Item.Text, e.Item.Font).Width) / 2;
            int textY = e.Bounds.Y + imageHeight + 8; // Un pequeño margen entre la imagen y el texto
            TextRenderer.DrawText(e.Graphics, e.Item.Text, e.Item.Font, new Point(textX, textY), e.Item.ForeColor);

            // Dibujar el rectángulo de selección más grande
            if (e.Item.Selected)
            {
                int selectionPadding = 1; // Ajustar el tamaño del rectángulo de selección
                System.Drawing.Rectangle selectionRectangle = new System.Drawing.Rectangle(
                    e.Bounds.X,
                    e.Bounds.Y,
                    e.Bounds.Width - 2 * selectionPadding,
                    e.Bounds.Height - 2 * selectionPadding
                );

                e.Graphics.DrawRectangle(Pens.Black, selectionRectangle);
            }
        }



        private void AñadirProductoBtn_Click(object sender, EventArgs e)
        {
            int cantidadAañadir = (int)CantidadProductAñadir.Value;
            if (listViewItems.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listViewItems.SelectedItems[0];
                var productData = (Dictionary<string, string>)selectedItem.Tag;

                // Verificar si el producto ya está en el DataGridView
                foreach (DataGridViewRow row in dataGridViewProducts.Rows)
                {
                    var cellValue = row.Cells["NombreProducto"].Value;
                    if (cellValue != null && cellValue.ToString() == productData["NombreProducto"])
                    {
                        int currentQuantity = int.Parse(row.Cells["Cantidad"].Value.ToString());
                        int availableQuantity = int.Parse(productData["Cantidad"]);

                        if (currentQuantity + cantidadAañadir > availableQuantity)
                        {
                            MessageBox.Show("No hay cantidad disponible.");
                            CantidadProductAñadir.Value = 1;
                            ActualizarTotal();
                            return;
                        }
                        else
                        {
                            row.Cells["Cantidad"].Value = currentQuantity + cantidadAañadir;
                            CantidadProductAñadir.Value = 1;
                            ActualizarTotal();
                            return;
                        }

                    }
                    CantidadProductAñadir.Value = 1;
                    ActualizarTotal();
                }


                // Crear una fila con todos los campos del producto
                DataGridViewRow newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridViewProducts);
                newRow.Cells[0].Value = listViewItems.LargeImageList.Images[selectedItem.ImageKey];
                newRow.Cells[1].Value = productData["NombreProducto"];

                newRow.Cells[2].Value = productData["Descripcion"];
                newRow.Cells[3].Value = productData["Categoria"];
                if (cantidadAañadir > int.Parse(productData["Cantidad"]))
                {
                    MessageBox.Show("No hay cantidad disponible.");
                    CantidadProductAñadir.Value = 1;
                    return;
                }
                newRow.Cells[4].Value = cantidadAañadir;
                newRow.Cells[5].Value = productData["Precio"] + "€";
                // Ajustar la altura de la nueva fila
                newRow.Height = 60;
                dataGridViewProducts.Rows.Add(newRow); // Añadir al DataGridView
                                                       // Seleccionar la nueva fila
                SelectRow(newRow.Index);
                CantidadProductAñadir.Value = 1;
                ActualizarTotal();
            }

        }
        private void SelectRow(int rowIndex)
        {
            if (rowIndex >= 0)
            {
                // Deseleccionar todas las celdas primero
                dataGridViewProducts.ClearSelection();

                // Seleccionar todas las celdas de la fila, excepto el selector de fila
                foreach (DataGridViewCell cell in dataGridViewProducts.Rows[rowIndex].Cells)
                {
                    cell.Selected = true;
                }

                // Obtener el nombre del producto de la fila seleccionada
                string nombreProducto = dataGridViewProducts.Rows[rowIndex].Cells["NombreProducto"].Value.ToString();

                // Deseleccionar todos los ítems en el ListView
                foreach (ListViewItem item in listViewItems.Items)
                {
                    item.Selected = false;
                }

                // Buscar y seleccionar el ítem correspondiente en el ListView
                foreach (ListViewItem item in listViewItems.Items)
                {
                    if (item.Text == nombreProducto)
                    {
                        item.Selected = true;
                        item.EnsureVisible(); // Asegurarse de que el ítem sea visible
                        break;
                    }
                }
                productPictureBox.Image = listViewItems.LargeImageList.Images[listViewItems.SelectedItems[0].ImageKey];
                CantidadProductAñadir.Value = 1;
                cantStockDisponible.Text = "Stock disponible: " + ((Dictionary<string, string>)listViewItems.SelectedItems[0].Tag)["Cantidad"];
            }
        }
        private void QuitarProductoBtn_Click(object sender, EventArgs e)
        {
            int cantidadAquitar = (int)CantidadProductAñadir.Value;
            if (listViewItems.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listViewItems.SelectedItems[0];
                var productData = (Dictionary<string, string>)selectedItem.Tag;

                // Verificar si el producto ya está en el DataGridView
                foreach (DataGridViewRow row in dataGridViewProducts.Rows)
                {
                    var cellValue = row.Cells["NombreProducto"].Value;
                    if (cellValue != null && cellValue.ToString() == productData["NombreProducto"])
                    {
                        int currentQuantity = int.Parse(row.Cells["Cantidad"].Value.ToString());
                        if (currentQuantity - cantidadAquitar <= 0)
                        {
                            dataGridViewProducts.Rows.Remove(row);
                            ActualizarTotal();
                        }
                        else
                        {
                            row.Cells["Cantidad"].Value = currentQuantity - cantidadAquitar;
                            ActualizarTotal();
                        }
                        CantidadProductAñadir.Value = 1;
                        ActualizarTotal();
                        return;

                    }

                }
                CantidadProductAñadir.Value = 1;
                ActualizarTotal();
            }
            CantidadProductAñadir.Value = 1;
            ActualizarTotal();
        }

        private void salirBtn_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1();
            form1.Visible = true;
        }

        private void ActualizarTotal()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dataGridViewProducts.Rows)
            {
                if (row.Cells["Precio"].Value != null && row.Cells["Cantidad"].Value != null)
                {
                    string precioStr = row.Cells["Precio"].Value.ToString().Replace("€", "").Trim();
                    if (decimal.TryParse(precioStr, out decimal precio) && int.TryParse(row.Cells["Cantidad"].Value.ToString(), out int cantidad))
                    {
                        total += precio * cantidad;
                    }
                }
            }

            TotalTextBox.Text = total.ToString("C");
        }
        private void imprimirBtn_Click(object sender, EventArgs e)
        {
            // Comprobar si hay algún ítem en el DataGridView
            if (dataGridViewProducts.Rows.Count == 0)
            {
                MessageBox.Show("No hay productos para imprimir.");
                return; // Salir del método si no hay ítems
            }

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (DataGridViewRow row in dataGridViewProducts.Rows)
                        {
                            if (row.Cells["NombreProducto"].Value != null && row.Cells["Cantidad"].Value != null)
                            {
                                string nombreProducto = row.Cells["NombreProducto"].Value.ToString();
                                int cantidad = int.Parse(row.Cells["Cantidad"].Value.ToString());

                                // Construir la consulta con los valores de los parámetros
                                string updateQuery = $"UPDATE Productos SET Cantidad = Cantidad - {cantidad} WHERE NombreProducto = '{nombreProducto}'";

                                // Actualizar la cantidad en la base de datos
                                using (OleDbCommand updateCommand = new OleDbCommand(updateQuery, connection, transaction))
                                {
                                    updateCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error al actualizar la base de datos: " + ex.Message);
                        return;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("******************** FACTURA *********************");
            sb.AppendLine("          Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sb.AppendLine("<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>");
            sb.AppendLine(string.Format("{0,-20} {1,-10} {2,-10} {3,-10}", "Producto", "Cantidad", "Precio", "Total"));
            sb.AppendLine("--------------------------------------------------"); // Línea separadora

            foreach (DataGridViewRow row in dataGridViewProducts.Rows)
            {
                if (row.Cells["NombreProducto"].Value != null && row.Cells["Cantidad"].Value != null && row.Cells["Precio"].Value != null)
                {
                    string nombreProducto = row.Cells["NombreProducto"].Value.ToString();
                    int cantidad = int.Parse(row.Cells["Cantidad"].Value.ToString());
                    string precioStr = row.Cells["Precio"].Value.ToString().Replace("€", "").Trim();
                    decimal precio = decimal.Parse(precioStr);
                    decimal totalProducto = cantidad * precio;

                    sb.AppendLine(string.Format("{0,-20} {1,-10} {2,-10:C} {3,-10:C}", nombreProducto, cantidad, precio, totalProducto));
                }
            }


            sb.AppendLine("<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>");
            sb.AppendLine("Total: " + TotalTextBox.Text);
            sb.AppendLine("**************************************************");

            // Guardar el archivo de texto
            string filePath = "Factura.txt";
            System.IO.File.WriteAllText(filePath, sb.ToString());

            MessageBox.Show("Factura generada correctamente en " + filePath);

            // Abrir el archivo de texto
            System.Diagnostics.Process.Start(filePath);

            // Limpiar el DataGridView
            dataGridViewProducts.Rows.Clear();

            // Eliminar todos los ítems del ListView
            listViewItems.Items.Clear();

            productPictureBox.Visible = false;
            CantidadProductAñadir.Visible = false;
            AñadirProductoBtn.Visible = false;
            cantStockDisponible.Visible = false;
            QuitarProductoBtn.Visible = false;
            labelcantidad.Visible = false;
            TotalTextBox.Text = "0,00 €";
            comboBoxCategoriasDisponibles.SelectedIndex = 0;

            InitializeTPV();
        }


        private void MenuCajaBtn_Click(object sender, EventArgs e)
        {
            InitializeTPV();
            panelCaja.Visible = true;
            panelReservas.Visible = false;
            panelAlmacen.Visible = false;
            panelUsuarios.Visible = false;
            comboBoxCategoriasDisponibles.SelectedIndex = 0;
            LoadCategoriesToComboBoxCategoriasDisponibles();

        }

        private void MenuReservaBtn_Click(object sender, EventArgs e)
        {
            panelReservas.Visible = true;
            panelCaja.Visible = false;
            panelAlmacen.Visible = false;
            panelUsuarios.Visible = false;
            comboBoxCategoriasDisponibles.SelectedIndex = 0;
        }

        private void MenuStockBtn_Click(object sender, EventArgs e)
        {
            panelAlmacen.Visible = true;
            panelReservas.Visible = false;
            panelCaja.Visible = false;
            panelUsuarios.Visible = false;
            comboBoxCategoriasDisponibles.SelectedIndex = 0;
            LoadProductsToDataGridViewAlmacen();


        }
        private void MenuUsuariosBtn_Click(object sender, EventArgs e)
        {
            panelUsuarios.Visible = true;
            panelAlmacen.Visible = false;
            panelReservas.Visible = false;
            panelCaja.Visible = false;
            comboBoxCategoriasDisponibles.SelectedIndex = 0;
            LoadUsersToDataGridView();

        }
        private void InitializeDataGridViewAlmacen()
        {
            dataGridViewAlmacen.Columns.Clear();
            dataGridViewAlmacen.AutoGenerateColumns = false;

            dataGridViewAlmacen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 70
            });

            // Añadir columna de imagen al DataGridView
            dataGridViewAlmacen.Columns.Add(new DataGridViewImageColumn
            {
                Name = "Imagen",
                HeaderText = "Imagen",
                DataPropertyName = "Imagen",
                Width = 100,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });

            // Añadir columnas al DataGridView
            dataGridViewAlmacen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreProducto",
                HeaderText = "Nombre",
                DataPropertyName = "NombreProducto",
                Width = 150
            });
            dataGridViewAlmacen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Precio",
                HeaderText = "Precio",
                DataPropertyName = "Precio",
                Width = 70
            });
            dataGridViewAlmacen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Descripcion",
                HeaderText = "Descripción",
                DataPropertyName = "Descripcion",
                Width = 200
            });
            dataGridViewAlmacen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Categoria",
                HeaderText = "Categoría",
                DataPropertyName = "Categoria",
                Width = 100
            });
            dataGridViewAlmacen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Cantidad",
                HeaderText = "Cantidad",
                DataPropertyName = "Cantidad",
                Width = 70
            });


            // Añadir columna invisible para la URL de la imagen
            dataGridViewAlmacen.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ImagenURL",
                HeaderText = "Imagen URL",
                DataPropertyName = "ImagenURL",
                Visible = false
            });




            // Estilo del DataGridView
            dataGridViewAlmacen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewAlmacen.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewAlmacen.MultiSelect = false;
            dataGridViewAlmacen.AllowUserToAddRows = false;
            dataGridViewAlmacen.AllowUserToDeleteRows = false;
            dataGridViewAlmacen.ReadOnly = true;

            // Cambiar el color de la fila seleccionada y las filas normales
            dataGridViewAlmacen.DefaultCellStyle.SelectionBackColor = Color.Green;
            dataGridViewAlmacen.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridViewAlmacen.DefaultCellStyle.BackColor = Color.LightGreen;
            dataGridViewAlmacen.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;

            // Cargar categorías en el ComboBox
            LoadCategoriesToComboBox();
        }
        private void LoadCategoriesToComboBox()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string query = "SELECT DISTINCT Categoria FROM Productos"; // Seleccionar categorías distintas

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            comboBoxCategoria.Items.Clear(); // Limpiar el ComboBox antes de agregar nuevas categorías

                            while (reader.Read())
                            {
                                string categoria = reader["Categoria"].ToString();
                                comboBoxCategoria.Items.Add(categoria);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar las categorías: " + ex.Message);
                }
            }

            // Añadir opción de "Nueva categoría" al final
            comboBoxCategoria.Items.Add("Nueva categoría");
        }
        private void DataGridViewAlmacen_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que la celda clicada no sea el encabezado
            if (e.RowIndex >= 0)
            {
                // Obtén la fila seleccionada
                DataGridViewRow row = dataGridViewAlmacen.Rows[e.RowIndex];

                // Carga la información del producto en los controles del panelAlmacen
                // Supongamos que tienes TextBoxes para mostrar la información del producto
                textBoxIdProducto.Text = row.Cells["Id"].Value.ToString();
                imagenProducto.Image = (System.Drawing.Image)row.Cells["Imagen"].Value;
                textBoxNombreProducto.Text = row.Cells["NombreProducto"].Value.ToString();
                textBoxPrecioProducto.Text = row.Cells["Precio"].Value.ToString();
                textBoxDescripcionProducto.Text = row.Cells["Descripcion"].Value.ToString();
                comboBoxCategoria.Text = row.Cells["Categoria"].Value.ToString();
                cantidadProducto.Text = row.Cells["Cantidad"].Value.ToString();
                textBoxImagenProducto.Text = row.Cells["ImagenURL"].Value.ToString();


                // Cambia el texto del botón de agregar a "Modificar"
                agregarProductoBtn.Text = "Modificar";
            }
        }

        private void LoadProductsToDataGridViewAlmacen()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string query = "SELECT Id, NombreProducto, Precio, Descripcion, Categoria, [Imagen(URL)], Cantidad FROM Productos"; // Seleccionar columnas específicas

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Añadir columna de imagen al DataTable
                            dataTable.Columns.Add("Imagen", typeof(System.Drawing.Image));
                            dataTable.Columns.Add("ImagenURL", typeof(string));

                            foreach (DataRow row in dataTable.Rows)
                            {
                                string imagenUrl = row["Imagen(URL)"].ToString();
                                row["Imagen"] = DownloadImage(imagenUrl);
                                row["ImagenURL"] = imagenUrl; // Asignar la URL de la imagen a la nueva columna


                            }

                            dataGridViewAlmacen.DataSource = dataTable;

                            // Deseleccionar cualquier celda
                            dataGridViewAlmacen.ClearSelection();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los productos: " + ex.Message);
                }
            }
        }

        private void TextBoxImagenProducto_TextChanged(object sender, EventArgs e)
        {
            string url = textBoxImagenProducto.Text;
            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    imagenProducto.Image = DownloadImage(url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar la imagen: " + ex.Message);
                }
            }
            else
            {
                imagenProducto.Image = null; // Limpiar la imagen si la URL está vacía
            }
        }

        private void agregarProductoBtn_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string defaultImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/d/da/Imagen_no_disponible.svg/1200px-Imagen_no_disponible.svg.png"; // Ruta de la imagen por defecto

            // Validar que el nombre no sea nulo o vacío
            if (string.IsNullOrWhiteSpace(textBoxNombreProducto.Text))
            {
                MessageBox.Show("El nombre del producto no puede estar vacío.");
                return;
            }
            // Validar que el precio no sea nulo o vacío
            if (string.IsNullOrWhiteSpace(textBoxPrecioProducto.Text))
            {
                MessageBox.Show("El precio del producto no puede estar vacío.");
                return;
            }
            // Validar que la categoría no sea nula o vacía
            if (string.IsNullOrWhiteSpace(comboBoxCategoria.Text))
            {
                MessageBox.Show("La categoría del producto no puede estar vacía.");
                return;
            }

            // Validar que el precio sea un número válido y que use coma como separador decimal
            if (!double.TryParse(textBoxPrecioProducto.Text.Trim().Replace('.', ','), out double precio))
            {
                MessageBox.Show("El precio debe ser un número válido.");
                return;
            }

            // Asignar imagen por defecto si la URL está vacía
            string imagenUrl = string.IsNullOrWhiteSpace(textBoxImagenProducto.Text) ? defaultImageUrl : textBoxImagenProducto.Text;

            // Validar y manejar la cantidad
            int cantidad = 0; // Valor por defecto
            if (!int.TryParse(cantidadProducto.Text, out cantidad))
            {
                // Si no se puede parsear, se asigna 0
                cantidad = 0;
            }

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (agregarProductoBtn.Text == "Modificar")
                    {
                        // Código para modificar el producto existente
                        string query = "UPDATE Productos SET NombreProducto = ?, Precio = ?, Descripcion = ?, Categoria = ?, [Imagen(URL)] = ?, Cantidad = ? WHERE Id = ?";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@NombreProducto", textBoxNombreProducto.Text);
                            command.Parameters.AddWithValue("@Precio", precio);
                            command.Parameters.AddWithValue("@Descripcion", (object)textBoxDescripcionProducto.Text ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Categoria", comboBoxCategoria.Text);
                            command.Parameters.AddWithValue("@Imagen(URL)", imagenUrl);
                            command.Parameters.AddWithValue("@Cantidad", cantidad); // Usamos la cantidad calculada
                            command.Parameters.AddWithValue("@Id", int.Parse(textBoxIdProducto.Text)); // Asumiendo que tienes un campo Id para identificar el producto

                            command.ExecuteNonQuery();
                        }

                        agregarProductoBtn.Text = "Agregar";
                    }
                    else if (agregarProductoBtn.Text == "Agregar")
                    {
                        // Código para insertar un nuevo producto
                        string query = "INSERT INTO Productos (NombreProducto, Precio, Descripcion, Categoria, [Imagen(URL)], Cantidad) VALUES (?, ?, ?, ?, ?, ?)";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@NombreProducto", textBoxNombreProducto.Text);
                            command.Parameters.AddWithValue("@Precio", precio);
                            command.Parameters.AddWithValue("@Descripcion", (object)textBoxDescripcionProducto.Text ?? DBNull.Value);
                            command.Parameters.AddWithValue("@Categoria", comboBoxCategoria.Text);
                            command.Parameters.AddWithValue("@Imagen(URL)", imagenUrl);
                            command.Parameters.AddWithValue("@Cantidad", cantidad); // Usamos la cantidad calculada

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el producto: " + ex.Message);
                }

                // Actualizar la vista del producto en el grid
                actualizarDataGridProductosAlmacen();

                // Limpiar los campos después de la operación
                textBoxIdProducto.Text = "";
                imagenProducto.Image = null;
                textBoxNombreProducto.Text = "";
                textBoxPrecioProducto.Text = "";
                textBoxDescripcionProducto.Text = "";
                comboBoxCategoria.Text = "";
                cantidadProducto.Text = ""; // Restablecer cantidad
                textBoxImagenProducto.Text = "";
            }
        }

        private async void actualizarDataGridProductosAlmacen()
        {

            await Task.Delay(100);

            // Inicializar y cargar el DataGridView
            InitializeDataGridViewAlmacen();
            LoadProductsToDataGridViewAlmacen();
        }

        private void eliminarProductoBtn_Click(object sender, EventArgs e)
        {
            // Suponiendo que tienes un TextBox llamado textBoxIdProducto
            string productoId = textBoxIdProducto.Text;

            if (string.IsNullOrWhiteSpace(productoId))
            {
                MessageBox.Show("Por favor, ingrese un ID de producto válido.");
                return;
            }

            // Cadena de conexión a la base de datos
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Consulta SQL para eliminar el producto
                    string query = "DELETE FROM Productos WHERE Id = @ProductoId";

                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Agregar el parámetro a la consulta
                        command.Parameters.AddWithValue("@ProductoId", productoId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Producto eliminado correctamente.");
                            // Actualizar el DataGridView
                            LoadProductsToDataGridViewAlmacen();
                        }
                        else
                        {
                            MessageBox.Show("El producto no existe.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el producto: " + ex.Message);
                }

            }

            // Limpiar los campos después de la operación
            textBoxIdProducto.Text = "";
            imagenProducto.Image = null;
            textBoxNombreProducto.Text = "";
            textBoxPrecioProducto.Text = "";
            textBoxDescripcionProducto.Text = "";
            comboBoxCategoria.Text = "";
            cantidadProducto.Text = ""; // Restablecer cantidad
            textBoxImagenProducto.Text = "";

            agregarProductoBtn.Text = "Agregar";
        }
        private void LoadUserTypesToComboBox()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string query = "SELECT DISTINCT Tipo FROM Usuarios"; // Seleccionar tipos distintos

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            comboBoxTipoUsuario.Items.Clear(); // Limpiar el ComboBox antes de agregar nuevos tipos

                            while (reader.Read())
                            {
                                string tipo = reader["Tipo"].ToString();
                                comboBoxTipoUsuario.Items.Add(tipo);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los tipos de usuario: " + ex.Message);
                }
            }

            // Añadir opción de "Nuevo tipo de usuario" al final
            comboBoxTipoUsuario.Items.Add("Nuevo tipo de usuario");
        }


        public void InitializeDataGridViewUsuarios()
        {
            // Limpiar el DataGridView
            dataGridViewUsuarios.Columns.Clear();
            dataGridViewUsuarios.AutoGenerateColumns = false;
            // Añadir columnas al DataGridView
            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = "Id",
                Width = 70
            });
            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NombreUsuario",
                HeaderText = "Nombre",
                DataPropertyName = "NombreUsuario",
                Width = 150
            });
            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Contraseña",
                HeaderText = "Contraseña",
                DataPropertyName = "Contraseña",
                Width = 150
            });
            dataGridViewUsuarios.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Tipo",
                HeaderText = "Tipo",
                DataPropertyName = "Tipo",
                Width = 100
            });
            // Estilo del DataGridView
            dataGridViewUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewUsuarios.MultiSelect = false;
            dataGridViewUsuarios.AllowUserToAddRows = false;
            dataGridViewUsuarios.AllowUserToDeleteRows = false;
            dataGridViewUsuarios.ReadOnly = true;
            // Cambiar el color de la fila seleccionada y las filas normales
            dataGridViewUsuarios.DefaultCellStyle.SelectionBackColor = Color.Green;
            dataGridViewUsuarios.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridViewUsuarios.DefaultCellStyle.BackColor = Color.LightGreen;
            dataGridViewUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGreen;
            // Cargar los usuarios en el DataGridView
            LoadUsersToDataGridView();

        }

        private void LoadUsersToDataGridView()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string query = "SELECT Id, NombreUsuario, Contraseña, Tipo FROM Usuarios"; // Seleccionar columnas específicas
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dataGridViewUsuarios.DataSource = dataTable;
                            // Deseleccionar cualquier celda
                            dataGridViewUsuarios.ClearSelection();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los usuarios: " + ex.Message);
                }
            }
        }
        private void panelUsuarios_Click(object sender, EventArgs e)
        {
            // Deselecciona la fila en el DataGridView
            dataGridViewUsuarios.ClearSelection();
            dataGridViewUsuarios.CurrentCell = null;
            textBoxContraseñaU.Text = "";
            textBoxNombreU.Text = "";
            textBoxIdU.Text = "";
            comboBoxTipoUsuario.SelectedIndex = -1;
            // Cambia el texto del botón de agregar a "Agregar"
            AgregarUsuarioBtn.Text = "Agregar";
        }
        private void DataGridViewUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica que la celda clicada no sea el encabezado
            if (e.RowIndex >= 0)
            {
                // Obtén la fila seleccionada
                DataGridViewRow row = dataGridViewUsuarios.Rows[e.RowIndex];

                textBoxContraseñaU.Text = row.Cells["Contraseña"].Value.ToString();
                textBoxNombreU.Text = row.Cells["NombreUsuario"].Value.ToString();
                comboBoxTipoUsuario.Text = row.Cells["Tipo"].Value.ToString();

                textBoxIdU.Text = row.Cells["Id"].Value.ToString();



                // Cambia el texto del botón de agregar a "Modificar"
                AgregarUsuarioBtn.Text = "Modificar";
            }
        }

        private void AgregarUsuarioBtn_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            // Validar que el nombre no sea nulo o vacío
            if (string.IsNullOrWhiteSpace(textBoxNombreU.Text))
            {
                MessageBox.Show("El nombre del usuario no puede estar vacío.");
                return;
            }
            // Validar que la contraseña no sea nula o vacía
            if (string.IsNullOrWhiteSpace(textBoxContraseñaU.Text))
            {
                MessageBox.Show("La contraseña del usuario no puede estar vacía.");
                return;
            }
            // Validar que el tipo no sea nulo o vacío
            if (string.IsNullOrWhiteSpace(comboBoxTipoUsuario.Text))
            {
                MessageBox.Show("El tipo del usuario no puede estar vacío.");
                return;
            }


            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (AgregarUsuarioBtn.Text == "Modificar")
                    {
                        // Código para modificar el usuario existente
                        string query = "UPDATE Usuarios SET NombreUsuario = ?, Contraseña = ?, Tipo = ? WHERE Id = ?";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@NombreUsuario", textBoxNombreU.Text);
                            command.Parameters.AddWithValue("@Contraseña", textBoxContraseñaU.Text);
                            command.Parameters.AddWithValue("@Tipo", comboBoxTipoUsuario.Text);
                            command.Parameters.AddWithValue("@Id", int.Parse(textBoxIdU.Text)); // Usar el Id del usuario
                            command.ExecuteNonQuery();
                        }
                        AgregarUsuarioBtn.Text = "Agregar";
                        actualizarDataGridUsuarios();
                    }
                    else if (AgregarUsuarioBtn.Text == "Agregar")
                    {
                        // Código para insertar un nuevo usuario
                        string query = "INSERT INTO Usuarios (NombreUsuario, Contraseña, Tipo) VALUES (?, ?, ?)";
                        using (OleDbCommand command = new OleDbCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@NombreUsuario", textBoxNombreU.Text);
                            command.Parameters.AddWithValue("@Contraseña", textBoxContraseñaU.Text);
                            command.Parameters.AddWithValue("@Tipo", comboBoxTipoUsuario.Text);
                            command.ExecuteNonQuery();
                        }
                        actualizarDataGridUsuarios();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el usuario: " + ex.Message);
                }
                actualizarDataGridUsuarios();

                // Limpiar los campos después de la operación
                textBoxIdU.Text = "";
                textBoxContraseñaU.Text = "";
                textBoxNombreU.Text = "";
                comboBoxTipoUsuario.Text = "";
            }
        }

        private async void actualizarDataGridUsuarios()
        {

            await Task.Delay(100);

            // Inicializar y cargar el DataGridView
            InitializeDataGridViewUsuarios();

            LoadUsersToDataGridView();
            LoadUserTypesToComboBox();

        }

        private void EliminarUsuarioBtn_Click(object sender, EventArgs e)
        {
            // Suponiendo que tienes un TextBox llamado textBoxNombreU y otro llamado textBoxContraseñaU
            string usuarioNombre = textBoxNombreU.Text;
            string usuarioContraseña = textBoxContraseñaU.Text;
            string id = textBoxIdU.Text;

            if (string.IsNullOrWhiteSpace(usuarioNombre))
            {
                MessageBox.Show("Por favor, ingrese un nombre de usuario válido.");
                return;
            }

            // Comprobar si el usuario que se intenta eliminar es el mismo que el usuario actual
            if (int.Parse(id) == currentUserId)
            {
                MessageBox.Show("No se puede eliminar el usuario actual.");
                return;
            }

            // Cadena de conexión a la base de datos
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Consulta SQL para eliminar el usuario
                    string query = "DELETE FROM Usuarios WHERE Id = @Id";
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Agregar el parámetro a la consulta
                        command.Parameters.AddWithValue("@Id", id);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Usuario eliminado correctamente.");
                            // Actualizar el DataGridView
                            LoadUsersToDataGridView();
                        }
                        else
                        {
                            MessageBox.Show("El usuario no existe.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el usuario: " + ex.Message);
                }
            }
            LoadUserTypesToComboBox();
            // Limpiar los campos después de la operación
            textBoxIdU.Text = "";
            textBoxContraseñaU.Text = "";
            textBoxNombreU.Text = "";
            comboBoxTipoUsuario.Text = "";

            AgregarUsuarioBtn.Text = "Agregar";
        }

        private void DateTimePickerOrComboBox_Changed(object sender, EventArgs e)
        {
            // Verificar si ambos controles tienen una selección
            if (dateTimePicker.Value.Date >= DateTime.Today && comboBoxTipoReserva.SelectedIndex != -1)
            {
                pictureBoxReserva.Visible = true;
                mesa1.Visible = true;
                mesa2.Visible = true;
                mesa3.Visible = true;
                mesa4.Visible = true;
                mesa5.Visible = true;
                ComprobarDisponibilidadMesas(dateTimePicker.Value.Date, comboBoxTipoReserva.SelectedItem.ToString());
            }
            else
            {
                pictureBoxReserva.Visible = false;
                mesa1.Visible = false;
                mesa2.Visible = false;
                mesa3.Visible = false;
                mesa4.Visible = false;
                mesa5.Visible = false;
            }
        }

        private void ComprobarDisponibilidadMesas(DateTime fecha, string tipo)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string query = "SELECT Mesa, IdUsuario FROM Reservas WHERE FechaReserva = ? AND Tipo = ?";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        // Agregar los parámetros de manera explícita
                        command.Parameters.Add(new OleDbParameter("@FechaReserva", OleDbType.Date)).Value = fecha;
                        command.Parameters.Add(new OleDbParameter("@Tipo", OleDbType.VarChar)).Value = tipo;

                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            // Inicializar todas las mesas como disponibles (verde) y sin texto
                            ResetMesas();

                            // Marcar las mesas reservadas y actualizar el texto con el IdUsuario
                            while (reader.Read())
                            {
                                string mesas = reader.GetString(0);
                                int idUsuario = reader.GetInt32(1);
                                string[] mesasArray = mesas.Split(',');

                                foreach (string mesaStr in mesasArray)
                                {
                                    if (int.TryParse(mesaStr, out int mesa))
                                    {
                                        switch (mesa)
                                        {
                                            case 1:
                                                mesa1.Text = idUsuario.ToString();
                                                mesa1.BackColor = idUsuario == currentUserId ? Color.Orange : Color.Red;
                                                break;
                                            case 2:
                                                mesa2.Text = idUsuario.ToString();
                                                mesa2.BackColor = idUsuario == currentUserId ? Color.Orange : Color.Red;
                                                break;
                                            case 3:
                                                mesa3.Text = idUsuario.ToString();
                                                mesa3.BackColor = idUsuario == currentUserId ? Color.Orange : Color.Red;
                                                break;
                                            case 4:
                                                mesa4.Text = idUsuario.ToString();
                                                mesa4.BackColor = idUsuario == currentUserId ? Color.Orange : Color.Red;
                                                break;
                                            case 5:
                                                mesa5.Text = idUsuario.ToString();
                                                mesa5.BackColor = idUsuario == currentUserId ? Color.Orange : Color.Red;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al comprobar la disponibilidad de las mesas: " + ex.Message);
                }
            }
        }




        private void ResetMesas()
        {
            mesa1.BackColor = Color.Green;
            mesa1.Text = string.Empty;
            mesa2.BackColor = Color.Green;
            mesa2.Text = string.Empty;
            mesa3.BackColor = Color.Green;
            mesa3.Text = string.Empty;
            mesa4.BackColor = Color.Green;
            mesa4.Text = string.Empty;
            mesa5.BackColor = Color.Green;
            mesa5.Text = string.Empty;
        }
        private void Mesa_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Button mesa = sender as System.Windows.Forms.Button;
            if (mesa != null)
            {
                CambiarColorMesa(mesa);
            }
        }
        private void CambiarColorMesa(System.Windows.Forms.Button mesa)
        {
            if (mesa.BackColor == Color.Red)
            {
                // No hacer nada si la mesa está en rojo
                return;
            }
            else if (mesa.BackColor == Color.Green)
            {
                // Cambiar a naranja si la mesa está en verde
                mesa.BackColor = Color.Orange;
                mesa.Text = currentUserId.ToString();
            }
            else if (mesa.BackColor == Color.Orange)
            {
                // Cambiar a verde si la mesa está en naranja
                mesa.Text = "";
                mesa.BackColor = Color.Green;
            }
        }

        private void reservarBtn_Click(object sender, EventArgs e)
        {
            // Recoger las mesas en naranja
            List<int> mesasNaranja = new List<int>();
            if (mesa1.BackColor == Color.Orange) mesasNaranja.Add(1);
            if (mesa2.BackColor == Color.Orange) mesasNaranja.Add(2);
            if (mesa3.BackColor == Color.Orange) mesasNaranja.Add(3);
            if (mesa4.BackColor == Color.Orange) mesasNaranja.Add(4);
            if (mesa5.BackColor == Color.Orange) mesasNaranja.Add(5);


            // Verificar si hay mesas en naranja
            if (mesasNaranja.Count == 0)
            {
                MessageBox.Show("No hay mesas seleccionadas para reservar.");
                ComprobarDisponibilidadMesas(dateTimePicker.Value.Date, comboBoxTipoReserva.SelectedItem.ToString());
                return;
            }

            // Formatear las mesas en una cadena separada por comas
            string mesas = string.Join(",", mesasNaranja);

            // Obtener la fecha y el tipo de reserva
            DateTime fechaReserva = dateTimePicker.Value.Date;
            string tipoReserva = comboBoxTipoReserva.SelectedItem.ToString();

            // Cadena de conexión a la base de datos
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Verificar si ya existe una reserva para el usuario, la fecha y el tipo
                    string checkQuery = "SELECT COUNT(*) FROM Reservas WHERE IdUsuario = ? AND FechaReserva = ? AND Tipo = ?";
                    using (OleDbCommand checkCommand = new OleDbCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@IdUsuario", currentUserId);
                        checkCommand.Parameters.AddWithValue("@FechaReserva", fechaReserva.ToString("dd/MM/yyyy"));
                        checkCommand.Parameters.AddWithValue("@Tipo", tipoReserva);

                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            // Si ya existe una reserva, actualizarla
                            string updateQuery = "UPDATE Reservas SET Mesa = ? WHERE IdUsuario = ? AND FechaReserva = ? AND Tipo = ?";
                            using (OleDbCommand updateCommand = new OleDbCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@Mesa", mesas);
                                updateCommand.Parameters.AddWithValue("@IdUsuario", currentUserId);
                                updateCommand.Parameters.AddWithValue("@FechaReserva", fechaReserva.ToString("dd/MM/yyyy"));
                                updateCommand.Parameters.AddWithValue("@Tipo", tipoReserva);

                                updateCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Reserva actualizada con éxito.");
                        }
                        else
                        {
                            // Si no existe una reserva, insertar una nueva
                            string insertQuery = "INSERT INTO Reservas (IdUsuario, FechaReserva, Tipo, Mesa) VALUES (?, ?, ?, ?)";
                            using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@IdUsuario", currentUserId);
                                insertCommand.Parameters.AddWithValue("@FechaReserva", fechaReserva.ToString("dd/MM/yyyy"));
                                insertCommand.Parameters.AddWithValue("@Tipo", tipoReserva);
                                insertCommand.Parameters.AddWithValue("@Mesa", mesas);

                                insertCommand.ExecuteNonQuery();
                            }

                            MessageBox.Show("Reserva realizada con éxito.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al realizar la reserva: " + ex.Message);
                }
            }

            ComprobarDisponibilidadMesas(dateTimePicker.Value.Date, comboBoxTipoReserva.SelectedItem.ToString());
        }

        private void buttonEliminarReserva_Click(object sender, EventArgs e)
        {
            // Obtener la fecha y el tipo de reserva
            DateTime fechaReserva = dateTimePicker.Value.Date;
            string tipoReserva = comboBoxTipoReserva.SelectedItem.ToString();

            // Cadena de conexión a la base de datos
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Consulta SQL para eliminar la reserva
                    string deleteQuery = "DELETE FROM Reservas WHERE IdUsuario = ? AND FechaReserva = ? AND Tipo = ?";
                    using (OleDbCommand deleteCommand = new OleDbCommand(deleteQuery, connection))
                    {
                        deleteCommand.Parameters.AddWithValue("@IdUsuario", currentUserId);
                        deleteCommand.Parameters.AddWithValue("@FechaReserva", fechaReserva.ToString("dd/MM/yyyy"));
                        deleteCommand.Parameters.AddWithValue("@Tipo", tipoReserva);

                        int rowsAffected = deleteCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Reserva eliminada con éxito.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontró ninguna reserva para eliminar.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar la reserva: " + ex.Message);
                }
            }

            // Actualizar la disponibilidad de las mesas
            ComprobarDisponibilidadMesas(dateTimePicker.Value.Date, comboBoxTipoReserva.SelectedItem.ToString());
        }

        private void GenerarInforme_Click(object sender, EventArgs e)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";
            string query = "SELECT * FROM Productos"; // Seleccionar todos los datos de la tabla Productos

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(query, connection))
                    {
                        using (OleDbDataReader reader = command.ExecuteReader())
                        {
                            // Crear un documento PDF con orientación horizontal
                            Document document = new Document(PageSize.A4.Rotate());
                            PdfWriter.GetInstance(document, new FileStream("InformeProductos.pdf", FileMode.Create));
                            document.Open();

                            // Agregar título al documento
                            document.Add(new Paragraph("Informe de Productos", FontFactory.GetFont(FontFactory.COURIER_BOLD, 16)));
                            document.Add(new Paragraph(" ", FontFactory.GetFont(FontFactory.COURIER, 12))); // Espacio en blanco

                            // Agregar encabezados de columna
                            document.Add(new Paragraph("ID".PadRight(10) + "Nombre".PadRight(30) + "Precio".PadRight(10) + "Descripción".PadRight(30) + "Categoría".PadRight(15) + "Cantidad".PadRight(15), FontFactory.GetFont(FontFactory.COURIER_BOLD, 12)));
                            document.Add(new Paragraph(new string('-', 106), FontFactory.GetFont(FontFactory.COURIER, 12))); // Línea separadora

                            // Agregar datos de la base de datos al documento
                            while (reader.Read())
                            {
                                string id = reader["Id"].ToString().PadRight(10);
                                string nombre = reader["NombreProducto"].ToString().PadRight(30);
                                string precio = (reader["Precio"].ToString() + " €").PadRight(10);
                                string descripcion = reader["Descripcion"].ToString().PadRight(30);
                                string categoria = reader["Categoria"].ToString().PadRight(20);
                                string cantidad = reader["Cantidad"].ToString();
                                int minStock = int.Parse(reader["min_stock"].ToString());

                                // Crear un Chunk para la cantidad con el color adecuado
                                BaseColor colorCantidad = int.Parse(cantidad) < minStock ? BaseColor.RED : BaseColor.GREEN;
                                Chunk cantidadChunk = new Chunk(cantidad.PadRight(10), FontFactory.GetFont(FontFactory.COURIER, 12, colorCantidad));

                                // Crear una frase que contenga todos los datos
                                Paragraph line = new Paragraph();
                                line.Add(new Chunk(id, FontFactory.GetFont(FontFactory.COURIER, 12)));
                                line.Add(new Chunk(nombre, FontFactory.GetFont(FontFactory.COURIER, 12)));
                                line.Add(new Chunk(precio, FontFactory.GetFont(FontFactory.COURIER, 12)));
                                line.Add(new Chunk(descripcion, FontFactory.GetFont(FontFactory.COURIER, 12)));
                                line.Add(new Chunk(categoria, FontFactory.GetFont(FontFactory.COURIER, 12)));
                                line.Add(cantidadChunk);

                                // Agregar la frase al documento
                                document.Add(line);
                            }

                            document.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el informe: " + ex.Message);
                }
            }

            MessageBox.Show("Informe generado correctamente en InformeProductos.pdf");
        }



    }

}





