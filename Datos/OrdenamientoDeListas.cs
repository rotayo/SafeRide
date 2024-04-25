using ProyectoFinal.Model;
using ProyectoFinal.ViewModel.administrador;
using ProyectoFinal.ViewModel.usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProyectoFinal.Datos
{
    public class OrdenamientoDeListas
    {
        public List<PlantillaIncidenteAdministrador> OrdernarMasAntiguoIncidente(List<PlantillaIncidenteAdministrador> lista)
        {
            return lista.OrderByDescending(incidente => int.Parse(incidente.id)).ToList();
        }

        public List<PlantillaIncidenteAdministrador> OrdernarMasNuevoIncidente(List<PlantillaIncidenteAdministrador> lista)
        {
            return lista.OrderBy(incidente => int.Parse(incidente.id)).ToList();
        }

        public List<PlantillaIncidenteUsuario> OrdernarMasAntiguoIncidenteUsuario(List<PlantillaIncidenteUsuario> lista)
        {
            return lista.OrderByDescending(incidente => int.Parse(incidente.id)).ToList();
        }

        public List<PlantillaIncidenteUsuario> OrdernarMasNuevoIncidenteUsuario(List<PlantillaIncidenteUsuario> lista)
        {
            return lista.OrderBy(incidente => int.Parse(incidente.id)).ToList();
        }

        public List<plantillaUsuario> OrdenarAZ(List<plantillaUsuario> lista)
        {
            return lista.OrderByDescending(usuario => usuario.nombre).ToList();
        }

        public List<plantillaUsuario> OrdenarZA(List<plantillaUsuario> lista)
        {
            return lista.OrderBy(usuario => usuario.nombre).ToList();
        }

    }
}
