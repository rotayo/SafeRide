using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoFinal.Conexion;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.agente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.agente
{
    public class VerIncidenteAgenteViewModel
    {
        public IncidenteModel Incidente { get; set; }
        private IncidenteConexion incidenteConexion { get; set; }
        private UsuarioConexion UsuarioConexion { get; set; }
        private AutoConexion AutoConexion { get; set; }
        private UsuarioModel Usuario {  get; set; }
        private AutoModel Auto { get; set; }
        public Command Back { get; set; }
        public Command Profile { get; set; }
        public Command PaginaUsuario { get; set; }
        public Command IniciarProceso { get; set; }
        public INavigation navigation { get; set; }

        public VerIncidenteAgenteViewModel(INavigation navigation, int id)
        {
            UsuarioConexion = new UsuarioConexion();
            AutoConexion = new AutoConexion();
            this.navigation = navigation;
            Incidente = SesionActualModel.Instancia().ObtenerIncidencia(id);
            PaginaUsuario = new Command<string>(async (id_usuario) => {
                await CargarDatos(id_usuario);
                await navigation.PushAsync(new VerUsuarioAgente(Usuario, Auto));
            });
            Back = new Command(() => navigation.PopAsync());
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));
            IniciarProceso = new Command(async () => await IniciarProcesoCommand(id));
        }

        private async Task IniciarProcesoCommand(int id)
        {
            var cdm = new CargaDeMapa();
            cdm.CargaMapaPorIdSesionActual(id);
            incidenteConexion = new IncidenteConexion();
            Incidente.estado = "Procesando";
            await incidenteConexion.Actualizar(Incidente);
            await navigation.PushAsync(new ProcesoIncidenteAgente(id));
        }

        public async Task CargarDatos(string id)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando Datos");
                HttpResponseMessage responseUsuario = await UsuarioConexion.ObtenerPorID(id);
                if (responseUsuario.IsSuccessStatusCode)
                {
                    var UsuarioJson = await responseUsuario.Content.ReadAsStringAsync();
                    Usuario = JsonConvert.DeserializeObject<UsuarioModel>(UsuarioJson);
                    if (string.IsNullOrEmpty(Usuario.id_automovil))
                    {
                        return;
                    }

                    var AutoJson = await AutoConexion.Obtener(Usuario.id_automovil);
                    Auto = JsonConvert.DeserializeObject<AutoModel>(AutoJson);
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert(ex.StackTrace, "Error", "Salir");
            }
        }
    }
}
