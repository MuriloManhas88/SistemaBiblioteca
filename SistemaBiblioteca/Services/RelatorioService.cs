using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly BibliotecaContext _context;

        public RelatorioService(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<object>> ObterLivrosMaisEmprestados()
        {
            var resultado = await _context.Emprestimos
                .GroupBy(e => e.ISBN)
                .Select(g => new
                {
                    ISBN = g.Key,
                    QuantidadeEmprestimos = g.Count()
                })
                .OrderByDescending(x => x.QuantidadeEmprestimos)
                .Take(10)
                .ToListAsync();

            return resultado;
        }

        public async Task<IEnumerable<object>> ObterUsuariosComMaisEmprestimos()
        {
            var resultado = await _context.Emprestimos
                .GroupBy(e => e.UsuarioId)
                .Select(g => new
                {
                    UsuarioId = g.Key,
                    QuantidadeEmprestimos = g.Count()
                })
                .OrderByDescending(x => x.QuantidadeEmprestimos)
                .Take(10)
                .ToListAsync();

            return resultado;
        }

        public async Task<IEnumerable<object>> ObterEmprestimosEmAtraso()
        {
            var hoje = DateTime.Now;
            var resultado = await _context.Emprestimos
                .Where(e => e.Status == StatusEmprestimo.ATIVO && e.DataPrevistaDevolucao < hoje)
                .Select(e => new
                {
                    e.Id,
                    e.ISBN,
                    e.UsuarioId,
                    e.DataEmprestimo,
                    e.DataPrevistaDevolucao,
                    DiasAtraso = (hoje - e.DataPrevistaDevolucao).Days
                })
                .ToListAsync();

            return resultado;
        }
    }
}
