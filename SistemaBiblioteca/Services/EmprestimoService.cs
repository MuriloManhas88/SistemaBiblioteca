using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Enums;
using SistemaBiblioteca.Exceptions;

namespace SistemaBiblioteca.Services
{
    public class EmprestimoService : IEmprestimoService
    {
        private readonly BibliotecaContext _context;
        private readonly ILivroService _livroService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMultaService _multaService;

        public EmprestimoService(BibliotecaContext context, ILivroService livroService, IUsuarioService usuarioService, IMultaService multaService)
        {
            _context = context;
            _livroService = livroService;
            _usuarioService = usuarioService;
            _multaService = multaService;
        }

        public async Task<Emprestimo> RegistrarEmprestimo(string isbn, int usuarioId)
        {
            // Validar se o livro existe e está disponível
            var livro = await _livroService.ObterLivroPorISBN(isbn);
            if (livro == null)
                throw new LivroNaoEncontradoException(isbn);

            if (livro.Status != StatusLivro.DISPONIVEL)
                throw new LivroIndisponivelException(isbn);

            // Validar se o usuário pode pegar empréstimo
            var usuario = await _usuarioService.ObterUsuarioPorId(usuarioId);
            if (usuario == null)
                throw new UsuarioNaoEncontradoException(usuarioId);

            if (!await _usuarioService.UsuarioPodePegarEmprestimo(usuarioId))
                throw new LimiteEmprestimosExcedidoException(usuarioId);

            // Validar se o usuário tem multas pendentes
            if (await _multaService.UsuarioTemMultasPendentes(usuarioId))
                throw new MultaPendenteException(usuarioId);

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
                throw new EmprestimoNaoEncontradoException(emprestimoId);

            if (emprestimo.Status != StatusEmprestimo.ATIVO)
                throw new EmprestimoInvalidoException("Empréstimo não está ativo.");

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
