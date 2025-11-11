using SistemaBiblioteca.Enums;

namespace SistemaBiblioteca.Models
{
    public class Multa
    {
        public int Id { get; set; }
        public int EmprestimoId { get; set; }
        public decimal Valor { get; set; }
        public StatusMulta Status { get; set; } = StatusMulta.PENDENTE;
    }
}
