using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using reservasAPI.Models;
using reservasAPI.Repository;

namespace reservasAPI.Controllers
{
    [Route("api/Destinos")]
    [ApiController]
    public class DestinosController : ControllerBase
    {
        private readonly DestinosRep destinosRep;
        private readonly IWebHostEnvironment webHost;

        public DestinosController(IWebHostEnvironment _webHost)
        {
            destinosRep = new();
            webHost = _webHost;
        }

        [HttpGet("MostrarDestinos")]
        public ActionResult MostrarDestinos()
        {
            List<DestinosModels>? list = new();

            list = destinosRep.ListaDestinos();
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("No se encontraron registros");
            }
        }

        [HttpGet("BuscarDestinos/{codigo}")]
        public ActionResult MostrarDestinosCodigo(int codigo)
        {
            DestinosModels? list = new();

            list = destinosRep.BuscarDestino(codigo);
            if (list != null)
            {
                return Ok(list);
            }
            else
            {
                return BadRequest("No se encontraron registros");
            }
        }

        [HttpPost("RegistrarDestino")]
        public ActionResult RegistrarDestinos(DestinosModels destinos)
        {
            int resultado = destinosRep.IsertarDestino(destinos);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intengar guardar el registro.");
            }
            else
            {
                return Ok(MostrarDestinosCodigo(resultado));
            }
        }

        [HttpPut("ActualizarDestino")]
        public ActionResult ActualizarDestinos(DestinosModels destinos)
        {
            int resultado = destinosRep.ActualizarDestino(destinos);
            if (resultado == 0)
            {
                return BadRequest("Error inesperado al intengar actualizar los datos del registro.");
            }
            else
            {
                return Ok(MostrarDestinosCodigo(resultado));
            }
        }

        [HttpDelete("EliminarDestino/{codigo}")]
        public ActionResult EliminarDestinos(int codigo)
        {
            int resultado = destinosRep.EliminarDestino(codigo);
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
