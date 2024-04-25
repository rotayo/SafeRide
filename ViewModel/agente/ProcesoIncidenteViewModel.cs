using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.agente;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.agente
{
    public class ProcesoIncidenteViewModel : INotifyPropertyChanged
    {
        private string _tituloBoton { get; set; }
        public IncidenteModel Incidente { get; set; }
        public IncidenteConexion incidenteConexion { get; set; }
        public CheckBox validarCaso {  get; set; }
        public CheckBox LlegadaUbicacion { get; set; }
        public Command Next { get; set; }
        public Command Profile { get; set; }
        public Command IniciarChat { get; set; }

        private INavigation navigation { get; set; }

        public string TituloBoton
        {
            get => _tituloBoton;
            set
            {
                _tituloBoton = value;
                OnPropertyChanged();
            }
        }
        public ProcesoIncidenteViewModel(INavigation navigation, CheckBox checkbox1, CheckBox checkbox2,
            int id_incidente)
        {
            Incidente = SesionActualModel.Instancia().ObtenerIncidencia(id_incidente);
            incidenteConexion = new IncidenteConexion();
            TituloBoton = "Llegada";
            LlegadaUbicacion = checkbox1;
            validarCaso = checkbox2;
            this.navigation = navigation;

            Next = new Command(async () => await NextCommand());
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));
            IniciarChat = new Command(async() => {
                UserDialogs.Instance.ShowLoading("Cargando Chat");
                await navigation.PushAsync(new Chat(Incidente.id));
                UserDialogs.Instance.HideLoading();
            });
        }

        private async Task NextCommand()
        {
            if (!LlegadaUbicacion.IsChecked)
            {
                LlegadaUbicacion.IsChecked = true;
                TituloBoton = "Datos Validados";
                return;
            }

            if (!validarCaso.IsChecked)
            {
                validarCaso.IsChecked = true;
                TituloBoton = "Finalizar Proceso";
                return;
            }

            Incidente.estado = "Finalizado";
            SesionActualModel.Instancia().BorrarIncidente(Incidente.id);
            SesionActualModel.Instancia().CerrarConexionesSignalR();
            await incidenteConexion.Actualizar(Incidente);
            navigation.InsertPageBefore(new MainPageAgente(), navigation.NavigationStack[0]);
            await navigation.PopToRootAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
