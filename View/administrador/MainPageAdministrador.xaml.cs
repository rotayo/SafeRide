using ProyectoFinal.ViewModel.administrador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.View.administrador
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPageAdministrador : ContentPage
	{
        private MainPageAdministradorViewModel viewModel;
        public MainPageAdministrador ()
		{
			InitializeComponent ();
			viewModel = new MainPageAdministradorViewModel(Navigation);
			this.BindingContext = viewModel;
        }

		protected override void OnDisappearing()
		{
			//viewModel.CerraConexion();
		}
	}
}