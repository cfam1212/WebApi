//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DetalleEquipos
    {
        public int id_detalle { get; set; }
        public int id_cabecera { get; set; }
        public string nombre_detalle { get; set; }
        public string valor_detalle { get; set; }
        public int valor_detallei { get; set; }
        public bool estado_detalle { get; set; }
        public string aux3_detalle { get; set; }
        public string aux4_detalle { get; set; }
    
        public virtual CabeceraEquipos CabeceraEquipos { get; set; }
    }
}