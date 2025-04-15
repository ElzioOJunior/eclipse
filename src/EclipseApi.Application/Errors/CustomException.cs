namespace Application.Exceptions
{
    public class CustomException : Exception
    {
        public string Type { get; }
        public string Detail { get; }

        public CustomException(string type, string message, string detail)
            : base(message)
        {
            Type = type;
            Detail = detail;
        }
    }
}
