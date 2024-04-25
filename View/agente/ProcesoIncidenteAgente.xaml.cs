using ProyectoFinal.Model;
using ProyectoFinal.ViewModel.agente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.View.agente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProcesoIncidenteAgente : ContentPage
    {
        public ProcesoIncidenteAgente(int id)
        {
            InitializeComponent();
            this.BindingContext = new ProcesoIncidenteViewModel(Navigation,LlegadaUbicacionCheckBox,ValidarCasoCheckBox,id);
            Map mapa = MapaModel.Instancia().Mapa;
            GridPrincipal.Children.Add(mapa);
            Grid.SetRow(mapa, 1);
        }
    }
}