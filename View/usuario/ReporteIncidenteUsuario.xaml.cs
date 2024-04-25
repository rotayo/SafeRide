using ProyectoFinal.Model;
using ProyectoFinal.ViewModel.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace ProyectoFinal.View.usuario
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ReporteIncidenteUsuario : ContentPage
	{
		public ReporteIncidenteUsuario ()
		{
			InitializeComponent ();
			this.BindingContext = new ReporteIncidenteViewModel(Navigation);
			Map mapa = MapaModel.Instancia().Mapa;
            GridPrincipal.Children.Add(mapa);
            Grid.SetRow(mapa, 1);
        }
	}
}