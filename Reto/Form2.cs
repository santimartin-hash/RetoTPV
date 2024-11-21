using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Reto
{
    public partial class Form2 : Form
    {
        public Form2(string userType)
        {
            InitializeComponent();
            ConfigureButtons(userType);
            InitializeTPV();
            InitializeDataGridView();
            // Inicializar columnas y estilo del DataGridView
            InitializeDataGridViewAlmacen();

            // Cargar productos en el DataGridView del panelAlmacen
            LoadProductsToDataGridViewAlmacen();
            panelCaja.Visible = true;
            panelReservas.Visible = false;
            // Suscribirse al evento MouseClick
            listViewItems.MouseClick += ListViewItems_MouseClick;
            panelAlmacen.Click += new EventHandler(panelAlmacen_Click);
            dataGridViewAlmacen.CellClick += new DataGridViewCellEventHandler(DataGridViewAlmacen_CellClick);
        
            // Configurar evento de clic
            listViewItems.Click += ListViewItems_Click;

            // Configurar el ListView para usar el modo de dibujo propietario
            listViewItems.OwnerDraw = true;
            listViewItems.DrawItem += ListViewItems_DrawItem;

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
            textBoxCategoriaProducto.Text = "";
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
            else if (userType == "Inv")
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

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=..\..\RETODESIN.accdb;Persist Security Info=False;";

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
                                Image image = DownloadImage(imagenUrl);
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
                e.Graphics.DrawImage(image, new Rectangle(imageX, imageY, imageWidth, imageHeight));
            }

            // Calcular la posición del texto más arriba
            int textX = e.Bounds.X + (e.Bounds.Width - TextRenderer.MeasureText(e.Item.Text, e.Item.Font).Width) / 2;
            int textY = e.Bounds.Y + imageHeight + 8; // Un pequeño margen entre la imagen y el texto
            TextRenderer.DrawText(e.Graphics, e.Item.Text, e.Item.Font, new Point(textX, textY), e.Item.ForeColor);

            // Dibujar el rectángulo de selección más grande
            if (e.Item.Selected)
            {
                int selectionPadding = 1; // Ajustar el tamaño del rectángulo de selección
                Rectangle selectionRectangle = new Rectangle(
                    e.Bounds.X,
                    e.Bounds.Y,
                    e.Bounds.Width - 2 * selectionPadding,
                    e.Bounds.Height - 2 * selectionPadding
                );

                e.Graphics.DrawRectangle(Pens.Black, selectionRectangle);
            }
        }

        private Image DownloadImage(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);
                using (var ms = new System.IO.MemoryStream(data))
                {
                    return Image.FromStream(ms);
                }
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
            InitializeTPV();
        }

        private void MenuCajaBtn_Click(object sender, EventArgs e)
        {
            InitializeTPV();
            panelCaja.Visible = true;
            panelReservas.Visible = false;
            panelAlmacen.Visible = false;
           
        }

        private void MenuReservaBtn_Click(object sender, EventArgs e)
        {
             panelReservas.Visible = true;
             panelCaja.Visible = false;
            panelAlmacen.Visible = false;
        }

        private void MenuStockBtn_Click(object sender, EventArgs e)
        {
            panelAlmacen.Visible = true;
            panelReservas.Visible = false;
            panelCaja.Visible = false;

        
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
                imagenProducto.Image = (Image)row.Cells["Imagen"].Value;
                textBoxNombreProducto.Text = row.Cells["NombreProducto"].Value.ToString();
                textBoxPrecioProducto.Text = row.Cells["Precio"].Value.ToString();
                textBoxDescripcionProducto.Text = row.Cells["Descripcion"].Value.ToString();
                textBoxCategoriaProducto.Text = row.Cells["Categoria"].Value.ToString();
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
                            dataTable.Columns.Add("Imagen", typeof(Image));
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
                            command.Parameters.AddWithValue("@Precio", decimal.Parse(textBoxPrecioProducto.Text));
                            command.Parameters.AddWithValue("@Descripcion", textBoxDescripcionProducto.Text);
                            command.Parameters.AddWithValue("@Categoria", textBoxCategoriaProducto.Text);
                            command.Parameters.AddWithValue("@Imagen(URL)", textBoxImagenProducto.Text);
                            command.Parameters.AddWithValue("@Cantidad", int.Parse(cantidadProducto.Text));
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
                            command.Parameters.AddWithValue("@Precio", decimal.Parse(textBoxPrecioProducto.Text ));
                            command.Parameters.AddWithValue("@Descripcion", textBoxDescripcionProducto.Text);
                            command.Parameters.AddWithValue("@Categoria", textBoxCategoriaProducto.Text);
                            command.Parameters.AddWithValue("@Imagen(URL)", textBoxImagenProducto.Text);
                            command.Parameters.AddWithValue("@Cantidad", int.Parse(cantidadProducto.Text));

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar el producto: " + ex.Message);
                }
                actualizarDataGridProductosAlmacen();

                textBoxIdProducto.Text = "";
                imagenProducto.Image = null;
                textBoxNombreProducto.Text = "";
                textBoxPrecioProducto.Text = "";
                textBoxDescripcionProducto.Text = "";
                textBoxCategoriaProducto.Text = "";
                cantidadProducto.Text = "";
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
        }



    }
}
  



