using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinal.Datos
{
    internal class CargaDeListas
    {
        UsuarioConexion usuarioConexion { get; set; }
        IncidenteConexion incidenteConexion { get; set; }

        public CargaDeListas()
        {
            usuarioConexion = new UsuarioConexion();
            incidenteConexion = new IncidenteConexion();
        }
        public async Task CargarListaUsuariosSesionActual()
        {
            var respuesta = await usuarioConexion.ObtenerLista();
            if (respuesta.IsSuccessStatusCode)
            {
                var listaUsuariosJson = await respuesta.Content.ReadAsStringAsync();
                List<UsuarioModel> lista = JsonConvert.DeserializeObject<List<UsuarioModel>>(listaUsuariosJson);
                SesionActualModel.Instancia().ListaUsuarios = new List<UsuarioModel>();
                foreach (UsuarioModel usuario in lista)
                {
                    if(usuario.id_rol != 3)
                        SesionActualModel.Instancia().ListaUsuarios.Add(usuario);
                }
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Error", "No se pudo obtener la lista de usuarios", "OK");
                return;
            }
        }

        public async Task CargarIncidentesSesionActual()
        {
            var respuesta = await incidenteConexion.ObtenerLista();
            if (respuesta.IsSuccessStatusCode)
            {
                var listaIncidentesJson = await respuesta.Content.ReadAsStringAsync();
                SesionActualModel.Instancia().ListaIncidentes = JsonConvert.DeserializeObject<List<IncidenteModel>>(listaIncidentesJson);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Error", "No se pudo obtener la lista de incidentes", "OK");
            }
        }

        public async Task CargarIncidentesSesionActual(string id, int rol)
        {
            HttpResponseMessage respuesta = null;
            if (rol == 1)
            {
                respuesta = await incidenteConexion.ObtenerListaUsuario(id);
            } else if(rol == 2)
            {
                respuesta = await incidenteConexion.ObtenerListaAgente(id);
            }

            if (respuesta.IsSuccessStatusCode)
            {
                var listaIncidentesJson = await respuesta.Content.ReadAsStringAsync();
                SesionActualModel.Instancia().ListaIncidentes = JsonConvert.DeserializeObject<List<IncidenteModel>>(listaIncidentesJson);
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Error", "No se pudo obtener la lista de incidentes", "OK");
            }
        }

        public async Task CargarTodo()
        {
            await CargarListaUsuariosSesionActual();
            await CargarIncidentesSesionActual();
        }
    }
}
