using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel
{
    public class EditarPerfilPageViewModel
    {
        public UsuarioModel Usuario { get; set; }
        public Command Back { get; set; }
        public Command Actualizar { get; set; }
        private UsuarioConexion usuarioConexion {  get; set; }
        private INavigation navigation { get; set; }

        public EditarPerfilPageViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            usuarioConexion = new UsuarioConexion();
            Usuario = SesionActualModel.Instancia().Usuario;

            Back = new Command(() => navigation.PopAsync());
            Actualizar = new Command(async () => await ActualizarCommand());
        }

        private async Task ActualizarCommand()
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

            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                if (Usuario.email == SesionActualModel.Instancia().Usuario.email)
                {
                    await usuarioConexion.Actualizar(Usuario);
                    await UserDialogs.Instance.AlertAsync("Datos Actualizados con Exito", "Confirmacion", "Ok");
                    SesionActualModel.Instancia().Usuario = Usuario;
                }
                else
                {
                    HttpResponseMessage respuesta = await usuarioConexion.Verificar("_", Usuario.email);
                    if (respuesta.IsSuccessStatusCode)
                    {
                        await usuarioConexion.Actualizar(Usuario);
                        await UserDialogs.Instance.AlertAsync("Datos Actualizados con Exito", "Confirmacion", "Ok");
                        SesionActualModel.Instancia().Usuario = Usuario;
                    }
                    else
                    {
                        await UserDialogs.Instance.AlertAsync(await respuesta.Content.ReadAsStringAsync(), "Email no valido", "Ok");
                    }
                }

                switch (Usuario.id_rol)
                {
                    case 1:
                        navigation.InsertPageBefore(new MainPageUsuario(), navigation.NavigationStack[0]);
                        break;
                    case 3:
                        navigation.InsertPageBefore(new MainPageAdministrador(), navigation.NavigationStack[0]);
                        break;
                }
                await navigation.PopToRootAsync();
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
