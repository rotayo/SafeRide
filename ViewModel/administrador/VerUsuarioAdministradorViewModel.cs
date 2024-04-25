using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.administrador
{
    public class VerUsuarioAdministradorViewModel
    {
        public bool RolCheck { get; set; }
        public UsuarioModel Usuario { get; set; }
        public AutoModel Auto { get; set; }
        public UsuarioConexion usuarioConexion { get; set; }
        public Command Actualizar { get; set; }
        public Command Back { get; set; }
        public Command Profile { get; set; }
        private INavigation navigation { get; set; }
        public VerUsuarioAdministradorViewModel(string id, INavigation navigation, AutoModel auto, 
                                                CheckBox desactivar, CheckBox agente)
        {
            this.navigation = navigation;
            Usuario = SesionActualModel.Instancia().ObtenerUsuario(id);
            usuarioConexion = new UsuarioConexion();

            Auto = (auto == null) ? new AutoModel { id = "Ningún Auto Asignado" } : auto;

            RolCheck = (Usuario.id_rol == 1) ? true : false;

            agente.IsChecked = (Usuario.id_rol == 2) ? true : false;

            desactivar.IsChecked = (Usuario.estado == false) ? true : false;

            Back = new Command(() => navigation.PopAsync());
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));
            Actualizar = new Command(async () => await ActualizarCommand(id));
        }

        private async Task ActualizarCommand(string id)
        {
            if (string.IsNullOrEmpty(Usuario.nombre) ||
                string.IsNullOrEmpty(Usuario.apellido) ||
                string.IsNullOrEmpty(Usuario.email) ||
                string.IsNullOrEmpty(Usuario.contrasena) ||
                string.IsNullOrEmpty(Usuario.telefono)
                )
            {
                await UserDialogs.Instance.AlertAsync("Uno o más campos están vacios", "Campos Vacios", "Ok");
                return;
            }

            Usuario.id_rol = (RolCheck == true) ? 1 : 2;

            await ActualizarUsuario(id);
        }

        private async Task ActualizarUsuario(string id)
        {
            try
            {
                if (Usuario.email == SesionActualModel.Instancia().ObtenerUsuario(id).email)
                {
                    await usuarioConexion.Actualizar(Usuario);
                    SesionActualModel.Instancia().CerrarConexionesSignalR();
                    navigation.InsertPageBefore(new MainPageAdministrador(), navigation.NavigationStack[0]);
                    await navigation.PopToRootAsync();
                }
                else
                {
                    HttpResponseMessage respuestaUsuario = await usuarioConexion.Verificar("_", Usuario.email);

                    if (respuestaUsuario.IsSuccessStatusCode)
                    {
                        await usuarioConexion.Actualizar(Usuario);
                        SesionActualModel.Instancia().CerrarConexionesSignalR();
                        navigation.InsertPageBefore(new MainPageAdministrador(), navigation.NavigationStack[0]);
                        await navigation.PopToRootAsync();
                    }
                    else
                    {

                        await UserDialogs.Instance.AlertAsync(await respuestaUsuario.Content.ReadAsStringAsync(), 
                            "Creedenciales ya registradas", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
