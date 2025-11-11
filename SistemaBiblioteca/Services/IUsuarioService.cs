using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> CadastrarUsuario(Usuario usuario);
        Task<Usuario?> ObterUsuarioPorId(int id);
        Task<IEnumerable<Usuario>> ObterTodosUsuarios();
        Task<int> ObterQuantidadeEmprestimosAtivos(int usuarioId);
        Task<bool> UsuarioPodePegarEmprestimo(int usuarioId);
    }
}
