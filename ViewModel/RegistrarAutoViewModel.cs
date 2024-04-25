using Acr.UserDialogs;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel
{
    internal class RegistrarAutoViewModel
    {
        private UsuarioConexion ConexionUsuario { get; set; }
        private AutoConexion ConexionAuto { get; set; }
        public UsuarioModel Usuario { get; set; }
        public AutoModel Auto { get; set; }
        private INavigation navigation { get; set; }
        public Command Registrarse { get; set; }
        public Command Back {  get; set; }

        public RegistrarAutoViewModel(UsuarioModel usuario, INavigation navigation)
        {
            this.Usuario = usuario;
            this.navigation = navigation;
            ConexionAuto = new AutoConexion();
            ConexionUsuario = new UsuarioConexion();
            Auto = new AutoModel();
            Registrarse = new Command(async () => await CrearUsuario());
            Back = new Command(async () => { await navigation.PopAsync();});
        }

        private async Task CrearUsuario()
        {
            if (string.IsNullOrEmpty(Auto.id) ||
               string.IsNullOrEmpty(Auto.poliza) ||
               string.IsNullOrEmpty(Auto.marca) ||
               string.IsNullOrEmpty(Auto.modelo) ||
               string.IsNullOrEmpty(Auto.anio)
               )
            {
                await UserDialogs.Instance.AlertAsync("Uno o más campos están vacios", "Campos Vacios", "Ok");
                return;
            }

            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                HttpResponseMessage VerificarAuto = await ConexionAuto.Verificar(Auto.id,Auto.poliza);
                if (VerificarAuto.IsSuccessStatusCode)
                {
                    Usuario.id_automovil = Auto.id;
                    Usuario.estado = true;
                    Usuario.email = Usuario.email.ToLower();
                    await ConexionAuto.Register(Auto);
                    await ConexionUsuario.Register(Usuario);
                    SesionActualModel.Instancia().Auto = Auto;
                    SesionActualModel.Instancia().Usuario = Usuario;
                    await navigation.PopToRootAsync();
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(await VerificarAuto.Content.ReadAsStringAsync(), "Ventana de error", "Salir");
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
