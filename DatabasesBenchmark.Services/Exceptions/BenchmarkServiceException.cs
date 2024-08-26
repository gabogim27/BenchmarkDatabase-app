namespace DatabasesBenchmark.Services.Exceptions
{
    public class BenchmarkServiceException : Exception
    {
        public BenchmarkServiceException()
        {
        }

        public BenchmarkServiceException(string message)
            : base(message)
        {
        }

        public BenchmarkServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
