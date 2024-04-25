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
	public partial class HistorialUsuario : ContentPage
	{
		HistorialUsuarioViewModel ViewModel {  get; set; }
		public HistorialUsuario ()
		{
            InitializeComponent();
            ViewModel = new HistorialUsuarioViewModel(lista, Navigation, PickerFiltros);
            BindingContext = ViewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
			ViewModel.CargarIncidentes();
        }

        private void listaIncidentes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ((ListView)sender).SelectedItem = null;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string id = txtSearch.Text;
            if (!String.IsNullOrEmpty(id))
            {
                var incidentes = ViewModel.buscarPorId(id);
                lista.ItemsSource = null;
                lista.ItemsSource = incidentes;
            }
            else
            {
                OnAppearing();
            }
        }

        private void PickerFiltros_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                string filtroSeleccionado = PickerFiltros.SelectedItem as string;
                AplicarFiltro(filtroSeleccionado);
            }
        }

        private void AplicarFiltro(string filtro)
        {
            switch (filtro)
            {
                case "Por Antiguedad":
                    ViewModel.OrdenarPorAntiguedad();
                    break;
                case "Más Nuevo":
                    ViewModel.OrdenarPorNuevo();
                    break;
                default:
                    break;
            }
        }
    }
}