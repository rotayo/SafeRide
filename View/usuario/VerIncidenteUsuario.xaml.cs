using ProyectoFinal.ViewModel.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.View.usuario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerIncidenteUsuario : ContentPage
    {
        public VerIncidenteUsuario(int id)
        {
            InitializeComponent();
            this.BindingContext = new VerIncidenteUsuarioViewModel(Navigation, id);
        }
    }
}