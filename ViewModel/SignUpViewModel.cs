using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel
{
    internal class SignUpViewModel
    {
        //Objetos para binding
        UsuarioConexion usuarioConexion { get; set; }
        public UsuarioModel Usuario { get; set; }
        public Command Next { get; set; }
        public Command Back { get; set; }
        private INavigation navigation { get; set; }

        //Constructor, se envia navegation como parametro para hacer la navegacion de paginas
        //Se utilizan metodos async debido a la respuesta de la api

        public SignUpViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            Usuario = new UsuarioModel();
            usuarioConexion = new UsuarioConexion();
            Next = new Command(async ()=> await VerificarUsuario());
            Back = new Command(async ()=> await navigation.PopAsync());
        }

        private async Task VerificarUsuario()
        {
            if (string.IsNullOrEmpty(Usuario.id) ||
                string.IsNullOrEmpty(Usuario.nombre) ||
                string.IsNullOrEmpty(Usuario.apellido) ||
                string.IsNullOrEmpty(Usuario.email) ||
                string.IsNullOrEmpty(Usuario.contrasena)
                ) {
                await UserDialogs.Instance.AlertAsync("Uno o más campos están vacios", "Campos Vacios", "Ok");
                return;
            }

            try
            {   
                UserDialogs.Instance.ShowLoading("Cargando");
                HttpResponseMessage respuesta = await usuarioConexion.Verificar(Usuario.id, Usuario.email.ToLower());
                if (respuesta.IsSuccessStatusCode) {
                    await navigation.PushAsync(new RegistrarAuto(Usuario));
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(await respuesta.Content.ReadAsStringAsync(), "Creedenciales no validas", "Ok");
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
