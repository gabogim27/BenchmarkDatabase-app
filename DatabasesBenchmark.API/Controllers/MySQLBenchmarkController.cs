using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Services.Implementations;
using DatabasesBenchmark.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabasesBenchmark.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MySQLBenchmarkController : ControllerBase
    {
        private readonly IBenchmarkService _benchmarkService;
        private readonly int _maxThreads;

        public MySQLBenchmarkController(IBenchmarkService benchmarkService, IConfiguration configuration)
        {
            _benchmarkService = benchmarkService;
            (_benchmarkService as BenchmarkService)?.SetDatabaseProvider(DatabaseProvider.MySQL);
            _maxThreads = configuration.GetValue<int>("BenchmarkSettings:MaxThreads");
        }

        /// <summary>
        /// Handles HTTP POST requests to perform an insertion benchmark test on a MySQL database.
        /// </summary>
        /// <param name="numRegistries">The number of records to insert during the benchmark. Must be greater than zero.</param>
        /// <param name="numThreads">The number of threads to use during the benchmark. Must be greater than zero.</param>
        /// <returns>
        /// An HTTP 200 OK response with the benchmark result if the parameters are valid.
        /// An HTTP 400 Bad Request response if the parameters are invalid.
        /// </returns>
        [HttpPost("Insertion")]
        public async Task<IActionResult> MySQLInsertion(int numRegistries, int numThreads)
        {
            if (!ValidateParameters(numRegistries, numThreads, out var validationError))
            {
                return BadRequest(validationError);
            }

            var result = await _benchmarkService.RunInsertionBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        /// <summary>
        /// Handles HTTP POST requests to perform a select and update benchmark test on a MySQL database.
        /// </summary>
        /// <param name="numRegistries">The number of records to select and update during the benchmark. Must be greater than zero.</param>
        /// <param name="numThreads">The number of threads to use during the benchmark. Must be greater than zero.</param>
        /// <returns>
        /// An HTTP 200 OK response with the benchmark result if the parameters are valid.
        /// An HTTP 400 Bad Request response if the parameters are invalid.
        /// </returns>
        [HttpPost("SelectPlusUpdate")]
        public async Task<IActionResult> MySQLSelectPlusUpdate(int numRegistries, int numThreads)
        {
            if (!ValidateParameters(numRegistries, numThreads, out var validationError))
            {
                return BadRequest(validationError);
            }

            var result = await _benchmarkService.RunSelectPlusUpdateBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        /// <summary>
        /// Handles HTTP POST requests to perform a benchmark test involving selection, update, and insertion operations on a MySQL database.
        /// </summary>
        /// <param name="numRegistries">The number of records to select, update, and insert during the benchmark. Must be greater than zero.</param>
        /// <param name="numThreads">The number of threads to use during the benchmark. Must be greater than zero.</param>
        /// <returns>
        /// An HTTP 200 OK response with the benchmark result if the parameters are valid.
        /// An HTTP 400 Bad Request response if the parameters are invalid.
        /// </returns>
        [HttpPost("SelectPlusUpdatePlusInsertion")]
        public async Task<IActionResult> MySQLSelectPlusUpdatePlusInsertion(int numRegistries, int numThreads)
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
