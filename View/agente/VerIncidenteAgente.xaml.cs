using ProyectoFinal.ViewModel.agente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.View.agente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerIncidenteAgente : ContentPage
    {
        public VerIncidenteAgente(int id)
        {
            InitializeComponent();
            this.BindingContext = new VerIncidenteAgenteViewModel(Navigation, id);
        }
    }
}