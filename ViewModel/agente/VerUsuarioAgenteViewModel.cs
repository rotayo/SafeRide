using Acr.UserDialogs;
using Newtonsoft.Json;
using ProyectoFinal.Conexion;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.agente
{
    class VerUsuarioAgenteViewModel
    {
        public UsuarioModel Usuario { get; set; }
        public AutoModel Auto{  get; set; }
        public Command Profile { get; set; }
        public Command Back {  get; set; }
        private INavigation navigation { get; set; }

        public VerUsuarioAgenteViewModel(INavigation navigation, UsuarioModel Usuario, AutoModel Auto)
        {
            this.Usuario = Usuario;
            this.Auto = Auto;
            this.navigation = navigation;
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));
            Back = new Command(async () => await this.navigation.PopAsync());
        }
       
    }
}
