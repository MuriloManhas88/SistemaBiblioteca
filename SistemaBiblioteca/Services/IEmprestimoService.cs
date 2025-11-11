using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public interface IEmprestimoService
    {
        Task<Emprestimo> RegistrarEmprestimo(string isbn, int usuarioId);
        Task<Emprestimo> RegistrarDevolucao(int emprestimoId);
        Task<Emprestimo?> ObterEmprestimoPorId(int id);
        Task<IEnumerable<Emprestimo>> ObterEmprestimosPorUsuario(int usuarioId);
    }
}
