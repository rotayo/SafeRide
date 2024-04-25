using Acr.UserDialogs;
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
	public partial class ListaUsuariosAdministrador : ContentPage
	{
		private ListaUsuariosAdministradorViewModel ViewModel;
        public ListaUsuariosAdministrador()
        {
            InitializeComponent();
            ViewModel = new ListaUsuariosAdministradorViewModel(listaUsuarios, Navigation, PickerFiltros);
            this.BindingContext = ViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.cargarUsuarios();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void listaUsuarios_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ((ListView)sender).SelectedItem = null;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string nombre = txtSearch.Text;
            if (!String.IsNullOrEmpty(nombre))
            {
                var students = ViewModel.buscarPorNombre(nombre);
                listaUsuarios.ItemsSource = null;
                listaUsuarios.ItemsSource = students;
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
                case "De A - Z":
                    ViewModel.OrdenarDeAZ();
                    break;
                case "De Z - A":
                    ViewModel.OrdenarDeZA();
                    break;
                default:
                    break;
            }
        }
    }
}