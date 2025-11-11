using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Services
{
    public class EmprestimoService : IEmprestimoService
    {
        private readonly BibliotecaContext _context;
        private readonly ILivroService _livroService;
        private readonly IUsuarioService _usuarioService;

        public EmprestimoService(BibliotecaContext context, ILivroService livroService, IUsuarioService usuarioService)
        {
            _context = context;
            _livroService = livroService;
            _usuarioService = usuarioService;
        }

        public async Task<Emprestimo> RegistrarEmprestimo(string isbn, int usuarioId)
        {
            // Validar se o livro existe e está disponível
            var livro = await _livroService.ObterLivroPorISBN(isbn);
            if (livro == null)
                throw new Exception($"Livro com ISBN {isbn} não encontrado.");

            if (livro.Status != StatusLivro.DISPONIVEL)
                throw new Exception("Livro não está disponível para empréstimo.");

            // Validar se o usuário pode pegar empréstimo
            var usuario = await _usuarioService.ObterUsuarioPorId(usuarioId);
            if (usuario == null)
                throw new Exception($"Usuário com ID {usuarioId} não encontrado.");

            if (!await _usuarioService.UsuarioPodePegarEmprestimo(usuarioId))
                throw new Exception("Usuário já possui 3 empréstimos ativos.");

            // Calcular prazo de devolução baseado no tipo de usuário
            int diasEmprestimo = usuario.Tipo == TipoUsuario.PROFESSOR ? 30 : 14;

            // Criar empréstimo
            var emprestimo = new Emprestimo
            {
                ISBN = isbn,
                UsuarioId = usuarioId,
                DataEmprestimo = DateTime.Now,
                DataPrevistaDevolucao = DateTime.Now.AddDays(diasEmprestimo),
                Status = StatusEmprestimo.ATIVO
            };

            _context.Emprestimos.Add(emprestimo);
            
            // Atualizar status do livro
            await _livroService.AtualizarStatusLivro(isbn, StatusLivro.EMPRESTADO);
            
            await _context.SaveChangesAsync();
            return emprestimo;
        }

        public async Task<Emprestimo> RegistrarDevolucao(int emprestimoId)
        {
            var emprestimo = await _context.Emprestimos.FindAsync(emprestimoId);
            if (emprestimo == null)
                throw new Exception($"Empréstimo com ID {emprestimoId} não encontrado.");

            if (emprestimo.Status != StatusEmprestimo.ATIVO)
                throw new Exception("Empréstimo não está ativo.");

            emprestimo.DataRealDevolucao = DateTime.Now;
            emprestimo.Status = StatusEmprestimo.FINALIZADO;

            // Calcular multa se houver atraso
            if (emprestimo.DataRealDevolucao > emprestimo.DataPrevistaDevolucao)
            {
                var diasAtraso = (emprestimo.DataRealDevolucao.Value - emprestimo.DataPrevistaDevolucao).Days;
                var valorMulta = diasAtraso * 1.00m; // R$ 1,00 por dia

                var multa = new Multa
                {
                    EmprestimoId = emprestimoId,
                    Valor = valorMulta,
                    Status = StatusMulta.PENDENTE
                };

                _context.Multas.Add(multa);
                emprestimo.Status = StatusEmprestimo.ATRASADO;
            }

            // Atualizar status do livro
            await _livroService.AtualizarStatusLivro(emprestimo.ISBN, StatusLivro.DISPONIVEL);

            await _context.SaveChangesAsync();
            return emprestimo;
        }

        public async Task<Emprestimo?> ObterEmprestimoPorId(int id)
        {
            return await _context.Emprestimos.FindAsync(id);
        }

        public async Task<IEnumerable<Emprestimo>> ObterEmprestimosPorUsuario(int usuarioId)
        {
            return await _context.Emprestimos
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();
        }
    }
}
