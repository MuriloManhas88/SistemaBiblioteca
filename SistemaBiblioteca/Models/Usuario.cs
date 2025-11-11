using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public TipoUsuario Tipo { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
