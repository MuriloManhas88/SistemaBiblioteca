namespace SistemaBiblioteca.Exceptions
{
    public class MultaPendenteException : BibliotecaException
    {
        public MultaPendenteException(int usuarioId) 
            : base($"Usuário {usuarioId} possui multas pendentes e não pode realizar novos empréstimos.")
        {
        }
    }
}
