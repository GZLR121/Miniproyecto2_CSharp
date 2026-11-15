// Autor: Gael Zair Limon Rdz
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using SistemaLogisticaAuditoria.Dominio;

namespace SistemaLogisticaAuditoria.Servicios.Persistencia
{
    /// <summary>
    /// Servicio de persistencia encargado de la lectura y escritura del archivo appsettings.json (RF1)
    /// utilizando la biblioteca estándar System.Text.Json y bloques using de liberación de recursos.
    /// </summary>
    public class ServicioConfiguracionJson
    {
        private readonly string _rutaArchivoJson;
        private bool _existiaArchivoAlIniciar;

        public ServicioConfiguracionJson(string rutaArchivoJson)
        {
            _rutaArchivoJson = string.IsNullOrWhiteSpace(rutaArchivoJson)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json")
                : rutaArchivoJson;
        }

        public string RutaArchivoJson => _rutaArchivoJson;

        public bool ExistiaArchivoAlIniciar => _existiaArchivoAlIniciar;

        public ConfiguracionTarifas CargarConfiguracion()
        {
            _existiaArchivoAlIniciar = File.Exists(_rutaArchivoJson);

            if (!_existiaArchivoAlIniciar)
            {
                ConfiguracionTarifas porDefecto = new ConfiguracionTarifas();
                GuardarConfiguracion(porDefecto);
                return porDefecto;
            }

            try
            {
                using (FileStream fs = new FileStream(_rutaArchivoJson, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    string contenidoJson = sr.ReadToEnd();
                    var opciones = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        ReadCommentHandling = JsonCommentHandling.Skip
                    };

                    ConfiguracionTarifas? config = JsonSerializer.Deserialize<ConfiguracionTarifas>(contenidoJson, opciones);
                    return config ?? new ConfiguracionTarifas();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error al leer o interpretar el archivo JSON '{_rutaArchivoJson}': {ex.Message}", ex);
            }
        }

        public void GuardarConfiguracion(ConfiguracionTarifas configuracion)
        {
            if (configuracion == null)
                throw new ArgumentNullException(nameof(configuracion));

            var opciones = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            string json = JsonSerializer.Serialize(configuracion, opciones);
            using (FileStream fs = new FileStream(_rutaArchivoJson, FileMode.Create, FileAccess.Write, FileShare.None))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
            {
                sw.Write(json);
            }
        }
    }
}
