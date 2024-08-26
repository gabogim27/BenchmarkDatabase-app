using DatabasesBenchmark.Domain.Entities;
using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Infrastructure.Repositories.Interfaces;
using DatabasesBenchmark.Services.Helpers.Interfaces;
using DatabasesBenchmark.Services.Implementations;
using FluentAssertions;
using Moq;

namespace DatabasesBenchmark.Services.Tests.Services
{
    [TestFixture]
    public class BenchmarkServiceTests
    {
        private Mock<IBenchmarkDbContextFactory> _mockDbContextFactory;
        private Mock<IStringHelper> _mockStringHelper;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IGenericRepository<Benchmark>> _mockBenchmarkRepository;
        private BenchmarkService _service;

        [SetUp]
        public void SetUp()
        {
            _mockDbContextFactory = new Mock<IBenchmarkDbContextFactory>();
            _mockStringHelper = new Mock<IStringHelper>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockBenchmarkRepository = new Mock<IGenericRepository<Benchmark>>();

            _mockUnitOfWork.Setup(u => u.BenchmarkRepository).Returns(_mockBenchmarkRepository.Object);
            _mockDbContextFactory.Setup(f => f.CreateUnitOfWork(It.IsAny<DatabaseProvider>())).Returns(_mockUnitOfWork.Object);

            _service = new BenchmarkService(_mockDbContextFactory.Object, _mockStringHelper.Object);
        }

        [Test]
        public async Task RunInsertionBenchmark_ShouldInsertRecords()
        {
            // Arrange
            const int numRegistries = 10;
            const int numThreads = 2;
            _mockStringHelper.Setup(s => s.GenerateRandomString(It.IsAny<int>())).Returns("RandomString");

            // Act
            var result = await _service.RunInsertionBenchmark(numRegistries, numThreads);

            // Assert
            result.Should().BeGreaterThan(0);
            _mockBenchmarkRepository.Verify(r => r.InsertAsync(It.IsAny<Benchmark>()), Times.Exactly(numRegistries * numThreads));
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Exactly(numRegistries * numThreads));
        }

        [Test]
        public async Task RunSelectPlusUpdateBenchmark_ShouldUpdateRecords()
        {
            // Arrange
            const int numRegistries = 10;
            const int numThreads = 2;
            var benchmark = new Benchmark { RandomString = "OldString" };
            _mockBenchmarkRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(benchmark);
            _mockStringHelper.Setup(s => s.GenerateRandomString(It.IsAny<int>())).Returns("NewString");

            // Act
            var result = await _service.RunSelectPlusUpdateBenchmark(numRegistries, numThreads);

            // Assert
            result.Should().BeGreaterThan(0);
            _mockBenchmarkRepository.Verify(r => r.UpdateAsync(It.Is<Benchmark>(b => b.RandomString == "NewString")), Times.Exactly(numRegistries * numThreads));
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Exactly(numRegistries * numThreads));
        }

        [Test]
        public async Task RunSelectPlusUpdatePlusInsertionBenchmark_ShouldUpdateAndInsertRecords()
        {
            // Arrange
            const int numRegistries = 10;
            const int numThreads = 2;
            var benchmark = new Benchmark { RandomString = "OldString" };
            _mockBenchmarkRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(benchmark);
            _mockStringHelper.Setup(s => s.GenerateRandomString(It.IsAny<int>())).Returns("NewString");

            // Act
            var result = await _service.RunSelectPlusUpdatePlusInsertionBenchmark(numRegistries, numThreads);

            // Assert
            result.Should().BeGreaterThan(0); // Verifica que el tiempo transcurrido sea mayor a 0
            _mockBenchmarkRepository.Verify(r => r.UpdateAsync(It.Is<Benchmark>(b => b.RandomString == "NewString")), Times.Exactly(numRegistries * numThreads));
            _mockBenchmarkRepository.Verify(r => r.InsertAsync(It.IsAny<Benchmark>()), Times.Exactly(numRegistries * numThreads));
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Exactly(numRegistries * numThreads));
        }

    }
}
