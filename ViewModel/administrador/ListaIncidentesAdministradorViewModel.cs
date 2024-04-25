using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoFinal.Conexion;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.administrador
{
    public class PlantillaIncidenteAdministrador
    {
        public string id {  get; set; }
        public string id_lbl { get; set; }
        public string id_agente { get; set; }
        public string estado { get; set; }
        public string fechaHora { get; set; }
        public Command PaginaIncidente { get; set; }
        public PlantillaIncidenteAdministrador (INavigation navigation)
        {
            try
            {
                PaginaIncidente = new Command<string>(async (id) => await navigation.PushAsync(new VerIncidenteAdministrador(int.Parse(id))));
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            
        }
    }
    public class ListaIncidentesAdministradorViewModel
    {
        public IncidenteConexion incidenteConexion { get; set; }
        public Command CambiarPagina { get; set; }
        public Picker PickerFiltros {  get; set; }
        private Stack<PlantillaIncidenteAdministrador> listaIncidentes { get; set; }
        private OrdenamientoDeListas Ordenamiento { get; set; }
        public ListView listView { get; set; }
        private INavigation navigation { get; set; }

        public ListaIncidentesAdministradorViewModel(ListView ListView, INavigation navigation, Picker picker)
        {
            this.listView = ListView;
            this.navigation = navigation;
            PickerFiltros = picker;
            Ordenamiento = new OrdenamientoDeListas();

            PickerFiltros.ItemsSource = new string[2] {"Por Antiguedad", "Más Nuevo"}; 

            incidenteConexion = new IncidenteConexion();
            listaIncidentes = new Stack<PlantillaIncidenteAdministrador>();

            CambiarPagina = new Command<string>(async destino =>
            {
                switch (destino)
                {
                    case "back":
                        await this.navigation.PopAsync();
                        break;
                    case "perfil":
                        await this.navigation.PushAsync(new PerfilPage());
                        break;
                }
            });
        }
        public async Task cargarIncidentes()
        {
            try
            {
                listaIncidentes.Clear();

                foreach (var Incidente in SesionActualModel.Instancia().ListaIncidentes)
                {
                    string agente = (Incidente.id_agente == null) ? "Ningún agente asignado" : ""+Incidente.id_agente;

                    PlantillaIncidenteAdministrador IncidenteNuevo = new PlantillaIncidenteAdministrador(navigation)
                    {
                        id = ""+Incidente.id,
                        id_lbl = "N. Incidente: " + Incidente.id,
                        id_agente = "Agente: "+agente,
                        estado = Incidente.estado,
                        fechaHora = "Fecha: " + Incidente.fecha + " Hora: " + Incidente.hora.ToString()
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

        public Stack<PlantillaIncidenteAdministrador> buscarPorId(string id)
        {
            var nuevaLista = new Stack<PlantillaIncidenteAdministrador>();

            foreach (var incidente in listaIncidentes)
            {
                if (incidente.id == id)
                {
                    nuevaLista.Push(incidente);
                }
            }
            return nuevaLista;
        }

        public List<PlantillaIncidenteAdministrador> ObtenerListaFiltrada()
        {
            var listaFiltrada = new List<PlantillaIncidenteAdministrador>();
            foreach (var incidente in listaIncidentes)
            {
                listaFiltrada.Add(incidente);
            }
            return listaFiltrada;
        }

        public void OrdenarPorAntiguedad()
        {
            var listaOrdenada = Ordenamiento.OrdernarMasAntiguoIncidente(listaIncidentes.ToList());
            listaIncidentes.Clear();
            foreach (var incidente in listaOrdenada)
            {
                listaIncidentes.Push(incidente);
            }
            ActualizarLista();
        }

        public void OrdenarPorNuevo()
        {
            var listaOrdenada = Ordenamiento.OrdernarMasNuevoIncidente(listaIncidentes.ToList());
            listaIncidentes.Clear();
            foreach (var incidente in listaOrdenada)
            {
                listaIncidentes.Push(incidente);
            }
            ActualizarLista();
        }

        private void ActualizarLista()
        {
            listView.ItemsSource = null;
            listView.ItemsSource = listaIncidentes;
        }
    }
}
