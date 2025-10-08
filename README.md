# IDEn — Indicadores de Desempeño Energético (WPF .NET)

Aplicación **WPF (MVVM)** para gestionar y analizar el **Indicador de Desempeño Energético (IDEn)** en una planta de producción. Incluye panel de inicio, análisis, captura de datos, reportes y configuración.

> Framework: **.NET 8** • UI: **WPF** • Patrón: **MVVM**

---

## Requisitos previos

- **Visual Studio 2022** (o superior) con workloads:  
  *Desktop development with .NET*
- **.NET SDK 8.0+**
- (Opcional) **SQL Server LocalDB** para EF Core

---

## Configuración del entorno

1. Clonar el repo y abrir `IDEn.sln` en Visual Studio.
2. Establecer `IDEn.App` como **Startup Project**.
3. Verificar el **target framework** (`net8.0-windows`) en cada `.csproj`.
4. Confirmar que los íconos existan en `IDEn.App/Assets/Icons/` y con **Build Action** = `Resource`.

---

## Ejecutar el proyecto

- **F5** (Debug) en Visual Studio.
- O por CLI:
  ```bash
  dotnet build IDEn.sln
  dotnet run --project IDEn.App
  ```

---

## Arquitectura y decisiones

- **MVVM estricto**: `Views` (XAML) enlazan a `ViewModels` con `DataContext`.
- **Comandos**: `AsyncRelayCommand` para operaciones asíncronas (`Execute(object?)` / `CanExecute(object?)` y `CanExecuteChanged?` nullable-safe).
- **Validación**: `DatosViewModel` implementa `INotifyDataErrorInfo` para errores en tiempo real.
- **Nulabilidad**: eventos `PropertyChanged?` y `ErrorsChanged?` marcados como `nullable` para evitar advertencias CS86xx.
- **Navegación**: simple por `Frame` en `MainWindow` (cada Page tiene su sidebar).

---

## Estilos y diseño

- Paleta usada:
  - **Fondo principal / sidebar**: `#F7FAFC`
  - **Hover/activo**: `#E8EDF2`
  - **Texto principal**: `#0D141C`
  - **Texto secundario**: `#4D7399`
- Fuente: **Inter** (opcional).  
- Sidebar **fijo**, ancho ~ **240–320 px** (en Figma: 320px).  
- Header superior fijo: navegación “Inicio / Datos / Análisis / Reportes / 2024”.

---

## Navegación

La ventana principal (`MainWindow`) hospeda un `Frame` (`MainFrame`). Cada `Page` incluye su **sidebar** con botones que llaman a métodos `GoInicio/GoDatos/...` que navegan con:

```csharp
var win = (MainWindow)Application.Current.MainWindow;
win.MainFrame.Navigate(new DatosPage());
```

---

## Datos y validación

- Entidades de dominio en **IDEn.Core** (`ProduccionMensual`, `ConsumoEnergia`).
- `DatosViewModel` utiliza `INotifyDataErrorInfo` para validar numéricos, rangos y requeridos.
- Si se configura EF Core, `IDEn.Infrastructure` aporta `IdenDbContext` para persistencia.

---

## Reportes

`ReportesViewModel` expone:
- Tipos de reporte (ej. “Producción y Consumo”, “IDE por mes”).
- Rango de fechas.
- Formatos (CSV, XLSX, PDF\*).  
  \*PDF requiere implementación opcional en `IDEn.Reports` o librería externa.

La `ReportesPage` muestra **vista previa** en `DataGrid` y comandos **Generar/Exportar**.

---

## Scripts útiles

- Restaurar paquetes y compilar:
  ```bash
  dotnet restore
  dotnet build IDEn.sln -c Release
  ```

- (EF Core) Crear migration:
  ```bash
  dotnet ef migrations add Init --project IDEn.Infrastructure --startup-project IDEn.App
  dotnet ef database update --project IDEn.Infrastructure --startup-project IDEn.App
  ```

---

## Licencia

Este proyecto se distribuye bajo la licencia **MIT**. Consulta `LICENSE` para más detalles.
