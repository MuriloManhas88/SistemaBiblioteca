using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Models
{
    public class Livro
    {
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public Categoria Categoria { get; set; }
        public StatusLivro Status { get; set; } = StatusLivro.DISPONIVEL;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
