using SistemaBiblioteca.Models;

namespace SistemaBiblioteca.Services
{
    public interface ILivroService
    {
        Task<Livro> CadastrarLivro(Livro livro);
        Task<Livro?> ObterLivroPorISBN(string isbn);
        Task<IEnumerable<Livro>> ObterTodosLivros();
        Task<Livro> AtualizarStatusLivro(string isbn, Enums.StatusLivro novoStatus);
    }
}
