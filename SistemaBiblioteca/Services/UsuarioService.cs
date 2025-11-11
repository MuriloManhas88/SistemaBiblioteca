using Microsoft.EntityFrameworkCore;
using SistemaBiblioteca.Data;
using SistemaBiblioteca.Models;
using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly BibliotecaContext _context;

        public UsuarioService(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<Usuario> CadastrarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> ObterUsuarioPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<IEnumerable<Usuario>> ObterTodosUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<int> ObterQuantidadeEmprestimosAtivos(int usuarioId)
        {
            return await _context.Emprestimos
                .CountAsync(e => e.UsuarioId == usuarioId && e.Status == StatusEmprestimo.ATIVO);
        }

        public async Task<bool> UsuarioPodePegarEmprestimo(int usuarioId)
        {
            var quantidadeAtivos = await ObterQuantidadeEmprestimosAtivos(usuarioId);
            return quantidadeAtivos < 3;
        }
    }
}
