using ProyectoFinal.Model;
using ProyectoFinal.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProyectoFinal.ViewModel.usuario
{
    public class VerIncidenteUsuarioViewModel
    {
        public IncidenteModel Incidente { get; set; }
        public Command Back {  get; set; }
        public Command Profile { get; set; }

        private INavigation navigation { get; set; }   
        public VerIncidenteUsuarioViewModel(INavigation navigation, int id)
        {
            this.navigation = navigation;
            Incidente = SesionActualModel.Instancia().ObtenerIncidencia(id);
            Back = new Command(() => this.navigation.PopAsync());
            Profile = new Command(() => this.navigation.PushAsync(new PerfilPage()));
        }
    }
}
