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
    public partial class MainPageUsuario : ContentPage
    {
        public MainPageUsuario()
        {
            InitializeComponent();
            this.BindingContext = new MainPageUsuarioViewModel(Navigation);
            Console.WriteLine(Navigation.NavigationStack.Count);
        }
    }
}