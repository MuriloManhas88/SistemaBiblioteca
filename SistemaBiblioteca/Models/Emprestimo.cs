using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public DateTime DataEmprestimo { get; set; } = DateTime.Now;
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataRealDevolucao { get; set; }
        public StatusEmprestimo Status { get; set; } = StatusEmprestimo.ATIVO;
    }
}
