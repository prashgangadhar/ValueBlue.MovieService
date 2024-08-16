namespace ValueBlue.Movies.Application.Exceptions
{
    public class SaveSearchRequestFailedException : Exception
    {
        public SaveSearchRequestFailedException() { } //For unit tests
        public SaveSearchRequestFailedException(string message) : base(message) { }
    }
}
