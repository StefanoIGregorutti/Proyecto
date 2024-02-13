using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductosTecnologicos
{
    class Producto
    {
        private int codigo;
        private string nombre;
        private int marca;
        private DateTime fecha;
        private double precio;

        public int pCodigo
        {
            get { return codigo; }
            set { codigo = value; }
        }
        public string pNombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public int pMarca
        {
            get { return marca; }
            set { marca = value; }
        }
        public DateTime pFecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public double pPrecio
        {
            get { return precio; }
            set { precio = value; }
        }

        public Producto()
        {
            this.codigo = 0;
            this.nombre = "";
            this.marca = 0;
            this.precio = 0;
            this.fecha = DateTime.Now;
        }

        public Producto(int codigo, string nombre, int marca, double precio, DateTime fecha)
        {
            this.codigo = codigo;
            this.nombre = nombre;
            this.marca = marca;
            this.fecha = fecha;
            this.precio = precio;
        }  
        public override string ToString()
        {
            return codigo + " - " + nombre + " - " + precio;
        }
    }
}
