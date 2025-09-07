# Nexo - Hub de Emuladores Personalizado

![Nexo](https://img.shields.io/badge/Estado-Desarrollo%20Activo-brightgreen)
![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![Avalonia](https://img.shields.io/badge/Avalonia-11.1.0-blue)

Nexo es una aplicación hub que centraliza y gestiona múltiples
emuladores de videojuegos desde una interfaz unificada.

## 🚀 Características Actuales

-   **Interfaz Unificada**: Gestión centralizada de emuladores
-   **Sistema CRUD**: Añadir, editar y eliminar emuladores
-   **Diseño Moderno**: Interfaz con modo oscuro
-   **Base de Datos**: Almacenamiento de configuración de emuladores

## 📋 Requisitos Actuales

### Essentials

-   **.NET 9.0 SDK**
    ([Descargar](https://dotnet.microsoft.com/download/dotnet/9.0))
-   **Sistema Operativo**: Windows, Linux o macOS
-   **RAM**: 2 GB mínimo
-   **Espacio**: 100 MB libres

## ⚡ Cómo Ejecutar

### Desde Código Fuente

``` bash
# Clonar repositorio
git clone https://github.com/tu-usuario/nexo.git
cd nexo

# Restaurar dependencias
dotnet restore

# Compilar proyecto
dotnet build

# Ejecutar aplicación
dotnet run
```

### Ejecutar Directamente

``` bash
# Navegar a la carpeta de compilación
cd bin/Debug/net9.0/

# Ejecutar la aplicación
./Nexo
```

## 🛠️ Tecnologías Utilizadas

-   **Avalonia UI 11.1.0** - Framework UI multiplataforma\
-   **.NET 9.0** - Runtime y SDK\
-   **ReactiveUI** - Patrón MVVM y programación reactiva\
-   **C# 12** - Lenguaje de programación principal

## 📁 Estructura del Proyecto

    nexo/
    ├── Assets/           # Recursos (imágenes, iconos)
    ├── Views/            # Vistas XAML
    ├── ViewModels/       # Lógica de presentación
    ├── Models/           # Modelos de datos
    ├── Services/         # Servicios auxiliares
    └── Nexo.csproj       # Configuración del proyecto

## 🎮 Configuración Básica

### Añadir un Emulador

1.  Haz clic en "Añadir" en la sección de Emuladores\
2.  Proporciona nombre y ruta del ejecutable\
3.  Configura extensiones asociadas\
4.  Guarda los cambios

### Emuladores Probados

✅ Cualquier emulador con ejecutable\
✅ Soporte para argumentos personalizados\
✅ Configuración individual por emulador

## ⚠️ Estado del Proyecto

🚧 En Desarrollo Activo

Nexo se encuentra actualmente en fase de desarrollo. Las características
incluyen:

✅ Interfaz principal funcional\
✅ CRUD de emuladores básico\
✅ Sistema de binding de datos

🚧 Gestión de juegos (próximamente)\
🚧 Scraping de metadatos (próximamente)\
🚧 Sistema de temas completo (próximamente)

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo LICENSE para más
detalles.

¡Gracias por probar Nexo! 🎮✨

Proyecto en desarrollo - Próximamente más características