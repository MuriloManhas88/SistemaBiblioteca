using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Services
{
    public class LivroService : ILivroService
    {
        private readonly BibliotecaContext _context;

        public LivroService(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<Livro> CadastrarLivro(Livro livro)
        {
            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();
            return livro;
        }

        public async Task<Livro?> ObterLivroPorISBN(string isbn)
        {
            return await _context.Livros.FindAsync(isbn);
        }

        public async Task<IEnumerable<Livro>> ObterTodosLivros()
        {
            return await _context.Livros.ToListAsync();
        }

        public async Task<Livro> AtualizarStatusLivro(string isbn, StatusLivro novoStatus)
        {
            var livro = await _context.Livros.FindAsync(isbn);
            if (livro == null)
                throw new Exception($"Livro com ISBN {isbn} não encontrado.");

            livro.Status = novoStatus;
            await _context.SaveChangesAsync();
            return livro;
        }
    }
}
