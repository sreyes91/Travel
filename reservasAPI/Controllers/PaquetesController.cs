using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservasAPI.Models;
using reservasAPI.Repository;

namespace reservasAPI.Controllers
{
    [Route("api/Paquetes")]
    [ApiController]
    public class PaquetesController : ControllerBase
    {
        private readonly PaquetesRep paquetesRep;
        private readonly IWebHostEnvironment webHost;

        public PaquetesController(IWebHostEnvironment _webHost)
        {
            paquetesRep = new();
            webHost = _webHost;
        }

        [HttpGet("MostrarPaquetes")]
        public ActionResult MostrarPaquetes()
        {
            List<PaquetesModels>? list = new();

            list = paquetesRep.ListaPaquetes();
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("No se encontraron registros");
            }
        }

        [HttpGet("BuscarPaquete/{codigo}")]
        public ActionResult MostrarPaquetesCodigo(int codigo)
        {
            PaquetesModels? list = new();

            list = paquetesRep.BuscarPaquete(codigo);
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("No se encontraron registros");
            }
        }

        [HttpPost("RegistrarPaquete")]
        public ActionResult RegistrarPaquete(PaquetesModels Paquetes)
        {
            int resultado = paquetesRep.IsertarPaquete(Paquetes);

            if (resultado == -1)
            {
                return BadRequest("Las fechas están fuera de rango. Fecha de inicio: " + Paquetes.FechaIni + " Fecha fin: " + Paquetes.FechaFin);
            }

            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intengar guardar el registro.");
            }
            else
            {
                return Ok(MostrarPaquetesCodigo(resultado));
            }
        }

        [HttpPut("ActualizarPaquete")]
        public ActionResult ActualizarPaquete(PaquetesModels Paquetes)
        {
            int resultado = paquetesRep.ActualizarPaquete(Paquetes);

            if (resultado == -1)
            {
                return BadRequest("Las fechas están fuera de rango. Fecha de inicio: " + Paquetes.FechaIni + " Fecha fin: " + Paquetes.FechaFin);
            }

            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intengar actualizar los datos del registro.");
            }
            else
            {
                return Ok(MostrarPaquetesCodigo(resultado));
            }
        }

        [HttpDelete("EliminarPaquete")]
        public ActionResult EliminarPaquete(int codigo)
        {
            int resultado = paquetesRep.EliminarPaquete(codigo);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intengar elminar el registro.");
            }
            else
            {
                return Ok("Registro eliminado con éxito.");
            }
        }        
    }
}
