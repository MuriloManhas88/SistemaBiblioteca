using Microsoft.AspNetCore.Mvc;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Services;

namespace SistemaBiblioteca.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MultasController : ControllerBase
    {
        private readonly IMultaService _multaService;

        public MultasController(IMultaService multaService)
        {
            _multaService = multaService;
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Multa>>> ObterMultasPorUsuario(int usuarioId)
        {
            var multas = await _multaService.ObterMultasPorUsuario(usuarioId);
            return Ok(multas);
        }

        [HttpPost("{multaId}/pagar")]
        public async Task<ActionResult<Multa>> PagarMulta(int multaId)
        {
            try
            {
                var multa = await _multaService.PagarMulta(multaId);
                return Ok(multa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("usuario/{usuarioId}/pendentes")]
        public async Task<ActionResult<bool>> VerificarMultasPendentes(int usuarioId)
        {
            var temMultas = await _multaService.UsuarioTemMultasPendentes(usuarioId);
            return Ok(new { UsuarioId = usuarioId, TemMultasPendentes = temMultas });
        }
    }
}
