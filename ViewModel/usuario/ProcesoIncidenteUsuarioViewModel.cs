using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoFinal.Conexion;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.View.administrador;
using ProyectoFinal.View.usuario;
using ProyectoFinal.ViewModel.administrador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.usuario
{
    public class ProcesoIncidenteUsuarioViewModel : INotifyPropertyChanged
    {
        private string _nombreAgente {  get; set; }
        public IncidenteModel Incidente { get; set; }
        public UsuarioModel Usuario {  get; set; }
        private IncidenteConexion incidenteConexion { get; set; }
        private UsuarioConexion usuarioConexion { get; set; }
        private INavigation navigation { get; set; }

        public string NombreAgente
        {
            get => _nombreAgente;
            set
            {
                _nombreAgente = value;
                OnPropertyChanged();
            }
        }

        public ProcesoIncidenteUsuarioViewModel(INavigation navigation, IncidenteModel Incidente, UsuarioModel Agente = null)
        {
            incidenteConexion = new IncidenteConexion();
            usuarioConexion = new UsuarioConexion();
            this.Incidente = Incidente;
            this.navigation = navigation;
            Usuario = Agente;
            NombreAgente = (Usuario == null) ? "Se está asignando un Agente a su caso" : $"Agente: {Usuario.id} {Usuario.nombre} {Usuario.apellido}";

            IniciarConexionTiempoReal();
        }
        private void IniciarConexionTiempoReal()
        {
            var sesion = SesionActualModel.Instancia();

            sesion.CrearConexionSignalR().AbrirConexionSignalR("AgenteAsignado", async () =>
            {
                var respuestaIncidente = await incidenteConexion.Obtener(Incidente.id);
                var IncidenteActualizadoJson = await respuestaIncidente.Content.ReadAsStringAsync();
                Incidente = JsonConvert.DeserializeObject<IncidenteModel>(IncidenteActualizadoJson);

                var respuestaUsuario = await usuarioConexion.ObtenerPorID(Incidente.id_agente);
                var AgenteAsignadoJson = await respuestaUsuario.Content.ReadAsStringAsync();
                Usuario = JsonConvert.DeserializeObject<UsuarioModel>(AgenteAsignadoJson);

                NombreAgente = $"Agente: {Usuario.id} {Usuario.nombre} {Usuario.apellido}";
            });

            sesion.CrearConexionSignalR().AbrirConexionSignalR("IncidenteFinalizado", async () =>
            {
                Device.BeginInvokeOnMainThread(async () => { 
                    UserDialogs.Instance.Alert("Muchas gracias, el proceso ha sido finalizado");
                    sesion.CerrarConexionesSignalR();
                    navigation.InsertPageBefore(new MainPageUsuario(), navigation.NavigationStack[0]);
                    await navigation.PopToRootAsync();
                });
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
