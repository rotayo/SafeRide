using ProyectoFinal.Model;
using ProyectoFinal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrarAuto : ContentPage
    {
        public RegistrarAuto(UsuarioModel Usuario)
        {
            InitializeComponent();
            this.BindingContext = new RegistrarAutoViewModel(Usuario, Navigation);
        }
    }
}