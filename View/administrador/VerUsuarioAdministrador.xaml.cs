using ProyectoFinal.Model;
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
	public partial class VerUsuarioAdministrador : ContentPage
	{
        public VerUsuarioAdministrador (string id, AutoModel auto)
		{
			InitializeComponent ();
            this.BindingContext = new VerUsuarioAdministradorViewModel(id, Navigation, auto, DesactivadoCheck, AgenteCheck);
		}

        private void DesactivadoCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (DesactivadoCheck.IsChecked)
            {
                ActivadoCheck.IsEnabled = true;
                ActivadoCheck.IsChecked = false;
                DesactivadoCheck.IsEnabled = false;
            }
        }

        private void ActivadoCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ActivadoCheck.IsChecked)
            {
                DesactivadoCheck.IsEnabled = true;
                DesactivadoCheck.IsChecked = false;
                ActivadoCheck.IsEnabled = false;
            }
        }

        private void UsuarioCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (UsuarioCheck.IsChecked)
            {
                AgenteCheck.IsEnabled = true;
                AgenteCheck.IsChecked = false;
                UsuarioCheck.IsEnabled = false;
            }
        }

        private void AgenteCheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (AgenteCheck.IsChecked)
            {
                UsuarioCheck.IsEnabled = true;
                UsuarioCheck.IsChecked = false;
                AgenteCheck.IsEnabled = false;
            }
        }
    }
}