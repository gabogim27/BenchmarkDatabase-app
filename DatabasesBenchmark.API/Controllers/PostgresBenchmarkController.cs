using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Services.Implementations;
using DatabasesBenchmark.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatabasesBenchmark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostgresBenchmarkController : ControllerBase
    {
        private readonly IBenchmarkService _benchmarkService;
        private readonly int _maxThreads;

        public PostgresBenchmarkController(IBenchmarkService benchmarkService, IConfiguration configuration)
        {
            _benchmarkService = benchmarkService;
            (_benchmarkService as BenchmarkService)?.SetDatabaseProvider(DatabaseProvider.PostgreSQL);
            _maxThreads = configuration.GetValue<int>("BenchmarkSettings:MaxThreads");
        }

        /// <summary>
        /// Handles HTTP POST requests to perform an insertion benchmark on the PostgreSQL database.
        /// </summary>
        /// <param name="numRegistries">The number of records to insert into the database.</param>
        /// <param name="numThreads">The number of concurrent threads to use for the insertion benchmark.</param>
        /// <returns>An IActionResult representing the result of the benchmark operation.</returns>
        [HttpPost("Insertion")]
        public async Task<IActionResult> PostgresInsertion(int numRegistries, int numThreads)
        {
            if (!ValidateParameters(numRegistries, numThreads, out var validationError))
            {
                return BadRequest(validationError);
            }

            var result = await _benchmarkService.RunInsertionBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        /// <summary>
        /// Handles HTTP POST requests to perform a select and update benchmark on the PostgreSQL database.
        /// </summary>
        /// <param name="numRegistries">The number of records to select and update in the database.</param>
        /// <param name="numThreads">The number of concurrent threads to use for the select and update benchmark.</param>
        /// <returns>An IActionResult representing the result of the benchmark operation.</returns>
        [HttpPost("SelectPlusUpdate")]
        public async Task<IActionResult> PostgresSelectPlusUpdate(int numRegistries, int numThreads)
        {
            if (!ValidateParameters(numRegistries, numThreads, out var validationError))
            {
                return BadRequest(validationError);
            }

            var result = await _benchmarkService.RunSelectPlusUpdateBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        /// <summary>
        /// Handles HTTP POST requests to perform a select, update, and insertion benchmark on the PostgreSQL database.
        /// </summary>
        /// <param name="numRegistries">The number of records to select, update, and insert into the database.</param>
        /// <param name="numThreads">The number of concurrent threads to use for the select, update, and insertion benchmark.</param>
        /// <returns>An IActionResult representing the result of the benchmark operation.</returns>
        [HttpPost("SelectPlusUpdatePlusInsertion")]
        public async Task<IActionResult> PostgresSelectPlusUpdatePlusInsertion(int numRegistries, int numThreads)
        {
            if (!ValidateParameters(numRegistries, numThreads, out var validationError))
            {
                return BadRequest(validationError);
            }

            var result = await _benchmarkService.RunSelectPlusUpdatePlusInsertionBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        /// <summary>
        /// Validates the input parameters for the benchmark operation.
        /// Ensures that the number of registries is greater than 0 and that the number of threads
        /// is within the acceptable range (greater than 0 and less than or equal to the maximum allowed threads).
        /// </summary>
        /// <param name="numRegistries">The number of registries to be processed.</param>
        /// <param name="numThreads">The number of threads to be used.</param>
        /// <param name="validationError">Outputs an error message if the validation fails; otherwise, null.</param>
        /// <returns>Returns true if the parameters are valid; otherwise, returns false.</returns>
        private bool ValidateParameters(int numRegistries, int numThreads, out string validationError)
        {
            if (numRegistries <= 0)
            {
                validationError = "numRegistries must be greater than 0.";
                return false;
            }

            if (numThreads <= 0 || numThreads > _maxThreads)
            {
                validationError = $"numThreads must be greater than 0 and less than or equal to {_maxThreads}.";
                return false;
            }

            validationError = null;
            return true;
        }
    }
}
