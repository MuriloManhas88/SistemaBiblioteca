using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Services
{
    public class MultaService : IMultaService
    {
        private readonly BibliotecaContext _context;

        public MultaService(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<bool> UsuarioTemMultasPendentes(int usuarioId)
        {
            var multasPendentes = await _context.Multas
                .Join(_context.Emprestimos,
                    multa => multa.EmprestimoId,
                    emprestimo => emprestimo.Id,
                    (multa, emprestimo) => new { multa, emprestimo })
                .Where(x => x.emprestimo.UsuarioId == usuarioId && x.multa.Status == StatusMulta.PENDENTE)
                .AnyAsync();

            return multasPendentes;
        }

        public async Task<IEnumerable<Multa>> ObterMultasPorUsuario(int usuarioId)
        {
            var multas = await _context.Multas
                .Join(_context.Emprestimos,
                    multa => multa.EmprestimoId,
                    emprestimo => emprestimo.Id,
                    (multa, emprestimo) => new { multa, emprestimo })
                .Where(x => x.emprestimo.UsuarioId == usuarioId)
                .Select(x => x.multa)
                .ToListAsync();

            return multas;
        }

        public async Task<Multa> PagarMulta(int multaId)
        {
            var multa = await _context.Multas.FindAsync(multaId);
            if (multa == null)
                throw new Exception($"Multa com ID {multaId} não encontrada.");

            multa.Status = StatusMulta.PAGA;
            await _context.SaveChangesAsync();
            return multa;
        }
    }
}
