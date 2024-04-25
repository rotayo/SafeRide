using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ProyectoFinal.ViewModel.administrador
{
    class VerIncidenteAdministradorViewModel
    {
        public string HabilitarChat {  get; set; }
        public IncidenteModel Incidente { get; set; }
        private Xamarin.Forms.Picker picker { get; set; }
        private IncidenteConexion incidenteConexion { get; set; }
        public Command Back { get; set; }
        public Command Profile { get; set; }
        public Command Actualizar { get; set; }
        public Command IniciarChat {  get; set; }
        private INavigation navigation { get; set; }
        public VerIncidenteAdministradorViewModel(int id, INavigation navigation, Xamarin.Forms.Picker picker)
        {

            this.navigation = navigation;
            this.picker = picker; 
            incidenteConexion = new IncidenteConexion();
            Incidente = SesionActualModel.Instancia().ObtenerIncidencia(id);

            HabilitarChat = (Incidente.estado == "Procesando") ? "habilitado" : "desabilitado";

            picker.SelectedItem = "";

            if (string.IsNullOrEmpty(Incidente.id_agente))
            {
                picker.ItemsSource = obtenerAgentes();
            }
            else
            {
                var usuario = SesionActualModel.Instancia().ObtenerUsuario(Incidente.id_agente);
                picker.Title = usuario.id + " - " + usuario.nombre + " " + usuario.apellido;
                picker.IsEnabled = false;
            }

            Back = new Command(() => { 
                navigation.PopAsync();
                picker.SelectedItem = "";
            });
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));
            Actualizar = new Command(async () => await ActualizarCommand());
            IniciarChat = new Command(async () => await navigation.PushAsync(new Chat(Incidente.id)));
        }

        private List<string> obtenerAgentes() {
            List<string> listaAgentes = new List<string>();
            foreach (var usuario in SesionActualModel.Instancia().ListaUsuarios)
            {
                if(usuario.id_rol == 2)
                {
                    listaAgentes.Add(usuario.id + " - " + usuario.nombre + " " + usuario.apellido);
                }
            }
            return listaAgentes;
        }

        private async Task ActualizarCommand()
        {
            try {
                if (string.IsNullOrEmpty(picker.SelectedItem.ToString()) || Incidente.id_agente != null)
                {
                    await UserDialogs.Instance.AlertAsync("No se ha asignado ningún Agente o el incidente ya esta siendo revisado por un Agente", "No se puede Actualizar", "Ok");
                    return;
                }

                //Toma el elemento seleccionado en el combo box y
                //se extrae el id, que es la primera palabra del string osea del indice [0]
                //separando el string en un array con las palabras individuales por espacios vacios "_"
                Incidente.id_agente = picker.SelectedItem.ToString().Split(' ')[0];
                Incidente.estado = "Asignado";
                HttpResponseMessage respuesta = await incidenteConexion.Actualizar(Incidente);
                if (respuesta.IsSuccessStatusCode)
                {
                    navigation.InsertPageBefore(new MainPageAdministrador(), navigation.NavigationStack[0]);
                    await navigation.PopToRootAsync();
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(await respuesta.Content.ReadAsStringAsync(),
                        "Error", "Ok");
                }
            }
            catch (Exception ex) { 
                 Console.WriteLine(ex.StackTrace);
                 Console.WriteLine(ex.Message);
            }
        }
    }
}
