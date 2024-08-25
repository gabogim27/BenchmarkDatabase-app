using DatabasesBenchmark.Domain.Entities;
using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
using DatabasesBenchmark.Services.Interfaces;
using System.Diagnostics;
using DatabasesBenchmark.Services.Helpers.Interfaces;

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

        public void SetDatabaseProvider(DatabaseProvider provider)
        {
            _provider = provider;
        }

        public async Task<long> RunInsertionBenchmark(int numRegistries, int numThreads)
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

        public async Task<long> RunSelectPlusUpdateBenchmark(int numRegistries, int numThreads)
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

        public async Task<long> RunSelectPlusUpdatePlusInsertionBenchmark(int numRegistries, int numThreads)
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
