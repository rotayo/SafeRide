using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoFinal.Conexion;
using ProyectoFinal.Datos;
using ProyectoFinal.Model;
using ProyectoFinal.SignalR;
using ProyectoFinal.View;
using ProyectoFinal.View.administrador;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.administrador
{
    public class plantillaUsuario
    {
        public string id { get; set; }
        public string nombre { get; set; }
        public string nombreCompleto { get; set; }
        public string cedula { get; set; }
        public string rol { get; set; }
        public string estado { get; set; }
        public string id_auto { get; set; }
        public Command PaginaUsuario { get; set; }

        public plantillaUsuario(INavigation navigation)
        {
            PaginaUsuario = new Command<string>(async (id) => {
                AutoModel auto = null;
                if (id_auto != "")
                {
                    var autoConexion = new AutoConexion();
                    var autoJson = await autoConexion.Obtener(id_auto);
                    auto = JsonConvert.DeserializeObject<AutoModel>(autoJson);
                }
                await navigation.PushAsync(new VerUsuarioAdministrador(id, auto));
            });
        }
    }

    public class ListaUsuariosAdministradorViewModel
    {
        public AutoConexion autoConexion { get; set; }
        public Picker PickerFiltros {  get; set; }
        public Command CambiarPagina { get; set; }
        private Stack<plantillaUsuario> listaUsuarios { get; set; }
        private OrdenamientoDeListas Ordenamiento {  get; set; }
        public ListView listView { get; set; }
        private INavigation navigation { get; set; }

        public ListaUsuariosAdministradorViewModel(ListView ListView, INavigation navigation, Picker picker)
        {
            this.listView = ListView;
            this.navigation = navigation;
            Ordenamiento = new OrdenamientoDeListas();
            autoConexion = new AutoConexion();
            listaUsuarios = new Stack<plantillaUsuario>();
            PickerFiltros = picker;

            PickerFiltros.ItemsSource = new string[2] { "De A - Z", "De Z - A" };

            CambiarPagina = new Command<string>(async destino =>
            {
                switch (destino)
                {
                    case "back":
                        await navigation.PopAsync();
                        break;
                    case "perfil":
                        await navigation.PushAsync(new PerfilPage());
                        break;
                }
            });

        }

        public async Task cargarUsuarios()
        {
            try
            {
                listaUsuarios.Clear();

                foreach (var Usuario in SesionActualModel.Instancia().ListaUsuarios)
                {
                    string estado = "";
                    string rol = "";

                    switch (Usuario.id_rol)
                    {
                        case 1:
                            rol = "Usuario";
                            break;
                        case 2:
                            rol = "Agente";
                            break;
                        case 3:
                            rol = "Administrador";
                            break;
                    }

                    estado = (Usuario.estado) ? "Activo" : "Desactivado";

                    plantillaUsuario UsuarioNuevo = new plantillaUsuario(navigation)
                    {
                        id = Usuario.id,
                        nombre = Usuario.nombre,
                        cedula = "Cédula: #"+Usuario.id,
                        nombreCompleto = "Nombre: "+Usuario.nombre +" "+Usuario.apellido,
                        rol = "Rol: "+rol,
                        estado = estado
                    };

                    if (!string.IsNullOrEmpty(Usuario.id_automovil))
                    {
                        UsuarioNuevo.id_auto = Usuario.id_automovil;
                    }
                    else { 
                        UsuarioNuevo.id_auto = "";
                    }
                    
                    listaUsuarios.Push(UsuarioNuevo);
                }
                listView.ItemsSource = listaUsuarios;
            }
            catch (Exception ex)
            {
                await UserDialogs.Instance.AlertAsync("Error", ex.Message, "OK");
            }
        }

        public Stack<plantillaUsuario> buscarPorNombre(string nombre)
        {
            Stack<plantillaUsuario> nuevaLista = new Stack<plantillaUsuario>();

            foreach (var usuario in listaUsuarios)
            {
                if(usuario.nombre.ToLower() == nombre.ToLower())
                {
                    nuevaLista.Push(usuario);
                }
            }
            return nuevaLista;
        }

        public void OrdenarDeAZ()
        {
            var listaOrdenada = Ordenamiento.OrdenarAZ(listaUsuarios.ToList());
            listaUsuarios.Clear();
            foreach (var usuario in listaOrdenada)
            {
                listaUsuarios.Push(usuario);
            }
            ActualizarLista();
        }

        public void OrdenarDeZA()
        {
            var listaOrdenada = Ordenamiento.OrdenarZA(listaUsuarios.ToList());
            listaUsuarios.Clear();
            foreach (var incidente in listaOrdenada)
            {
                listaUsuarios.Push(incidente);
            }
            ActualizarLista();
        }

        private void ActualizarLista()
        {
            listView.ItemsSource = null;
            listView.ItemsSource = listaUsuarios;
        }
    }
}
