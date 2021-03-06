﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace Historia_Clinica
{
   public class ClsSqlServer:ClsGestores
    {
       
        public static string ValorConfig;
        public int x;
        //Asignar la conexión establecida()
        public SqlConnection conexion = new SqlConnection();
        //private string Servidor = "";
        //private string BaseDatos = "";
        private string Query = "";
        private string cadenaC = Historia_Clinica.Conexion.CadenaConexion.cadena();
       
        //Propiedad para la sentencia a ejecutar()
        public override string CadnaSentencia { set { Query = value; } }

        //Para asignarle la cadena de conexión()

        public override string CadenaCnn { set { cadenaC = value; } }

        public override void Conectar()
        {
            try
            {                
                conexion.ConnectionString = cadenaC;
                conexion.Close();
                conexion.Open();
            }
            catch (Exception)
            {
            }
        }
        public async Task  ConectarAsync()
        {
            try
            {
                conexion.ConnectionString = cadenaC;
                await Task.Run(() => {
                    conexion.Open();
                });
                //MessageBox.Show(conexion.State.ToString());
            }
            catch (Exception)
            {
            }
        }
        //Permite ejecutar una sentencia en la base de datos()
        public override void Sentencia()
        {
            Conectar();
            SqlCommand Cmd = new SqlCommand(Query, conexion);
            Cmd.ExecuteNonQuery();
        }
        //Permite cargar los datos al DGV()
        public override void CargarDatos(DataGridView Dgv, string Consulta)
        {
            DataTable Tabla = new DataTable();
            SqlDataAdapter DataAd = new SqlDataAdapter(Consulta, conexion);
            DataAd.Fill(Tabla);
            Dgv.DataSource = Tabla;
            DataAd.Dispose();
        }

        public override DataTable LlenarTabla( string Consulta)
        {
            DataTable Tabla = new DataTable();
            try
            {
                Conectar();
                SqlDataAdapter DataAd = new SqlDataAdapter(Consulta, conexion);
                DataAd.Fill(Tabla);
                DataAd.Dispose();
                Tabla.Dispose();
            }
            catch (Exception e)
            {
                //MessageBox.Show("No se pudo establecer conexión con el servidor " + e.ToString());
                //Tabla = null;
            }
            return Tabla;

        }
        public  DataTable LlenarTablaAsync(string Consulta)
        {
            DataTable Tabla = new DataTable();
            try
            {
                SqlDataAdapter DataAd = new SqlDataAdapter();
                //DataAd.Dispose();
                 DataAd = new SqlDataAdapter(Consulta, conexion);
                DataAd.Fill(Tabla);
                DataAd.Dispose();
                Tabla.Dispose();
               
            }
            catch (Exception e)
            {
                //throw new Exception();
                //MessageBox.Show("No se pudo establecer conexión con el servidor " + e.ToString());
                //Tabla = null;
            }
            return Tabla;

        }
        public void Solo_Numeros(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    //el resto de teclas pulsadas se desactivan 
                    e.Handled = true;
                }
        }
        public void Solo_Letras(KeyPressEventArgs e)
        {

            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }

        }
       public string ValidarCelda(string valor)
        {
           string retornar="";

           if (valor == null)
               retornar = "";
           else
               retornar = valor;
            
           return valor;
        }
        public void EstilosDgv(DataGridView DGV)
        {
            DGV.DefaultCellStyle.BackColor = Color.WhiteSmoke;
            DGV.DefaultCellStyle.Font = new Font("Century Gothic", 10);
            Font prFont = new Font("Century Gothic", 10, FontStyle.Bold);
            for (int i = 0; i < DGV.Columns.Count; i++)
            {
                DGV.Columns[i].HeaderCell.Style.Font = prFont;
                DGV.Columns[i].HeaderCell.Style.ForeColor = Color.Black;
                DGV.Columns[i].HeaderCell.Style.BackColor = Color.Gainsboro;
                DGV.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
           // DGV.AutoResizeColumns();
            DGV.EnableHeadersVisualStyles = false;
        }

    }

}

