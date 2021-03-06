namespace WebApi.Models
{
    public class Orden
    {
        public int IdOrden { get; set; }
        public int IdEquipo { get; set; }
        public string OrdenEstado { get; set; }
        public string Cliente { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }
        public string Celular { get; set; }
        public string FechaInicioOT { get; set; }
        public string FechaFinalOT { get; set; }
        public string TipoTrabajo { get; set; }
        public string ProblemaEquipo { get; set; }
        public string Notas { get; set; }
        public string Equipo { get; set; }
        public string MarcaId { get; set; }
        public string ModeloId { get; set; }
        public string Voltaje { get; set; }
        public string Amperaje { get; set; }
        public string Presion { get; set; }
        public string FechaInicioTR { get; set; }
        public string FechaFinalTR { get; set; }
        public string Observacion { get; set; }
        public byte[] ImagenTR { get; set; }
        public string RutaImagen { get; set; }
        public string Latitud { get; set; }
        public string Logintud { get; set; }
    }
}