namespace DatabasesBenchmark.Services.Interfaces
{
    public interface IBenchmarkService
    {
        /// <summary>
        /// Realiza una prueba de rendimiento para la inserción de registros en la base de datos.
        /// </summary>
        /// <param name="numRegistries">Número de registros a insertar.</param>
        /// <param name="numThreads">Número de hilos a utilizar.</param>
        /// <returns>Tiempo total en milisegundos que tomó completar la prueba.</returns>
        Task<long> RunInsertionBenchmark(int numRegistries, int numThreads);

        /// <summary>
        /// Realiza una prueba de rendimiento para la selección y actualización de registros en la base de datos.
        /// </summary>
        /// <param name="numRegistries">Número de registros a seleccionar y actualizar.</param>
        /// <param name="numThreads">Número de hilos a utilizar.</param>
        /// <returns>Tiempo total en milisegundos que tomó completar la prueba.</returns>
        Task<long> RunSelectPlusUpdateBenchmark(int numRegistries, int numThreads);

        /// <summary>
        /// Realiza una prueba de rendimiento para la selección, actualización e inserción de registros en la base de datos.
        /// </summary>
        /// <param name="numRegistries">Número de registros a seleccionar, actualizar e insertar.</param>
        /// <param name="numThreads">Número de hilos a utilizar.</param>
        /// <returns>Tiempo total en milisegundos que tomó completar la prueba.</returns>
        Task<long> RunSelectPlusUpdatePlusInsertionBenchmark(int numRegistries, int numThreads);
    }
}
