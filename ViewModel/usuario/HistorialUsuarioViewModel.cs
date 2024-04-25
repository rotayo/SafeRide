using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using ProyectoFinal.View.usuario;
using ProyectoFinal.ViewModel.administrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.usuario
{
    public class PlantillaIncidenteUsuario
    {
        public string id { get; set; }
        public string id_lbl { get; set; }
        public string id_agente { get; set; }
        public string fechaHora { get; set; }
        public Command PaginaIncidente { get; set; }
        public PlantillaIncidenteUsuario(INavigation navigation)
        {
            try
            {
                PaginaIncidente = new Command<string>(async (id) => await navigation.PushAsync(new VerIncidenteUsuario(int.Parse(id))));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }
    }
    public class HistorialUsuarioViewModel
    {
        public IncidenteConexion incidenteConexion { get; set; }
        public Command CambiarPagina { get; set; }
        private Stack<PlantillaIncidenteUsuario> listaIncidentes { get; set; }
        private OrdenamientoDeListas Ordenamiento { get; set; }
        public Picker PickerFiltros {  get; set; }
        public ListView listView { get; set; }
        private INavigation navigation { get; set; }

        public HistorialUsuarioViewModel(ListView ListView, INavigation navigation, Picker picker)
        {
            this.listView = ListView;
            this.navigation = navigation;
            incidenteConexion = new IncidenteConexion();
            listaIncidentes = new Stack<PlantillaIncidenteUsuario>();
            Ordenamiento = new OrdenamientoDeListas();

            PickerFiltros = picker;

            PickerFiltros.ItemsSource = new string[2] { "Por Antiguedad", "Más Nuevo" };

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
        public void CargarIncidentes()
        {
            try
            {
                listaIncidentes.Clear();
                foreach (var Incidente in SesionActualModel.Instancia().ListaIncidentes)
                {
                    var NuevoIncidente = new PlantillaIncidenteUsuario(navigation)
                    {
                        id = ""+Incidente.id,
                        id_lbl = $"Código del caso: #{Incidente.id}",
                        id_agente= $"ID del Agente del caso: #{Incidente.id_agente}",
                        fechaHora = $"Hora: {Incidente.hora} Fecha: {Incidente.fecha}"
                    };
                    listaIncidentes.Push(NuevoIncidente);
                }
                listView.ItemsSource = listaIncidentes;
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.AlertAsync(ex.Message + ex.StackTrace, "Error", "OK");
            }
        }

        public Stack<PlantillaIncidenteUsuario> buscarPorId(string id)
        {
            Stack<PlantillaIncidenteUsuario> nuevaLista = new Stack<PlantillaIncidenteUsuario>();

            foreach (var incidente in listaIncidentes)
            {
                if (incidente.id == id)
                {
                    nuevaLista.Push(incidente);
                }
            }
            return nuevaLista;
        }

        public void OrdenarPorAntiguedad()
        {
            var listaOrdenada = Ordenamiento.OrdernarMasAntiguoIncidenteUsuario(listaIncidentes.ToList());
            listaIncidentes.Clear();
            foreach (var incidente in listaOrdenada)
            {
                listaIncidentes.Push(incidente);
            }
            ActualizarLista();
        }

        public void OrdenarPorNuevo()
        {
            var listaOrdenada = Ordenamiento.OrdernarMasNuevoIncidenteUsuario(listaIncidentes.ToList());
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

