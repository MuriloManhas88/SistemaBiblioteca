using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public interface IMultaService
    {
        Task<bool> UsuarioTemMultasPendentes(int usuarioId);
        Task<IEnumerable<Multa>> ObterMultasPorUsuario(int usuarioId);
        Task<Multa> PagarMulta(int multaId);
    }
}
