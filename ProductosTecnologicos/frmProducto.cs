using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductosTecnologicos
{
    public partial class FrmProductos : Form
    {
        SqlConnection conexion = new SqlConnection(@"Data Source=LAPTOP-45JU0EKC;Initial Catalog=ProductosT;Integrated Security=True");
        SqlCommand comando = new SqlCommand();
        SqlDataReader lector;
        bool nuevo = false;
        const int tam = 10;
        Producto[] aProducto = new Producto[tam];
        int c;


        public FrmProductos()
        {
            InitializeComponent();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            nuevo = true;
            habilitar(true);
            limpiar();
            txtCodigo.Focus();
        }
        private void habilitar(bool x)
        {
            txtCodigo.Enabled = x;
            txtNombre.Enabled = x;
            cboMarca.Enabled = x;
            txtFecha.Enabled = x;
            txtPrecio.Enabled = x;
        }
        private void limpiar()
        {
            txtCodigo.Text = "";
            txtNombre.Text = "";
            cboMarca.Text = "";
            txtFecha.Text = "";
            txtPrecio.Text = "";
        }

        private void cargarCombo(ComboBox combo, string nombreTabla)
        {
            DataTable tabla = consultarTabla(nombreTabla);
            combo.DataSource = tabla;
            combo.ValueMember = tabla.Columns[0].ColumnName;
            combo.DisplayMember = tabla.Columns[1].ColumnName;
        }
        private DataTable consultarTabla(string nombreTabla)
        {
            DataTable tabla = new DataTable();
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText= "SELECT* FROM " + nombreTabla;
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            return tabla;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            string consultarSql = "";


            if (validarCampos())
            {
                Producto p = new Producto();
                p.pCodigo = int.Parse(txtCodigo.Text);
                p.pNombre = txtNombre.Text;
                p.pMarca = Convert.ToInt32(cboMarca.SelectedValue);
                p.pFecha = DateTime.Parse(txtFecha.Text);
                p.pPrecio = Convert.ToDouble(txtPrecio.Text);

                if (nuevo)
                {
                    consultarSql = "INSERT INTO Productos (Codigo,Nombre,Marca,Fecha,Precio)" +
                                  "VALUES (@Codigo,@Nombre,@Marca,@Fecha,@Precio)";
                    conexion.Open();
                    comando = new SqlCommand();

                    comando.Connection = conexion;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = consultarSql;
                    comando.Parameters.AddWithValue("@Codigo",p.pCodigo);
                    comando.Parameters.AddWithValue("@Nombre",p.pNombre);
                    comando.Parameters.AddWithValue("@Marca", p.pMarca);
                    comando.Parameters.AddWithValue("@Fecha",p.pFecha);
                    comando.Parameters.AddWithValue("@Precio", p.pPrecio);

                    comando.ExecuteNonQuery();
                    conexion.Close();
                }
                else
                {
                    consultarSql = "UPDATE  Productos SET Codigo=@Codigo," +
                                                     " Nombre=@Nombre," +
                                                     " Marca=@Marca," +
                                                     " Precio=@Precio," +
                                                     "Fecha=@Fecha,"+
                                                     " WHERE codigo=@codigo";
                    conexion.Open();
                    comando = new SqlCommand();
                    comando.Connection = conexion;
                    comando.CommandType = CommandType.Text;
                    comando.CommandText = consultarSql;
                    comando.Parameters.AddWithValue("@Codigo", p.pCodigo);
                    comando.Parameters.AddWithValue("@Nombre", p.pNombre);
                    comando.Parameters.AddWithValue("@Bodega", p.pMarca);
                    comando.Parameters.AddWithValue("@Precio", p.pPrecio);
                    comando.Parameters.AddWithValue("@Fecha", p.pFecha);

                    comando.ExecuteNonQuery();
                    conexion.Close();
                }
                  this.cargarLista(lstProductos, "Productos");
                  habilitar(false);
                  nuevo = false;
            }
        }
        private bool validarCampos()
        {
            if (txtCodigo.Text == "")
            {
                MessageBox.Show("Debe ingresar un codigo...");
                txtCodigo.Focus();
                return false;
            }
            if (txtNombre.Text == "")
            {
                MessageBox.Show("Debe ingresar un Nombre...");
                txtNombre.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(cboMarca.Text))
            {
                MessageBox.Show("Debe seleccionar una Marca...");
                cboMarca.Focus();
                return false;
            }
            if (txtFecha.Text == "")
            {
                MessageBox.Show("Debe ingresar una Fecha...");
                txtFecha.Focus();
                return false;
            }
            if (txtPrecio.Text == "")
            {
                MessageBox.Show("Debe ingresar un Precio...");
                txtPrecio.Focus();
                return false;
            }

            return true;
        }
        private void cargarLista(ListBox lista,string nombreTabla)
        {
            c = 0;
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = "SELECT * FROM " + nombreTabla;
            lector = comando.ExecuteReader();
            while (lector.Read() == true)
            {
                Producto p = new Producto();
                if (!lector.IsDBNull(0))
                    p.pCodigo = lector.GetInt32(0);
                if (!lector.IsDBNull(1))
                    p.pNombre = lector.GetString(1);
                if (!lector.IsDBNull(2))
                    p.pMarca = lector.GetInt32(2);
                if (!lector.IsDBNull(3))
                    p.pFecha = lector.GetDateTime(3);
                if (!lector.IsDBNull(4))
                    p.pPrecio = lector.GetDouble(4);



                aProducto[c] = p;
                c++;

            }


            lector.Close();
            conexion.Close();

            lista.Items.Clear();
            for (int i = 0; i < c; i++)
            {
                lista.Items.Add(aProducto[i].ToString());

            }
            //lista.SelectedIndex = 0;

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro de abandonar la aplicación ?",
            "SALIR", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                this.Close();
        }

        
        private void FrmProductos_Load(object sender, EventArgs e)
        {
            habilitar(false);
            this.cargarLista(lstProductos, "Productos");
            this.cargarCombo(cboMarca, "Marcas");
        }
    }

}
