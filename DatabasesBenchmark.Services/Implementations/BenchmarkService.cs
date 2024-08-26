<<<<<<< HEAD
﻿using System.Collections.Concurrent;
using DatabasesBenchmark.Domain.Entities;
=======
﻿using DatabasesBenchmark.Domain.Entities;
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
using DatabasesBenchmark.Services.Interfaces;
using System.Diagnostics;
<<<<<<< HEAD
=======
using DatabasesBenchmark.Services.Helpers.Interfaces;
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8

namespace DatabasesBenchmark.Services.Implementations
{
    public class BenchmarkService : IBenchmarkService
    {
        private readonly IBenchmarkDbContextFactory _dbContextFactory;
<<<<<<< HEAD
        private DatabaseProvider _provider;

        //private ConcurrentBag<int> RandomNumbers = new ConcurrentBag<int>();

        public BenchmarkService(IBenchmarkDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
=======
        private readonly IStringHelper _stringHelper;
        private DatabaseProvider _provider;

        public BenchmarkService(IBenchmarkDbContextFactory dbContextFactory, IStringHelper stringHelper)
        {
            _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
            _stringHelper = stringHelper;
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
        }

        public void SetDatabaseProvider(DatabaseProvider provider)
        {
            _provider = provider;
        }

        public async Task<long> RunInsertionBenchmark(int numRegistries, int numThreads)
        {
<<<<<<< HEAD
            return await RunBenchmark(numRegistries, numThreads, async (dbContextFactory, random, usedIds) =>
            {
                using var unitOfWork = dbContextFactory.CreateUnitOfWork(DatabaseProvider.MySQL);
                var entity = new Benchmark
                {
                    RandomString = GenerateRandomString()
=======
            return await RunBenchmark(numRegistries, numThreads, async (unitOfWork, random, usedIds) =>
            {
                var entity = new Benchmark
                {
                    RandomString = _stringHelper.GenerateRandomString()
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
                };
                await unitOfWork.BenchmarkRepository.InsertAsync(entity);
                await unitOfWork.SaveChangesAsync();
            });
        }

        public async Task<long> RunSelectPlusUpdateBenchmark(int numRegistries, int numThreads)
        {
<<<<<<< HEAD
            return await RunBenchmark(numRegistries, numThreads, async (dbContextFactory, random, usedIds) =>
            {
                using var unitOfWork = dbContextFactory.CreateUnitOfWork(DatabaseProvider.MySQL);

                var id = CheckRepeatedIds(numRegistries, random, usedIds);

                var entity = await unitOfWork.BenchmarkRepository.GetByIdAsync(id);
                if (entity != null)
                {
                    entity.RandomString = GenerateRandomString();
=======
            return await RunBenchmark(numRegistries, numThreads, async (unitOfWork, random, usedIds) =>
            {
                var id = GetRandomId(numRegistries, random, usedIds);
                var entity = await unitOfWork.BenchmarkRepository.GetByIdAsync(id);

                if (entity != null)
                {
                    entity.RandomString = _stringHelper.GenerateRandomString();
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
                    await unitOfWork.BenchmarkRepository.UpdateAsync(entity);
                    await unitOfWork.SaveChangesAsync();
                }
            });
        }

<<<<<<< HEAD
        private int CheckRepeatedIds(int numRegistries, Random random, HashSet<int> usedIds)
        {
            int id;
            do
            {
                id = random.Next(1, numRegistries + 1);
            } while (usedIds.Contains(id)); // Verificar si el ID ya ha sido utilizado

            usedIds.Add(id); // Agregar el ID al conjunto de IDs utilizados
            return id;
        }

        public async Task<long> RunSelectPlusUpdatePlusInsertionBenchmark(int numRegistries, int numThreads)
        {
            return await RunBenchmark(numRegistries, numThreads, async (dbContextFactory, random, usedIds) =>
            {
                using var unitOfWork = dbContextFactory.CreateUnitOfWork(DatabaseProvider.MySQL);
                var id = CheckRepeatedIds(numRegistries, random, usedIds);
                var entity = await unitOfWork.BenchmarkRepository.GetByIdAsync(id);
                if (entity != null)
                {
                    entity.RandomString = GenerateRandomString();
=======
        public async Task<long> RunSelectPlusUpdatePlusInsertionBenchmark(int numRegistries, int numThreads)
        {
            return await RunBenchmark(numRegistries, numThreads, async (unitOfWork, random, usedIds) =>
            {
                var id = GetRandomId(numRegistries, random, usedIds);
                var entity = await unitOfWork.BenchmarkRepository.GetByIdAsync(id);

                if (entity != null)
                {
                    entity.RandomString = _stringHelper.GenerateRandomString();
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
                    await unitOfWork.BenchmarkRepository.UpdateAsync(entity);
                }

                var newEntity = new Benchmark
                {
<<<<<<< HEAD
                    RandomString = GenerateRandomString()
=======
                    RandomString = _stringHelper.GenerateRandomString()
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
                };
                await unitOfWork.BenchmarkRepository.InsertAsync(newEntity);
                await unitOfWork.SaveChangesAsync();
            });
        }

<<<<<<< HEAD
        private async Task<long> RunBenchmark(int numRegistries, int numThreads, Func<IBenchmarkDbContextFactory, Random, HashSet<int>, Task> operation)
=======
        private async Task<long> RunBenchmark(int numRegistries, int numThreads, Func<IUnitOfWork, Random, HashSet<int>, Task> operation)
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
        {
            var tasks = Enumerable.Range(0, numThreads).Select(async _ =>
            {
                var random = new Random();
<<<<<<< HEAD
                var usedIds = new HashSet<int>(); // HashSet para almacenar los números generados en este hilo

                for (int i = 0; i < numRegistries; i++)
                {
                    await operation(_dbContextFactory, random, usedIds);
=======
                var usedIds = new HashSet<int>();

                for (int i = 0; i < numRegistries; i++)
                {
                    using var unitOfWork = _dbContextFactory.CreateUnitOfWork(_provider);
                    await operation(unitOfWork, random, usedIds);
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
                }
            }).ToArray();

            var stopwatch = Stopwatch.StartNew();
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

<<<<<<< HEAD
        private string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
=======
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

>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
}
