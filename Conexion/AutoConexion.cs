using Newtonsoft.Json;
using ProyectoFinal.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Conexion
{
    public class AutoConexion
    {
        //Ver Usuario Conexion, funciona al igual que AutoConexion

        private HttpClient cliente;

        public AutoConexion()
        {
            cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://saferide-api-mocz.onrender.com/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> Register(AutoModel auto)
        {
            var json = JsonConvert.SerializeObject(auto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await cliente.PostAsync("autos", data);
            return response;
        }

        public async Task<HttpResponseMessage> Actualizar(AutoModel auto)
        {
            var json = JsonConvert.SerializeObject(auto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await cliente.PutAsync("autos", data);
            return response;
        }

        public async Task<HttpResponseMessage> Verificar(string id, string poliza)
        {
            var response = await cliente.GetAsync($"autos/verificar/{id}/{poliza}");
            return response;
        }

        public async Task<string> Obtener(string id)
        {
            var response = await cliente.GetAsync($"autos/obtener/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }
        }
    }
}
