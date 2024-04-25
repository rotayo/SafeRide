using Acr.UserDialogs;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.SignalR;
using ProyectoFinal.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static System.Collections.Specialized.BitVector32;

namespace ProyectoFinal.ViewModel
{
    public class PlantillaMensaje
    {
        public string Color {  get; set; }
        public string Nombre { get; set; }
        public string Texto { get; set; }

    }
    public class ChatViewModel : INotifyPropertyChanged
    {
        public int IncidenteID { get; set; }
        public ListView listView { get; set; }
        public List<PlantillaMensaje> Mensajes { get; set; }
        public Command EnviarMensaje { get; set; }
        public Command Back { get; set; }
        public Command Profile { get; set; }
        private MetodosSignal signalrRecibirMensajes {  get; set; }
        private string _MensajeAEnviar { get; set; }
        private INavigation navigation { get; set; }

        public string MensajeAEnviar
        {
            get => _MensajeAEnviar;
            set
            {
                _MensajeAEnviar = value;
                OnPropertyChanged();
            }
        }
        public ChatViewModel(INavigation navigation, int idIncidente, ListView listview)
        {
            this.listView = listview;
            this.navigation = navigation;
            IncidenteID = idIncidente;
            Mensajes = new List<PlantillaMensaje>();

            EnviarMensaje = new Command(EnviarMensajeCommand);
            Back = new Command(() => {
                signalrRecibirMensajes.CerrarConexion();
                this.navigation.PopAsync();
            });
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));

            RecibirMensajesTiempoReal();
        }

        private void RecibirMensajesTiempoReal()
        {
            var sesion = SesionActualModel.Instancia();
            signalrRecibirMensajes = sesion.CrearConexionSignalR();
            signalrRecibirMensajes.RecibirMensajes((mensaje) =>
            {
                var Elementos = mensaje.Split(',');

                if (Elementos[0] == ""+IncidenteID && Elementos[1] != sesion.Usuario.id) {

                    PlantillaMensaje NuevoMensaje = new PlantillaMensaje
                    {
                        Color = "azul",
                        Nombre = Elementos[2],
                        Texto = Elementos[3]
                    };
                    Mensajes.Add(NuevoMensaje);
                    Device.BeginInvokeOnMainThread(async () => {
                        listView.ItemsSource = null;
                        listView.ItemsSource = Mensajes;
                    });
                }
            });
        }

        private void EnviarMensajeCommand()
        {
            if (!string.IsNullOrEmpty(MensajeAEnviar))
            {
                var sesion = SesionActualModel.Instancia();

                PlantillaMensaje NuevoMensaje = new PlantillaMensaje
                {
                    Color = "blanco",
                    Nombre = sesion.Usuario.nombre,
                    Texto = MensajeAEnviar
                };
                Mensajes.Add(NuevoMensaje);

                var MensajeConDestinatario = $"{IncidenteID},{sesion.Usuario.id},{sesion.Usuario.nombre},{MensajeAEnviar}";
                MensajeAEnviar = "";
                sesion.CrearConexionSignalR().EnviarMensaje(MensajeConDestinatario);

                Device.BeginInvokeOnMainThread(async () => {
                    listView.ItemsSource = null;
                    listView.ItemsSource = Mensajes;
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
