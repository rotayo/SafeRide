using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using ProyectoFinal.View.agente;
using ProyectoFinal.ViewModel.administrador;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.agente
{
    public class PlantillaIncidenteAgente
    {
        public string id { get; set; }
        public string ubicacion { get; set; }
        public string id_lbl { get; set; }
        public string fechaHora { get; set; }
        public Command PaginaIncidente { get; set; }
        public PlantillaIncidenteAgente(INavigation navigation)
        {
            try
            {
                PaginaIncidente = new Command<string>(async (id) => await navigation.PushAsync(new VerIncidenteAgente(int.Parse(id))));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }
    }
    public class MainPageAgenteViewModel
    {
        public IncidenteConexion incidenteConexion { get; set; }
        public Command Profile { get; set; }
        private Stack<PlantillaIncidenteAgente> listaIncidentes { get; set; }
        public ListView listView { get; set; }
        private INavigation navigation { get; set; }

        public MainPageAgenteViewModel(INavigation navigation, ListView listView)
        {
            this.navigation = navigation;
            this.listView = listView;
            listaIncidentes = new Stack<PlantillaIncidenteAgente>();
            IniciarConexionTiempoReal();
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));
        }

        public async Task cargarIncidentes()
        {
            try
            {
                listaIncidentes.Clear();

                foreach (var Incidente in SesionActualModel.Instancia().ListaIncidentes)
                {

                    PlantillaIncidenteAgente IncidenteNuevo = new PlantillaIncidenteAgente(navigation)
                    {
                        id = "" + Incidente.id,
                        id_lbl = "N. Incidente: " + Incidente.id,
                        ubicacion = "Ubicación: " + Incidente.ubicacion,
                        fechaHora = "Fecha: " + Incidente.fecha + " Hora: " + Incidente.hora
                    };

                    listaIncidentes.Push(IncidenteNuevo);
                }

                listView.ItemsSource = listaIncidentes;
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync("Error", ex.Message, "OK");
            }
        }

        private void IniciarConexionTiempoReal()
        {
            var sesion = SesionActualModel.Instancia();
            CargaDeListas cdl = new CargaDeListas();

            sesion.CrearConexionSignalR().AbrirConexionSignalR("AgenteAsignado", async () =>
            {
                int tamano = sesion.ListaIncidentes.Count;
                await cdl.CargarIncidentesSesionActual(sesion.Usuario.id, sesion.Usuario.id_rol);
                if(tamano != sesion.ListaIncidentes.Count) {
                    Device.BeginInvokeOnMainThread(async () => {
                        listView.ItemsSource = null;
                        await cargarIncidentes();
                    });
                }
            });
        }

    }
}
