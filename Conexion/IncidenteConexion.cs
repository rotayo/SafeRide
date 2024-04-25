using Newtonsoft.Json;
using ProyectoFinal.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Conexion
{
    public class IncidenteConexion
    {
        //Ver Usuario Conexion, funciona al igual que IncidenteConexion

        private HttpClient cliente;

        public IncidenteConexion()
        {
            cliente = new HttpClient();
            cliente.BaseAddress = new Uri("https://saferide-api-mocz.onrender.com/");
            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> Register(IncidenteModel incidente)
        {
            var json = JsonConvert.SerializeObject(incidente);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await cliente.PostAsync("incidentes", data);
            return response;
        }

        public async Task<HttpResponseMessage> ObtenerLista()
        {
            var response = await cliente.GetAsync("incidentes");
            return response;
        }

        public async Task<HttpResponseMessage> Obtener(int id)
        {
            var response = await cliente.GetAsync($"incidentes/obtener/{id}");
            return response;
        }

        public async Task<HttpResponseMessage> ObtenerListaAgente(string id)
        {
            var response = await cliente.GetAsync($"incidentes/obtener/agente/{id}");
            return response;
        }

        public async Task<HttpResponseMessage> ObtenerListaUsuario(string id)
        {
            var response = await cliente.GetAsync($"incidentes/obtener/usuario/{id}");
            return response;
        }

        public async Task<HttpResponseMessage> Actualizar(IncidenteModel incidente)
        {
            var json = JsonConvert.SerializeObject(incidente);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await cliente.PutAsync("incidentes", data);
            return response;
        }
    }
}
