# Sistema de Procesamiento y Auditoría de Facturación (Logística y Entregas)

Aplicación de escritorio en **C# Windows Forms (`.NET 8.0-windows`)** diseñada bajo una arquitectura en capas (**Presentación**, **Dominio** y **Servicios de Lógica y Persistencia**) para el procesamiento, validación léxica/numérica, cálculo financiero escalonado, auditoría visual, persistencia transaccional en **SQLite** y exportación a **CSV** de lotes de fletes.

## Estructura en Capas del Proyecto

```text
Miniproyecto_Logistica/
├── Miniproyecto_Logistica.slnx
└── SistemaLogisticaAuditoria/
    ├── appsettings.json                         # Configuración parametrizada (RF1)
    ├── Program.cs                               # Punto de entrada de la aplicación
    ├── Form1.cs / Form1.Designer.cs             # Capa de Presentación (UI Multi-Idioma ES/EN y Eventos)
    ├── Dominio/                                 # Capa de Dominio (Modelos con encapsulamiento estricto)
    │   ├── ConfiguracionTarifas.cs
    │   ├── Paquete.cs
    │   └── ResultadoProcesamientoLote.cs
    └── Servicios/                               # Capa de Servicios
        ├── Logica/
        │   ├── ServicioCalculoFinanciero.cs     # Reto 3: Impuestos escalonados y tarifa regional
        │   ├── ServicioProcesamientoLotes.cs    # Reto 1 (Parseo tolerante a fallos) y Reto 2 (Arreglos y Burbuja)
        │   └── GestorIdioma.cs                  # RF2: Diccionario dinámico Español / Inglés
        └── Persistencia/
            ├── ServicioConfiguracionJson.cs     # RF1: Lectura/escritura de appsettings.json
            ├── ServicioExportacionCsv.cs        # RF6: Exportación a CSV con bloques using
            └── RepositorioBaseDatos.cs          # RF4: Persistencia en Base de Datos SQLite (auditoria_logistica.db)
```

## Requisitos Técnicos
* **.NET 8.0 SDK** en Windows 10/11.
* Compatible al 100% con el **Diseñador de Windows Forms de Visual Studio 2022** (`Shift + F7`).

## Repositorio y Autor
* **Autor:** Gael Zair Limon Rdz (ID: 555274 | l555274@edu.uaa.mx / .gzlr55@gmail.com)
* **Repositorio en GitHub:** https://github.com/GZLR121/Miniproyecto2_CSharp
