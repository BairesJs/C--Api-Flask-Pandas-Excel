using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Api_Flask_07_02_24
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textBox5.Text);
                string nombre = textBox1.Text;
                string direccion = textBox2.Text;
                string telefono = textBox3.Text;
                string email = textBox4.Text;

                var postData = new Dictionary<string, string>
    {
        { "id", id.ToString() },
        { "nombre", nombre },
        { "direccion", direccion },
        { "telefono", telefono },
        { "email", email }
    };

                string postUrl = "http://127.0.0.1:5000/form";

                using (var httpClient = new HttpClient())
                {
                    var postDataContent = new FormUrlEncodedContent(postData);

                    var postResponse = httpClient.PostAsync(postUrl, postDataContent).Result; // Síncrono

                    if (postResponse.IsSuccessStatusCode)
                    {
                        // MessageBox.Show("Datos enviados correctamente.");
                    }
                    else
                    {
                        MessageBox.Show("Error al enviar datos. Código de estado: " + postResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            ///////////////////////////////////////////////////////

            try
            {
                // Realizar la solicitud GET para obtener los datos más recientes
                string getDataUrl = "http://127.0.0.1:5000/data";

                using (var httpClient = new HttpClient())
                {
                    var getDataResponse = httpClient.GetAsync(getDataUrl).Result; // Síncrono

                    if (getDataResponse.IsSuccessStatusCode)
                    {
                        string responseData = getDataResponse.Content.ReadAsStringAsync().Result; // Síncrono
                        List<Dictionary<string, string>> dataList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(responseData);

                        // Crear DataTable y agregar columnas
                        DataTable dataTable = new DataTable();
                        if (dataList.Count > 0)
                        {
                            foreach (var key in dataList[0].Keys)
                            {
                                dataTable.Columns.Add(key);
                            }

                            // Agregar filas al DataTable
                            foreach (var data in dataList)
                            {
                                DataRow row = dataTable.NewRow();
                                foreach (var key in data.Keys)
                                {
                                    row[key] = data[key];
                                }
                                dataTable.Rows.Add(row);
                            }

                            // Actualizar el DataSource del DataGridView
                            dataGridView1.Invoke((MethodInvoker)delegate
                            {
                                dataGridView1.DataSource = dataTable;
                            });

                            if (dataGridView1.Columns.Count > 0)
                            {
                                // Mover la columna "Nombre" al principio
                                if (dataGridView1.Columns.Contains("id"))
                                    dataGridView1.Columns["id"].DisplayIndex = 0;

                                // Mover la columna "Edad" después de la columna "Nombre"
                                if (dataGridView1.Columns.Contains("nombre"))
                                    dataGridView1.Columns["nombre"].DisplayIndex = 1;

                                // Mover la columna "ID" al final
                                if (dataGridView1.Columns.Contains("direccion"))
                                    dataGridView1.Columns["direccion"].DisplayIndex = 2;

                                if (dataGridView1.Columns.Contains("telefono"))
                                    dataGridView1.Columns["telefono"].DisplayIndex = 3;

                                if (dataGridView1.Columns.Contains("mail"))
                                    dataGridView1.Columns["mail"].DisplayIndex = 2;
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron datos para actualizar.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los datos para actualizar. Código de estado: " + getDataResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los datos del DataGridView: " + ex.Message);
            }

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = true;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button1.Enabled = false;
            textBox5.Visible = false;

            /////////////////////////////////////////////////////////
            try
            {
                // Realizar la solicitud GET para obtener los datos más recientes
                string getDataUrl = "http://127.0.0.1:5000/data";

                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage getDataResponse = httpClient.GetAsync(getDataUrl).Result;

                    if (getDataResponse.IsSuccessStatusCode)
                    {
                        string responseData = getDataResponse.Content.ReadAsStringAsync().Result;
                        List<Dictionary<string, string>> dataList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(responseData);

                        // Crear DataTable y agregar columnas
                        DataTable dataTable = new DataTable();
                        if (dataList.Count > 0)
                        {
                            foreach (var key in dataList[0].Keys)
                            {
                                dataTable.Columns.Add(key);
                            }

                            // Agregar filas al DataTable
                            foreach (var data in dataList)
                            {
                                DataRow row = dataTable.NewRow();
                                foreach (var key in data.Keys)
                                {
                                    row[key] = data[key];
                                }
                                dataTable.Rows.Add(row);
                            }

                            // Actualizar el DataSource del DataGridView en el hilo de la interfaz de usuario
                            dataGridView1.Invoke((MethodInvoker)delegate
                            {
                                dataGridView1.DataSource = dataTable;
                            });

                            if (dataGridView1.Columns.Count > 0)
                            {
                                // Mover la columna "Nombre" al principio
                                if (dataGridView1.Columns.Contains("id"))
                                    dataGridView1.Columns["id"].DisplayIndex = 0;

                                // Mover la columna "Edad" después de la columna "Nombre"
                                if (dataGridView1.Columns.Contains("nombre"))
                                    dataGridView1.Columns["nombre"].DisplayIndex = 1;

                                // Mover la columna "ID" al final
                                if (dataGridView1.Columns.Contains("direccion"))
                                    dataGridView1.Columns["direccion"].DisplayIndex = 2;

                                if (dataGridView1.Columns.Contains("telefono"))
                                    dataGridView1.Columns["telefono"].DisplayIndex = 3;

                                if (dataGridView1.Columns.Contains("mail"))
                                    dataGridView1.Columns["mail"].DisplayIndex = 2;
                            }



                            // MessageBox.Show("Datos actualizados correctamente en el DataGridView.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron datos para actualizar.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los datos para actualizar. Código de estado: " + getDataResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los datos del DataGridView: " + ex.Message);
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Realizar la solicitud GET para obtener los datos más recientes
                string getDataUrl = "http://127.0.0.1:5000/data";

                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage getDataResponse = httpClient.GetAsync(getDataUrl).Result; // Síncrono

                    if (getDataResponse.IsSuccessStatusCode)
                    {
                        string responseData = getDataResponse.Content.ReadAsStringAsync().Result; // Síncrono
                        List<Dictionary<string, string>> dataList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(responseData);

                        // Extraer el último ID
                        if (dataList.Any())
                        {
                            string lastIdString = dataList.Last()["id"]; // Suponiendo que el ID es una cadena
                            if (int.TryParse(lastIdString, out int lastId))
                            {
                                // Sumarle 1 al último ID
                                int siguienteId = lastId + 1;

                                // Escribir el siguiente ID en textBox5
                                textBox5.Text = siguienteId.ToString();
                            }
                            else
                            {
                                // Manejar el caso en el que el último ID no sea un número válido
                                textBox5.Text = "1"; // Por ejemplo, podrías establecer el valor predeterminado a 1
                            }
                        }
                        else
                        {
                            // Manejar el caso en el que no haya datos en la lista
                            textBox5.Text = "1"; // Por ejemplo, podrías establecer el valor predeterminado a 1
                        }
                    }
                    else
                    {
                        // Manejar el caso en el que la solicitud no sea exitosa
                        textBox5.Text = "1"; // Por ejemplo, podrías establecer el valor predeterminado a 1
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante la solicitud
                Console.WriteLine("Error al obtener los datos: " + ex.Message);
            }
           
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox1.Focus();
            button1.Enabled = true;
            button2.Enabled = false;
        }

        
        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string nombreBuscado = textBox6.Text.Trim(); // Obtener el texto ingresado en textBox6 para la búsqueda por nombre

                // Realizar la solicitud GET para obtener los datos más recientes
                string getDataUrl = "http://127.0.0.1:5000/data";

                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage getDataResponse = httpClient.GetAsync(getDataUrl).Result;

                    if (getDataResponse.IsSuccessStatusCode)
                    {
                        string responseData = getDataResponse.Content.ReadAsStringAsync().Result;
                        List<Dictionary<string, string>> dataList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(responseData);

                        // Filtrar los datos por nombre en tiempo real mientras el usuario escribe
                        List<Dictionary<string, string>> resultadosBusqueda = dataList
                            .Where(d => d.ContainsKey("nombre") && d["nombre"].IndexOf(nombreBuscado, StringComparison.OrdinalIgnoreCase) >= 0)
                            .ToList();

                        // Crear DataTable y agregar columnas
                        DataTable dataTable = new DataTable();
                        if (resultadosBusqueda.Count > 0) // Usar los resultados de la búsqueda en lugar de los datos originales
                        {
                            foreach (var key in resultadosBusqueda[0].Keys)
                            {
                                dataTable.Columns.Add(key);
                            }

                            // Agregar filas al DataTable
                            foreach (var data in resultadosBusqueda)
                            {
                                DataRow row = dataTable.NewRow();
                                foreach (var key in data.Keys)
                                {
                                    row[key] = data[key];
                                }
                                dataTable.Rows.Add(row);
                            }

                            // Actualizar el DataSource del DataGridView en el hilo de la interfaz de usuario
                            dataGridView1.Invoke((MethodInvoker)delegate
                            {
                                dataGridView1.DataSource = dataTable;
                                ////////////////////////////////////////
                                if (dataGridView1.Columns.Count > 0)
                                {
                                    // Mover la columna "Nombre" al principio
                                    if (dataGridView1.Columns.Contains("id"))
                                        dataGridView1.Columns["id"].DisplayIndex = 0;

                                    // Mover la columna "Edad" después de la columna "Nombre"
                                    if (dataGridView1.Columns.Contains("nombre"))
                                        dataGridView1.Columns["nombre"].DisplayIndex = 1;

                                    // Mover la columna "ID" al final
                                    if (dataGridView1.Columns.Contains("direccion"))
                                        dataGridView1.Columns["direccion"].DisplayIndex = 2;

                                    if (dataGridView1.Columns.Contains("telefono"))
                                        dataGridView1.Columns["telefono"].DisplayIndex = 3;

                                    if (dataGridView1.Columns.Contains("mail"))
                                        dataGridView1.Columns["mail"].DisplayIndex = 2;
                                }

                            });

                            // Resto del código para ajustar las columnas del DataGridView...
                        }
                        else
                        {
                            dataGridView1.DataSource = null; // Limpiar el DataGridView si no se encuentran resultados
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los datos. Código de estado: " + getDataResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al procesar la búsqueda: " + ex.Message);
            }
            /////////////////////////////////////////////////////////////////////
            

            


        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                int idABorrar;
                if (!int.TryParse(textBox7.Text.Trim(), out idABorrar))
                {
                    MessageBox.Show("Ingrese un ID válido para el registro a borrar.");
                    return;
                }

                string urlBorrar = $"http://127.0.0.1:5000/data/{idABorrar}";

                WebRequest request = WebRequest.Create(urlBorrar);
                request.Method = "DELETE";

                WebResponse response = request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                MessageBox.Show($"Registro con ID {idABorrar} borrado exitosamente.");

                reader.Close();
                response.Close();

                // Actualizar el DataGridView u otra lógica necesaria después de borrar el registro...
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al borrar el registro: " + ex.Message);
            }
            ///////////////////////////////////////////////////////
            try
            {
                // Realizar la solicitud GET para obtener los datos más recientes
                string getDataUrl = "http://127.0.0.1:5000/data";

                using (var httpClient = new HttpClient())
                {
                    HttpResponseMessage getDataResponse = httpClient.GetAsync(getDataUrl).Result;

                    if (getDataResponse.IsSuccessStatusCode)
                    {
                        string responseData = getDataResponse.Content.ReadAsStringAsync().Result;
                        List<Dictionary<string, string>> dataList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(responseData);

                        // Crear DataTable y agregar columnas
                        DataTable dataTable = new DataTable();
                        if (dataList.Count > 0)
                        {
                            foreach (var key in dataList[0].Keys)
                            {
                                dataTable.Columns.Add(key);
                            }

                            // Agregar filas al DataTable
                            foreach (var data in dataList)
                            {
                                DataRow row = dataTable.NewRow();
                                foreach (var key in data.Keys)
                                {
                                    row[key] = data[key];
                                }
                                dataTable.Rows.Add(row);
                            }

                            // Actualizar el DataSource del DataGridView en el hilo de la interfaz de usuario
                            dataGridView1.Invoke((MethodInvoker)delegate
                            {
                                dataGridView1.DataSource = dataTable;
                            });

                            if (dataGridView1.Columns.Count > 0)
                            {
                                // Mover la columna "Nombre" al principio
                                if (dataGridView1.Columns.Contains("id"))
                                    dataGridView1.Columns["id"].DisplayIndex = 0;

                                // Mover la columna "Edad" después de la columna "Nombre"
                                if (dataGridView1.Columns.Contains("nombre"))
                                    dataGridView1.Columns["nombre"].DisplayIndex = 1;

                                // Mover la columna "ID" al final
                                if (dataGridView1.Columns.Contains("direccion"))
                                    dataGridView1.Columns["direccion"].DisplayIndex = 2;

                                if (dataGridView1.Columns.Contains("telefono"))
                                    dataGridView1.Columns["telefono"].DisplayIndex = 3;

                                if (dataGridView1.Columns.Contains("mail"))
                                    dataGridView1.Columns["mail"].DisplayIndex = 2;
                            }



                            // MessageBox.Show("Datos actualizados correctamente en el DataGridView.");
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron datos para actualizar.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error al obtener los datos para actualizar. Código de estado: " + getDataResponse.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar los datos del DataGridView: " + ex.Message);
            }
            textBox7.Text = "";

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        




    }
    }

       
  

