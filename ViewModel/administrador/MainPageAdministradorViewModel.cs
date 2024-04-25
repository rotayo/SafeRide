using Acr.UserDialogs;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using ProyectoFinal.Conexion;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ProyectoFinal.SignalR;
using static ProyectoFinal.SignalR.MetodosSignal;
using System.Runtime.CompilerServices;

namespace ProyectoFinal.ViewModel.administrador
{
    public class MainPageAdministradorViewModel
    {
        public delegate void Funcion();
        public Command CambiarPagina { get; set; }
        public INavigation navigation { get; set; }

        public MainPageAdministradorViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            IniciarConexionTiempoReal();
            CambiarPagina = new Command<string>(async destino =>
            {
                switch (destino)
                {
                    case "usuarios":
                        await navigation.PushAsync(new ListaUsuariosAdministrador());
                        break;
                    case "incidentes":
                        await navigation.PushAsync(new ListaIncidentesAdministrador());
                        break;
                    case "perfil":
                        await navigation.PushAsync(new PerfilPage());
                        break;
                }
            });
        }
        
        private void IniciarConexionTiempoReal()
        {
            var sesion = SesionActualModel.Instancia();
            CargaDeListas cdl = new CargaDeListas();

            sesion.CrearConexionSignalR().AbrirConexionSignalR("UsuarioActualizado", async () =>
            {
                await cdl.CargarListaUsuariosSesionActual();
                var currentPage = (App.Current.MainPage as NavigationPage)?.CurrentPage;
                if (currentPage is VerUsuarioAdministrador) {
                    Device.BeginInvokeOnMainThread(async () => {
                        await navigation.PopToRootAsync();
                    });
                }else if (currentPage is ListaUsuariosAdministrador)
                {
                    var viewModel = (currentPage.BindingContext as ListaUsuariosAdministradorViewModel);
                    if (viewModel != null)
                    {
                        Device.BeginInvokeOnMainThread(async () => {
                            viewModel.listView.ItemsSource = null;
                            await viewModel.cargarUsuarios();
                        });
                    }
                }
                UserDialogs.Instance.Alert("Se ha agregado un nuevo usuario o se ha actualizado un registro");
            });

            sesion.CrearConexionSignalR().AbrirConexionSignalR("IncidenteActualizado", async () =>
            {
                await cdl.CargarIncidentesSesionActual();
                var currentPage = (App.Current.MainPage as NavigationPage)?.CurrentPage;
                if (currentPage is VerIncidenteAdministrador)
                {
                    Device.BeginInvokeOnMainThread(async () => {
                        await navigation.PopToRootAsync();
                    });
                }
                else if (currentPage is ListaIncidentesAdministrador)
                {
                    var viewModel = (currentPage.BindingContext as ListaIncidentesAdministradorViewModel);
                    if (viewModel != null)
                    {
                        Device.BeginInvokeOnMainThread(async () => {
                            viewModel.listView.ItemsSource = null;
                            await viewModel.cargarIncidentes();
                        });
                    }
                }
                UserDialogs.Instance.Alert("Se ha agregado un nuevo incidente o se ha actualizado un registro");
            });
        }
    }
}
