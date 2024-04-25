using Acr.UserDialogs;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ProyectoFinal.ViewModel.usuario
{
    class MainPageUsuarioViewModel
    {
        public string DatosAuto{ get; set; }
        public string Placa { get; set; }
        private CargaDeMapa mapa { get; set; }
        public Command CambiarPagina { get; set; }
        public INavigation navigation { get; set; }

        public MainPageUsuarioViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            mapa = new CargaDeMapa();

            DatosAuto = $"{SesionActualModel.Instancia().Auto.marca} " +
                $"{SesionActualModel.Instancia().Auto.modelo}";

            Placa = $"Placa: {SesionActualModel.Instancia().Auto.id}";

            CambiarPagina = new Command<string>(async destino =>
            {
                switch (destino)
                {
                    case "reportar":
                        await mapa.CargarMapaSesionActual();
                        await navigation.PushAsync(new ReporteIncidenteUsuario());
                        break;
                    case "historial":
                        await navigation.PushAsync(new HistorialUsuario());
                        break;
                    case "perfil":
                        await navigation.PushAsync(new PerfilPage());
                        break;
                    case "auto":
                        await navigation.PushAsync(new EditarAutoPage());
                        break;
                }
            });
        }
    }
}
