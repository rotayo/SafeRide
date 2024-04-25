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
	public partial class VerIncidenteAdministrador : ContentPage
	{
		public VerIncidenteAdministrador (int id)
		{
			InitializeComponent ();
			this.BindingContext = new VerIncidenteAdministradorViewModel(id, Navigation, PickerAgentes);
		}
	}
}