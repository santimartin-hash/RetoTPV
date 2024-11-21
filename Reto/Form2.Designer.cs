namespace Reto
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.panel1 = new System.Windows.Forms.Panel();
            this.salirBtn = new System.Windows.Forms.Button();
            this.MenuUsuariosBtn = new System.Windows.Forms.Button();
            this.MenuStockBtn = new System.Windows.Forms.Button();
            this.MenuCajaBtn = new System.Windows.Forms.Button();
            this.MenuReservaBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelReservas = new System.Windows.Forms.Panel();
            this.panelCaja = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.TotalTextBox = new System.Windows.Forms.TextBox();
            this.imprimirBtn = new System.Windows.Forms.Button();
            this.labelcantidad = new System.Windows.Forms.Label();
            this.QuitarProductoBtn = new System.Windows.Forms.Button();
            this.cantStockDisponible = new System.Windows.Forms.Label();
            this.AñadirProductoBtn = new System.Windows.Forms.Button();
            this.CantidadProductAñadir = new System.Windows.Forms.NumericUpDown();
            this.productPictureBox = new System.Windows.Forms.PictureBox();
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            this.listViewItems = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panelAlmacen = new System.Windows.Forms.Panel();
            this.textBoxIdProducto = new System.Windows.Forms.TextBox();
            this.dataGridViewAlmacen = new System.Windows.Forms.DataGridView();
            this.eliminarProductoBtn = new System.Windows.Forms.Button();
            this.agregarProductoBtn = new System.Windows.Forms.Button();
            this.imagenProducto = new System.Windows.Forms.PictureBox();
            this.cantidadProducto = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxPrecioProducto = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxImagenProducto = new System.Windows.Forms.TextBox();
            this.textBoxCategoriaProducto = new System.Windows.Forms.TextBox();
            this.textBoxDescripcionProducto = new System.Windows.Forms.TextBox();
            this.textBoxNombreProducto = new System.Windows.Forms.TextBox();
            this.NombreProducto = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelCaja.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CantidadProductAñadir)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.panelAlmacen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAlmacen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagenProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cantidadProducto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGreen;
            this.panel1.Controls.Add(this.salirBtn);
            this.panel1.Controls.Add(this.MenuUsuariosBtn);
            this.panel1.Controls.Add(this.MenuStockBtn);
            this.panel1.Controls.Add(this.MenuCajaBtn);
            this.panel1.Controls.Add(this.MenuReservaBtn);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(-3, 1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(215, 475);
            this.panel1.TabIndex = 0;
            // 
            // salirBtn
            // 
            this.salirBtn.FlatAppearance.BorderSize = 0;
            this.salirBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.salirBtn.Font = new System.Drawing.Font("Palatino Linotype", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.salirBtn.Location = new System.Drawing.Point(6, 415);
            this.salirBtn.Name = "salirBtn";
            this.salirBtn.Size = new System.Drawing.Size(205, 48);
            this.salirBtn.TabIndex = 5;
            this.salirBtn.Text = "Salir";
            this.salirBtn.UseVisualStyleBackColor = true;
            this.salirBtn.Click += new System.EventHandler(this.salirBtn_Click);
            // 
            // MenuUsuariosBtn
            // 
            this.MenuUsuariosBtn.FlatAppearance.BorderSize = 0;
            this.MenuUsuariosBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MenuUsuariosBtn.Font = new System.Drawing.Font("Palatino Linotype", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuUsuariosBtn.Location = new System.Drawing.Point(7, 251);
            this.MenuUsuariosBtn.Name = "MenuUsuariosBtn";
            this.MenuUsuariosBtn.Size = new System.Drawing.Size(205, 48);
            this.MenuUsuariosBtn.TabIndex = 4;
            this.MenuUsuariosBtn.Text = "Usuarios";
            this.MenuUsuariosBtn.UseVisualStyleBackColor = true;
            // 
            // MenuStockBtn
            // 
            this.MenuStockBtn.FlatAppearance.BorderSize = 0;
            this.MenuStockBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MenuStockBtn.Font = new System.Drawing.Font("Palatino Linotype", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuStockBtn.Location = new System.Drawing.Point(7, 197);
            this.MenuStockBtn.Name = "MenuStockBtn";
            this.MenuStockBtn.Size = new System.Drawing.Size(205, 48);
            this.MenuStockBtn.TabIndex = 3;
            this.MenuStockBtn.Text = "Almacen";
            this.MenuStockBtn.UseVisualStyleBackColor = true;
            this.MenuStockBtn.Click += new System.EventHandler(this.MenuStockBtn_Click);
            // 
            // MenuCajaBtn
            // 
            this.MenuCajaBtn.FlatAppearance.BorderSize = 0;
            this.MenuCajaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MenuCajaBtn.Font = new System.Drawing.Font("Palatino Linotype", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuCajaBtn.Location = new System.Drawing.Point(7, 89);
            this.MenuCajaBtn.Name = "MenuCajaBtn";
            this.MenuCajaBtn.Size = new System.Drawing.Size(205, 48);
            this.MenuCajaBtn.TabIndex = 1;
            this.MenuCajaBtn.Text = "Caja";
            this.MenuCajaBtn.UseVisualStyleBackColor = true;
            this.MenuCajaBtn.Click += new System.EventHandler(this.MenuCajaBtn_Click);
            // 
            // MenuReservaBtn
            // 
            this.MenuReservaBtn.FlatAppearance.BorderSize = 0;
            this.MenuReservaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.MenuReservaBtn.Font = new System.Drawing.Font("Palatino Linotype", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MenuReservaBtn.Location = new System.Drawing.Point(6, 143);
            this.MenuReservaBtn.Name = "MenuReservaBtn";
            this.MenuReservaBtn.Size = new System.Drawing.Size(205, 48);
            this.MenuReservaBtn.TabIndex = 2;
            this.MenuReservaBtn.Text = "Reservas";
            this.MenuReservaBtn.UseVisualStyleBackColor = true;
            this.MenuReservaBtn.Click += new System.EventHandler(this.MenuReservaBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(3, 11);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(212, 85);
            this.panel2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-6, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(247, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "________________________________________";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.Location = new System.Drawing.Point(5, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(197, 43);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // panelReservas
            // 
            this.panelReservas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panelReservas.Location = new System.Drawing.Point(211, 0);
            this.panelReservas.Name = "panelReservas";
            this.panelReservas.Size = new System.Drawing.Size(796, 475);
            this.panelReservas.TabIndex = 1;
            // 
            // panelCaja
            // 
            this.panelCaja.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panelCaja.Controls.Add(this.label2);
            this.panelCaja.Controls.Add(this.TotalTextBox);
            this.panelCaja.Controls.Add(this.imprimirBtn);
            this.panelCaja.Controls.Add(this.labelcantidad);
            this.panelCaja.Controls.Add(this.QuitarProductoBtn);
            this.panelCaja.Controls.Add(this.cantStockDisponible);
            this.panelCaja.Controls.Add(this.AñadirProductoBtn);
            this.panelCaja.Controls.Add(this.CantidadProductAñadir);
            this.panelCaja.Controls.Add(this.productPictureBox);
            this.panelCaja.Controls.Add(this.dataGridViewProducts);
            this.panelCaja.Controls.Add(this.listViewItems);
            this.panelCaja.Location = new System.Drawing.Point(211, 0);
            this.panelCaja.Name = "panelCaja";
            this.panelCaja.Size = new System.Drawing.Size(799, 475);
            this.panelCaja.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(648, 409);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Total:";
            // 
            // TotalTextBox
            // 
            this.TotalTextBox.Location = new System.Drawing.Point(688, 406);
            this.TotalTextBox.Name = "TotalTextBox";
            this.TotalTextBox.ReadOnly = true;
            this.TotalTextBox.Size = new System.Drawing.Size(100, 20);
            this.TotalTextBox.TabIndex = 10;
            this.TotalTextBox.Text = "0,00 €";
            // 
            // imprimirBtn
            // 
            this.imprimirBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.imprimirBtn.Font = new System.Drawing.Font("Palatino Linotype", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imprimirBtn.Location = new System.Drawing.Point(688, 441);
            this.imprimirBtn.Name = "imprimirBtn";
            this.imprimirBtn.Size = new System.Drawing.Size(100, 23);
            this.imprimirBtn.TabIndex = 9;
            this.imprimirBtn.Text = "Imprimir ticket";
            this.imprimirBtn.UseVisualStyleBackColor = true;
            this.imprimirBtn.Click += new System.EventHandler(this.imprimirBtn_Click);
            // 
            // labelcantidad
            // 
            this.labelcantidad.AutoSize = true;
            this.labelcantidad.Location = new System.Drawing.Point(651, 125);
            this.labelcantidad.Name = "labelcantidad";
            this.labelcantidad.Size = new System.Drawing.Size(52, 13);
            this.labelcantidad.TabIndex = 8;
            this.labelcantidad.Text = "Cantidad:";
            this.labelcantidad.Visible = false;
            // 
            // QuitarProductoBtn
            // 
            this.QuitarProductoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.QuitarProductoBtn.Font = new System.Drawing.Font("Palatino Linotype", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuitarProductoBtn.Location = new System.Drawing.Point(713, 180);
            this.QuitarProductoBtn.Name = "QuitarProductoBtn";
            this.QuitarProductoBtn.Size = new System.Drawing.Size(75, 23);
            this.QuitarProductoBtn.TabIndex = 7;
            this.QuitarProductoBtn.Text = "Quitar";
            this.QuitarProductoBtn.UseVisualStyleBackColor = true;
            this.QuitarProductoBtn.Visible = false;
            this.QuitarProductoBtn.Click += new System.EventHandler(this.QuitarProductoBtn_Click);
            // 
            // cantStockDisponible
            // 
            this.cantStockDisponible.AutoSize = true;
            this.cantStockDisponible.Location = new System.Drawing.Point(652, 153);
            this.cantStockDisponible.Name = "cantStockDisponible";
            this.cantStockDisponible.Size = new System.Drawing.Size(0, 13);
            this.cantStockDisponible.TabIndex = 6;
            this.cantStockDisponible.Visible = false;
            // 
            // AñadirProductoBtn
            // 
            this.AñadirProductoBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.AñadirProductoBtn.Font = new System.Drawing.Font("Palatino Linotype", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AñadirProductoBtn.Location = new System.Drawing.Point(628, 180);
            this.AñadirProductoBtn.Name = "AñadirProductoBtn";
            this.AñadirProductoBtn.Size = new System.Drawing.Size(75, 23);
            this.AñadirProductoBtn.TabIndex = 5;
            this.AñadirProductoBtn.Text = "Añadir";
            this.AñadirProductoBtn.UseVisualStyleBackColor = true;
            this.AñadirProductoBtn.Visible = false;
            this.AñadirProductoBtn.Click += new System.EventHandler(this.AñadirProductoBtn_Click);
            // 
            // CantidadProductAñadir
            // 
            this.CantidadProductAñadir.Location = new System.Drawing.Point(713, 123);
            this.CantidadProductAñadir.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CantidadProductAñadir.Name = "CantidadProductAñadir";
            this.CantidadProductAñadir.Size = new System.Drawing.Size(62, 20);
            this.CantidadProductAñadir.TabIndex = 4;
            this.CantidadProductAñadir.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.CantidadProductAñadir.Visible = false;
            // 
            // productPictureBox
            // 
            this.productPictureBox.Location = new System.Drawing.Point(637, 15);
            this.productPictureBox.Name = "productPictureBox";
            this.productPictureBox.Size = new System.Drawing.Size(151, 102);
            this.productPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.productPictureBox.TabIndex = 3;
            this.productPictureBox.TabStop = false;
            this.productPictureBox.Visible = false;
            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.ColumnHeadersHeight = 18;
            this.dataGridViewProducts.Location = new System.Drawing.Point(31, 15);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.ReadOnly = true;
            this.dataGridViewProducts.Size = new System.Drawing.Size(580, 254);
            this.dataGridViewProducts.TabIndex = 2;
            // 
            // listViewItems
            // 
            this.listViewItems.HideSelection = false;
            this.listViewItems.Location = new System.Drawing.Point(31, 286);
            this.listViewItems.Name = "listViewItems";
            this.listViewItems.Size = new System.Drawing.Size(580, 178);
            this.listViewItems.TabIndex = 1;
            this.listViewItems.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList2.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // panelAlmacen
            // 
            this.panelAlmacen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panelAlmacen.Controls.Add(this.textBoxIdProducto);
            this.panelAlmacen.Controls.Add(this.dataGridViewAlmacen);
            this.panelAlmacen.Controls.Add(this.eliminarProductoBtn);
            this.panelAlmacen.Controls.Add(this.agregarProductoBtn);
            this.panelAlmacen.Controls.Add(this.imagenProducto);
            this.panelAlmacen.Controls.Add(this.cantidadProducto);
            this.panelAlmacen.Controls.Add(this.label6);
            this.panelAlmacen.Controls.Add(this.label7);
            this.panelAlmacen.Controls.Add(this.textBoxPrecioProducto);
            this.panelAlmacen.Controls.Add(this.label5);
            this.panelAlmacen.Controls.Add(this.label4);
            this.panelAlmacen.Controls.Add(this.label3);
            this.panelAlmacen.Controls.Add(this.textBoxImagenProducto);
            this.panelAlmacen.Controls.Add(this.textBoxCategoriaProducto);
            this.panelAlmacen.Controls.Add(this.textBoxDescripcionProducto);
            this.panelAlmacen.Controls.Add(this.textBoxNombreProducto);
            this.panelAlmacen.Controls.Add(this.NombreProducto);
            this.panelAlmacen.Controls.Add(this.pictureBox2);
            this.panelAlmacen.Controls.Add(this.pictureBox3);
            this.panelAlmacen.Location = new System.Drawing.Point(211, 0);
            this.panelAlmacen.Name = "panelAlmacen";
            this.panelAlmacen.Size = new System.Drawing.Size(796, 475);
            this.panelAlmacen.TabIndex = 2;
            this.panelAlmacen.Visible = false;
            // 
            // textBoxIdProducto
            // 
            this.textBoxIdProducto.Location = new System.Drawing.Point(671, 54);
            this.textBoxIdProducto.Name = "textBoxIdProducto";
            this.textBoxIdProducto.Size = new System.Drawing.Size(32, 20);
            this.textBoxIdProducto.TabIndex = 19;
            this.textBoxIdProducto.Visible = false;
            // 
            // dataGridViewAlmacen
            // 
            this.dataGridViewAlmacen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAlmacen.Location = new System.Drawing.Point(31, 16);
            this.dataGridViewAlmacen.Name = "dataGridViewAlmacen";
            this.dataGridViewAlmacen.Size = new System.Drawing.Size(539, 388);
            this.dataGridViewAlmacen.TabIndex = 18;
            // 
            // eliminarProductoBtn
            // 
            this.eliminarProductoBtn.Location = new System.Drawing.Point(688, 306);
            this.eliminarProductoBtn.Name = "eliminarProductoBtn";
            this.eliminarProductoBtn.Size = new System.Drawing.Size(87, 23);
            this.eliminarProductoBtn.TabIndex = 17;
            this.eliminarProductoBtn.Text = "Eliminar";
            this.eliminarProductoBtn.UseVisualStyleBackColor = true;
            this.eliminarProductoBtn.Click += new System.EventHandler(this.eliminarProductoBtn_Click);
            // 
            // agregarProductoBtn
            // 
            this.agregarProductoBtn.Location = new System.Drawing.Point(579, 306);
            this.agregarProductoBtn.Name = "agregarProductoBtn";
            this.agregarProductoBtn.Size = new System.Drawing.Size(87, 23);
            this.agregarProductoBtn.TabIndex = 16;
            this.agregarProductoBtn.Text = "Agregar";
            this.agregarProductoBtn.UseVisualStyleBackColor = true;
            this.agregarProductoBtn.Click += new System.EventHandler(this.agregarProductoBtn_Click);
            // 
            // imagenProducto
            // 
            this.imagenProducto.Location = new System.Drawing.Point(617, 12);
            this.imagenProducto.Name = "imagenProducto";
            this.imagenProducto.Size = new System.Drawing.Size(124, 96);
            this.imagenProducto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imagenProducto.TabIndex = 15;
            this.imagenProducto.TabStop = false;
            // 
            // cantidadProducto
            // 
            this.cantidadProducto.Location = new System.Drawing.Point(688, 265);
            this.cantidadProducto.Name = "cantidadProducto";
            this.cantidadProducto.Size = new System.Drawing.Size(91, 20);
            this.cantidadProducto.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(685, 248);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Cantidad";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(576, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(37, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Precio";
            // 
            // textBoxPrecioProducto
            // 
            this.textBoxPrecioProducto.Location = new System.Drawing.Point(579, 264);
            this.textBoxPrecioProducto.Name = "textBoxPrecioProducto";
            this.textBoxPrecioProducto.Size = new System.Drawing.Size(87, 20);
            this.textBoxPrecioProducto.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(685, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Imagen (URL)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(576, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Categoria";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(617, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Descripcion";
            // 
            // textBoxImagenProducto
            // 
            this.textBoxImagenProducto.Location = new System.Drawing.Point(688, 218);
            this.textBoxImagenProducto.Name = "textBoxImagenProducto";
            this.textBoxImagenProducto.Size = new System.Drawing.Size(91, 20);
            this.textBoxImagenProducto.TabIndex = 5;
            this.textBoxImagenProducto.TextChanged += new System.EventHandler(this.TextBoxImagenProducto_TextChanged);
            // 
            // textBoxCategoriaProducto
            // 
            this.textBoxCategoriaProducto.Location = new System.Drawing.Point(579, 218);
            this.textBoxCategoriaProducto.Name = "textBoxCategoriaProducto";
            this.textBoxCategoriaProducto.Size = new System.Drawing.Size(87, 20);
            this.textBoxCategoriaProducto.TabIndex = 4;
            // 
            // textBoxDescripcionProducto
            // 
            this.textBoxDescripcionProducto.Location = new System.Drawing.Point(617, 169);
            this.textBoxDescripcionProducto.Name = "textBoxDescripcionProducto";
            this.textBoxDescripcionProducto.Size = new System.Drawing.Size(124, 20);
            this.textBoxDescripcionProducto.TabIndex = 3;
            // 
            // textBoxNombreProducto
            // 
            this.textBoxNombreProducto.Location = new System.Drawing.Point(617, 130);
            this.textBoxNombreProducto.Name = "textBoxNombreProducto";
            this.textBoxNombreProducto.Size = new System.Drawing.Size(124, 20);
            this.textBoxNombreProducto.TabIndex = 2;
            // 
            // NombreProducto
            // 
            this.NombreProducto.AutoSize = true;
            this.NombreProducto.Location = new System.Drawing.Point(617, 114);
            this.NombreProducto.Name = "NombreProducto";
            this.NombreProducto.Size = new System.Drawing.Size(90, 13);
            this.NombreProducto.TabIndex = 1;
            this.NombreProducto.Text = "Nombre Producto";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(579, 198);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(203, 143);
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new System.Drawing.Point(610, 114);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(148, 85);
            this.pictureBox3.TabIndex = 21;
            this.pictureBox3.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1011, 476);
            this.Controls.Add(this.panelAlmacen);
            this.Controls.Add(this.panelCaja);
            this.Controls.Add(this.panelReservas);
            this.Controls.Add(this.panel1);
            this.Name = "Form2";
            this.Text = "SidraLovers";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelCaja.ResumeLayout(false);
            this.panelCaja.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CantidadProductAñadir)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.panelAlmacen.ResumeLayout(false);
            this.panelAlmacen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAlmacen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imagenProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cantidadProducto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button MenuReservaBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button MenuUsuariosBtn;
        private System.Windows.Forms.Button MenuStockBtn;
        private System.Windows.Forms.Button MenuCajaBtn;
        private System.Windows.Forms.Panel panelReservas;
        private System.Windows.Forms.Panel panelCaja;
        private System.Windows.Forms.ListView listViewItems;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridView dataGridViewProducts;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Button AñadirProductoBtn;
        private System.Windows.Forms.NumericUpDown CantidadProductAñadir;
        private System.Windows.Forms.PictureBox productPictureBox;
        private System.Windows.Forms.Label cantStockDisponible;
        private System.Windows.Forms.Button QuitarProductoBtn;
        private System.Windows.Forms.Label labelcantidad;
        private System.Windows.Forms.Button salirBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TotalTextBox;
        private System.Windows.Forms.Button imprimirBtn;
        private System.Windows.Forms.Panel panelAlmacen;
        private System.Windows.Forms.Label NombreProducto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxImagenProducto;
        private System.Windows.Forms.TextBox textBoxCategoriaProducto;
        private System.Windows.Forms.TextBox textBoxDescripcionProducto;
        private System.Windows.Forms.TextBox textBoxNombreProducto;
        private System.Windows.Forms.Button eliminarProductoBtn;
        private System.Windows.Forms.Button agregarProductoBtn;
        private System.Windows.Forms.PictureBox imagenProducto;
        private System.Windows.Forms.NumericUpDown cantidadProducto;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxPrecioProducto;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dataGridViewAlmacen;
        private System.Windows.Forms.TextBox textBoxIdProducto;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}