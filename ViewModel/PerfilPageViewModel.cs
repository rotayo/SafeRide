using ProyectoFinal.Model;
using ProyectoFinal.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel
{
    public class PerfilPageViewModel
    {
        public Command Back { get; set; }
        public Command Editar { get; set; }
        public Command CerrarSesion { get; set; }
        public UsuarioModel Usuario { get; set; }
        public INavigation navigation{ get; set; }

        public PerfilPageViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            Usuario = new UsuarioModel
            {
                id = SesionActualModel.Instancia().Usuario.id,
                nombre = SesionActualModel.Instancia().Usuario.nombre,
                apellido = SesionActualModel.Instancia().Usuario.apellido,
            };

            Back = new Command(() => navigation.PopAsync());
            Editar = new Command(() => navigation.PushAsync(new EditarPerfilPage()));
            CerrarSesion = new Command(async () => await CerrarSesionFunc());
        }

        private async Task CerrarSesionFunc()
        {
            SesionActualModel.Instancia().Usuario = null;
            SesionActualModel.Instancia().Auto = null;
            SesionActualModel.Instancia().CerrarConexionesSignalR();
            navigation.InsertPageBefore(new MainPage(), navigation.NavigationStack[0]);
            await navigation.PopToRootAsync();
        }


    }
}
