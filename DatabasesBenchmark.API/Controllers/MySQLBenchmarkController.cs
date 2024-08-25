using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Services.Implementations;
using DatabasesBenchmark.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatabasesBenchmark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MySQLBenchmarkController : ControllerBase
    {
        private readonly IBenchmarkService _benchmarkService;

        public MySQLBenchmarkController(IBenchmarkService benchmarkService)
        {
            _benchmarkService = benchmarkService;
            (_benchmarkService as BenchmarkService)?.SetDatabaseProvider(DatabaseProvider.MySQL);
        }

        [HttpPost("Insertion")]
        public async Task<IActionResult> MySQLInsertion(int numRegistries, int numThreads)
        {
            if (numRegistries <= 0 || numThreads <= 0)
            {
                return BadRequest("numRegistries and numThreads should be greater than 0.");
            }

            var result = await _benchmarkService.RunInsertionBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        [HttpPost("SelectPlusUpdate")]
        public async Task<IActionResult> MySQLSelectPlusUpdate(int numRegistries, int numThreads)
        {
            if (numRegistries <= 0 || numThreads <= 0)
            {
                return BadRequest("numRegistries and numThreads should be greater than 0.");
            }

            var result = await _benchmarkService.RunSelectPlusUpdateBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        [HttpPost("SelectPlusUpdatePlusInsertion")]
        public async Task<IActionResult> MySQLSelectPlusUpdatePlusInsertion(int numRegistries, int numThreads)
        {
            if (numRegistries <= 0 || numThreads <= 0)
            {
                return BadRequest("numRegistries and numThreads should be greater than 0.");
            }

            var result = await _benchmarkService.RunSelectPlusUpdatePlusInsertionBenchmark(numRegistries, numThreads);
            return Ok(result);
        }
    }
}
