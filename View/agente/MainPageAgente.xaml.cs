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
    public partial class MainPageAgente : ContentPage
    {
        MainPageAgenteViewModel viewModel;
        public MainPageAgente()
        {
            InitializeComponent();
            viewModel = new MainPageAgenteViewModel(Navigation, listaIncidentes);
            this.BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.cargarIncidentes();
        }

        private void listaIncidentes_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            ((ListView)sender).SelectedItem = null;
        }
    }
}