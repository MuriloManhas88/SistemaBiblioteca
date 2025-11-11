namespace SistemaBiblioteca.Services
{
    public interface IRelatorioService
    {
        Task<IEnumerable<object>> ObterLivrosMaisEmprestados();
        Task<IEnumerable<object>> ObterUsuariosComMaisEmprestimos();
        Task<IEnumerable<object>> ObterEmprestimosEmAtraso();
    }
}
