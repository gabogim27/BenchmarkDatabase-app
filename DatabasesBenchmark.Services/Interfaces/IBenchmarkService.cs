namespace DatabasesBenchmark.Services.Interfaces
{
    public interface IBenchmarkService
    {
        Task<long> RunInsertionBenchmark(int numRegistries, int numThreads);

        Task<long> RunSelectPlusUpdateBenchmark(int numRegistries, int numThreads);

        Task<long> RunSelectPlusUpdatePlusInsertionBenchmark(int numRegistries, int numThreads);
    }
}
