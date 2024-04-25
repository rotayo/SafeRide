using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoFinal.Conexion;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using ProyectoFinal.View.agente;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel
{
    class MainPageViewModel
    {
        /*ViewModel de Main Page*/

        //Objetos para el Binding
        public UsuarioModel Usuario { get; set; }
        public AutoModel Auto { get; set; }
        private AutoConexion autoConexion { get; set; }
        private UsuarioConexion usuarioConexion { get; set; }
        public Command Login { get; set; }
        public Command Sign { get; set; }
        private INavigation navigation { get; set; }

        //Constructor, se envia navigation como parametro para hacer la navegacion de paginas
        //Se utilizan metodos async debido a la respuesta de la api

        public MainPageViewModel(INavigation navigation) {
            this.navigation = navigation;
            Usuario = new UsuarioModel();
            autoConexion = new AutoConexion();
            usuarioConexion = new UsuarioConexion();
            Login = new Command(async () => await IniciarSesion());
            Sign = new Command(() => {
                navigation.PushAsync(new SignUp());
                Usuario.email = "";
                Usuario.contrasena = "";
            });
        }

        /*Metodo iniciar sesion, si los datos (email, contrasena) son correctos, deja cambiar de pagina
         * ver metodo Login en la clase UsuarioConexion para mas info */
        public async Task IniciarSesion()
        {
            if (string.IsNullOrEmpty(Usuario.email) ||
                string.IsNullOrEmpty(Usuario.contrasena)) 
            {
                await UserDialogs.Instance.AlertAsync("Uno o más campos están vacios", "Campos Vacios", "Ok");
                return;
            }

            try
            {
                UserDialogs.Instance.ShowLoading("Cargando");
                HttpResponseMessage respuesta = await usuarioConexion.Login(Usuario.email.ToLower(), Usuario.contrasena);

                if (respuesta.IsSuccessStatusCode)
                {
                    var usuarioJson = await respuesta.Content.ReadAsStringAsync();
                    Usuario = JsonConvert.DeserializeObject<UsuarioModel>(usuarioJson);

                    //Si la cuenta esta desactivada no permite el inicio de sesion
                    if (!Usuario.estado)
                    {
                        UserDialogs.Instance.HideLoading();
                        UserDialogs.Instance.Alert("Su cuenta se encuentra bloqueada");
                        return;
                    }

                    //Id automovil solo es nulo cuando la sesion es de un rol Administrador
                    if (!string.IsNullOrEmpty(Usuario.id_automovil)) {
                        var autoJson = await autoConexion.Obtener(Usuario.id_automovil);
                        Auto = JsonConvert.DeserializeObject<AutoModel>(autoJson);
                        SesionActualModel.Instancia().Auto = Auto;
                    }

                    SesionActualModel.Instancia().Usuario = Usuario;
                    //Dependiendo del rol entra a un menu u otro
                    CargaDeListas cdl = new CargaDeListas();
                    switch (Usuario.id_rol)
                    {
                        case 1:
                            await cdl.CargarIncidentesSesionActual(Usuario.id, Usuario.id_rol);
                            var IdEnProcesoUsuario = SesionActualModel.Instancia().SeEncuentraEnProceso(Usuario.id_rol);
                            if (IdEnProcesoUsuario != 0)
                            {
                                UsuarioModel Agente = null;
                                IncidenteModel Incidente = SesionActualModel.Instancia().ObtenerIncidencia(IdEnProcesoUsuario);
                                if(Incidente.id_agente != null)
                                {
                                    var respuestaUsuario = await usuarioConexion.ObtenerPorID(Incidente.id_agente);
                                    var AgenteAsignadoJson = await respuestaUsuario.Content.ReadAsStringAsync();
                                    Agente = JsonConvert.DeserializeObject<UsuarioModel>(AgenteAsignadoJson);
                                }
                                await navigation.PushAsync(new ProcesoIncidenteUsuario(Incidente, Agente));
                            }
                            else
                                await navigation.PushAsync(new MainPageUsuario());
                            break;
                        case 2:
                            await cdl.CargarIncidentesSesionActual(Usuario.id, Usuario.id_rol);
                            var IdEnProcesoAgente = SesionActualModel.Instancia().SeEncuentraEnProceso(Usuario.id_rol);
                            if (IdEnProcesoAgente != 0)
                            {
                                var cdm = new CargaDeMapa();
                                cdm.CargaMapaPorIdSesionActual(IdEnProcesoAgente);
                                await navigation.PushAsync(new ProcesoIncidenteAgente(IdEnProcesoAgente));
                            }
                            else
                                await navigation.PushAsync(new MainPageAgente());
                            break;
                        case 3:
                            await cdl.CargarTodo();
                            await navigation.PushAsync(new MainPageAdministrador());
                            break;
                    }
                    navigation.RemovePage(navigation.NavigationStack[0]);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(await respuesta.Content.ReadAsStringAsync(), "Ventana de error", "Salir");
                }
                UserDialogs.Instance.HideLoading();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
