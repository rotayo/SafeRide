using Acr.UserDialogs;
using ProyectoFinal.Model;
using ProyectoFinal.View;
using ProyectoFinal.View.usuario;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace ProyectoFinal.ViewModel.usuario
{
    class ReporteIncidenteViewModel
    {
        public IncidenteModel Incidente { get; set; }
        public Command Next {  get; set; }
        public Command Back { get; set; }
        public Command Profile { get; set; }

        private INavigation navigation;
        public ReporteIncidenteViewModel(INavigation navigation) {
            Incidente = new IncidenteModel {
                fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                hora = DateTime.Now.ToString("HH:mm:ss"),
            };
            this.navigation = navigation;

            Next = new Command(()=> 
                navigation.PushAsync(new ReporteIncidenteDosUsuario(Incidente))
            );
            Back = new Command(() => navigation.PopAsync());
            Profile = new Command(() => navigation.PushAsync(new PerfilPage()));
        }
    }
}
