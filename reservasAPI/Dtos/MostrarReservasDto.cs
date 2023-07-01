using reservasAPI.Models;

namespace reservasAPI.Dtos
{
    public class MostrarReservasDto
    {
        public int CodigoReserva { get; set; }
        public string? CodigoReferencia { get; set; }
        public DateTime FechaReserva { get; set; }
        public ClientesModels? Cliente { get; set; }
        public PaquetesDestinosDto? PaquetesDestinos { get; set; }

    }
}
