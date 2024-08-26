﻿using DatabasesBenchmark.Domain.Enums;
using DatabasesBenchmark.Services.Implementations;
using DatabasesBenchmark.Services.Interfaces;
<<<<<<< HEAD
using Microsoft.AspNetCore.Http;
=======
>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
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
<<<<<<< HEAD
=======
            if (numRegistries <= 0 || numThreads <= 0)
            {
                return BadRequest("numRegistries and numThreads should be greater than 0.");
            }

>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
            var result = await _benchmarkService.RunInsertionBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        [HttpPost("SelectPlusUpdate")]
        public async Task<IActionResult> MySQLSelectPlusUpdate(int numRegistries, int numThreads)
        {
<<<<<<< HEAD
=======
            if (numRegistries <= 0 || numThreads <= 0)
            {
                return BadRequest("numRegistries and numThreads should be greater than 0.");
            }

>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
            var result = await _benchmarkService.RunSelectPlusUpdateBenchmark(numRegistries, numThreads);
            return Ok(result);
        }

        [HttpPost("SelectPlusUpdatePlusInsertion")]
        public async Task<IActionResult> MySQLSelectPlusUpdatePlusInsertion(int numRegistries, int numThreads)
        {
<<<<<<< HEAD
=======
            if (numRegistries <= 0 || numThreads <= 0)
            {
                return BadRequest("numRegistries and numThreads should be greater than 0.");
            }

>>>>>>> 4b81f786cd0f9757a173c02912df4cf971c60ab8
            var result = await _benchmarkService.RunSelectPlusUpdatePlusInsertionBenchmark(numRegistries, numThreads);
            return Ok(result);
        }
    }
}
