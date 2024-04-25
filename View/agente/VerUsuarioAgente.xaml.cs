using ProyectoFinal.Model;
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
	public partial class VerUsuarioAgente : ContentPage
	{
        private VerUsuarioAgenteViewModel ViewModel {  get; set; }
        public VerUsuarioAgente (UsuarioModel Usuario, AutoModel Auto)
		{
			InitializeComponent ();
			this.BindingContext = new VerUsuarioAgenteViewModel(Navigation, Usuario, Auto);
        }
    }
}