using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.usuario
{
    internal class ReporteIncidentesDosUsuarioViewModel
    {
        public IncidenteModel Incidente { get; set; }
        public Command Crear { get; set; }
        public Command Back { get; set; }
        public Command Profile {  get; set; }
        private INavigation navigation;
        private IncidenteConexion ConexionIncidente {get;set;} 
        public ReporteIncidentesDosUsuarioViewModel(INavigation navigation, IncidenteModel Incidente)
        {
            this.navigation = navigation;
            this.Incidente = Incidente;
            ConexionIncidente = new IncidenteConexion();
            Crear = new Command(async () => await CrearReporte());
            Back = new Command(()=>navigation.PopAsync());
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));
        }

        public async Task CrearReporte()
        {
            if (string.IsNullOrEmpty(Incidente.descripcion) ||
               string.IsNullOrEmpty(Incidente.id_afectado)
               )
            {
                await UserDialogs.Instance.AlertAsync("Uno o más campos están vacios", "Campos Vacios", "Ok");
                return;
            }

            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                Incidente.id_reportador = SesionActualModel.Instancia().Usuario.id;
                Incidente.ubicacion = MapaModel.Instancia().Location.Latitude+","+ MapaModel.Instancia().Location.Longitude;
                Incidente.estado = "Pendiente";
                HttpResponseMessage CrearReporte = await ConexionIncidente.Register(Incidente);
                if (CrearReporte.IsSuccessStatusCode)
                {
                    await navigation.PushAsync(new ProcesoIncidenteUsuario(Incidente, null));
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(await CrearReporte.Content.ReadAsStringAsync(), "Ventana de error", "Salir");
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
