using DatabasesBenchmark.Domain.Entities;
using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
using DatabasesBenchmark.Services.Interfaces;
using System.Diagnostics;
using DatabasesBenchmark.Services.Helpers.Interfaces;
using DatabasesBenchmark.Services.Exceptions;

namespace DatabasesBenchmark.Services.Implementations
{
    public class BenchmarkService : IBenchmarkService
    {
        private readonly IBenchmarkDbContextFactory _dbContextFactory;
        private readonly IStringHelper _stringHelper;
        private DatabaseProvider _provider;

        public BenchmarkService(IBenchmarkDbContextFactory dbContextFactory, IStringHelper stringHelper)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _stringHelper = stringHelper;
        }

        /// <summary>
        /// Sets the database provider to be used by the service.
        /// </summary>
        /// <param name="provider">The database provider to be used.</param>
        public void SetDatabaseProvider(DatabaseProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Runs a benchmark test that measures the performance of inserting new records into the database.
        /// </summary>
        /// <param name="numRegistries">The number of records to insert.</param>
        /// <param name="numThreads">The number of threads to use for the benchmark.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating the time taken for the benchmark in milliseconds.</returns>
        public async Task<long> RunInsertionBenchmark(int numRegistries, int numThreads)
        {
            try
            {
                return await RunBenchmark(numRegistries, numThreads, async (unitOfWork, random, usedIds) =>
                {
                    var entity = new Benchmark
                    {
                        RandomString = _stringHelper.GenerateRandomString()
                    };
                    await unitOfWork.BenchmarkRepository.InsertAsync(entity);
                    await unitOfWork.SaveChangesAsync();
                });
            }
            catch (Exception ex)
            {
                throw new BenchmarkServiceException("An error occurred while running the insertion benchmark.", ex);
            }
        }

        /// <summary>
        /// Runs a benchmark test that measures the performance of selecting and updating existing records in the database.
        /// </summary>
        /// <param name="numRegistries">The number of records to work with.</param>
        /// <param name="numThreads">The number of threads to use for the benchmark.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating the time taken for the benchmark in milliseconds.</returns>
        public async Task<long> RunSelectPlusUpdateBenchmark(int numRegistries, int numThreads)
        {
            try
            {
                return await RunBenchmark(numRegistries, numThreads, async (unitOfWork, random, usedIds) =>
                {
                    var id = GetRandomId(numRegistries, random, usedIds);
                    var entity = await unitOfWork.BenchmarkRepository.GetByIdAsync(id);

                    if (entity != null)
                    {
                        entity.RandomString = _stringHelper.GenerateRandomString();
                        await unitOfWork.BenchmarkRepository.UpdateAsync(entity);
                        await unitOfWork.SaveChangesAsync();
                    }
                });
            }
            catch (Exception ex)
            {
                throw new BenchmarkServiceException("An error occurred while running the select and update benchmark.", ex);
            }
        }

        /// <summary>
        /// Runs a benchmark test that measures the performance of selecting, updating, and inserting new records into the database.
        /// </summary>
        /// <param name="numRegistries">The number of records to work with.</param>
        /// <param name="numThreads">The number of threads to use for the benchmark.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating the time taken for the benchmark in milliseconds.</returns>
        public async Task<long> RunSelectPlusUpdatePlusInsertionBenchmark(int numRegistries, int numThreads)
        {
            try
            {
                return await RunBenchmark(numRegistries, numThreads, async (unitOfWork, random, usedIds) =>
                {
                    var id = GetRandomId(numRegistries, random, usedIds);
                    var entity = await unitOfWork.BenchmarkRepository.GetByIdAsync(id);

                    if (entity != null)
                    {
                        entity.RandomString = _stringHelper.GenerateRandomString();
                        await unitOfWork.BenchmarkRepository.UpdateAsync(entity);
                    }

                    var newEntity = new Benchmark
                    {
                        RandomString = _stringHelper.GenerateRandomString()
                    };
                    await unitOfWork.BenchmarkRepository.InsertAsync(newEntity);
                    await unitOfWork.SaveChangesAsync();
                });
            }
            catch (Exception ex)
            {
                throw new BenchmarkServiceException("An error occurred while running the select, update, and insertion benchmark.", ex);
            }
        }

        /// <summary>
        /// Executes a benchmarking operation using multiple threads to perform a series of operations on the database.
        /// </summary>
        /// <param name="numRegistries">The number of records to process in each thread.</param>
        /// <param name="numThreads">The number of threads to use for the benchmark.</param>
        /// <param name="operation">A function defining the operation to be performed within the benchmark.</param>
        /// <returns>A task representing the asynchronous operation, with a result indicating the time taken for the benchmark in milliseconds.</returns>
        private async Task<long> RunBenchmark(int numRegistries, int numThreads, Func<IUnitOfWork, Random, HashSet<int>, Task> operation)
        {
            var tasks = Enumerable.Range(0, numThreads).Select(async _ =>
            {
                var random = new Random();
                var usedIds = new HashSet<int>();

                for (int i = 0; i < numRegistries; i++)
                {
                    using var unitOfWork = _dbContextFactory.CreateUnitOfWork(_provider);
                    await operation(unitOfWork, random, usedIds);
                }
            }).ToArray();

            var stopwatch = Stopwatch.StartNew();
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Generates a random ID that has not been used previously.
        /// </summary>
        /// <param name="numRegistries">The maximum number of possible IDs.</param>
        /// <param name="random">The random number generator.</param>
        /// <param name="usedIds">A set of IDs that have already been used.</param>
        /// <returns>A unique random ID.</returns>
        private int GetRandomId(int numRegistries, Random random, HashSet<int> usedIds)
        {
            int id;
            do
            {
                id = random.Next(1, numRegistries + 1);
            } while (usedIds.Contains(id));

            usedIds.Add(id);
            return id;
        }
    }

}
