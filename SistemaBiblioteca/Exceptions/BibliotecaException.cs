namespace SistemaBiblioteca.Exceptions
{
    public class BibliotecaException : Exception
    {
        public BibliotecaException(string message) : base(message)
        {
        }

        public BibliotecaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
