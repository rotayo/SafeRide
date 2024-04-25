using ProyectoFinal.Datos;
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
	public partial class ListaIncidentesAdministrador : ContentPage
	{
        private ListaIncidentesAdministradorViewModel ViewModel { get; set; }
        public ListaIncidentesAdministrador ()
		{
			InitializeComponent ();
            ViewModel = new ListaIncidentesAdministradorViewModel(listaIncidentes, Navigation, PickerFiltros);
            this.BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.cargarIncidentes();
        }

        private void listaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
                listaIncidentes.ItemsSource = null;
                listaIncidentes.ItemsSource = incidentes;
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