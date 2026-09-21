# CursosPlatform - Plataforma de Gestión de Cursos

Una aplicación completa de gestión de cursos online desarrollada con arquitectura de microservicios, utilizando **ASP.NET Core** para el backend y **Angular** para el frontend.

## 📋 Tabla de Contenidos

- [Características](#características)
- [Arquitectura del Proyecto](#arquitectura-del-proyecto)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Configuración del Entorno](#configuración-del-entorno)
- [Instalación y Ejecución](#instalación-y-ejecución)
- [API Endpoints](#api-endpoints)
- [Modelo de Datos](#modelo-de-datos)
- [Funcionalidades Implementadas](#funcionalidades-implementadas)

## ✨ Características

- **Gestión de Usuarios**: Sistema completo de usuarios con roles y permisos
- **Gestión de Cursos**: CRUD completo para cursos con categorías
- **Gestión de Categorías**: Organización de cursos por categorías
- **Secciones de Cursos**: Estructura de cursos en secciones y lecciones
- **Sistema de Inscripciones**: Gestión de matrículas de usuarios en cursos
- **Seguridad**: Hash de contraseñas con Argon2
- **API REST**: API RESTful documentada con Swagger
- **Frontend Angular**: Interfaz de usuario moderna con Angular 22
- **Base de Datos Relacional**: SQL Server con Entity Framework Core
- **Arquitectura en Capas**: Separación clara de responsabilidades

## 🏗️ Arquitectura del Proyecto

El proyecto sigue una arquitectura de **N-Capas** con principios SOLID y Clean Architecture:

```
┌─────────────────────────────────────────────────────────────┐
│                     Courses-web (Angular)                    │
│                    Frontend Application                       │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                     Courses.API                              │
│                   API REST / Controllers                      │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                  Courses.Application                         │
│            Lógica de Negocio / Services / DTOs                │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   Courses.Domain                             │
│              Entidades del Dominio / Models                   │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                Courses.Infrastructure                         │
│           Acceso a Datos / Repositories / DbContext           │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   SQL Server Database                         │
└─────────────────────────────────────────────────────────────┘
```

## 🛠️ Tecnologías Utilizadas

### Backend
- **.NET 10.0**: Framework principal
- **ASP.NET Core**: Framework para la API REST
- **Entity Framework Core 10.0.12**: ORM para acceso a datos
- **SQL Server**: Base de datos relacional
- **Swashbuckle.AspNetCore 10.2.3**: Documentación de API con Swagger
- **Argon2**: Algoritmo de hash para contraseñas

### Frontend
- **Angular 22.1.0**: Framework frontend
- **TypeScript 6.0.2**: Lenguaje de programación
- **RxJS 7.8.0**: Programación reactiva
- **Angular CLI 22.1.7**: Herramientas de línea de comandos

### Desarrollo
- **Visual Studio / VS Code**: IDE de desarrollo
- **Git**: Control de versiones

## 📁 Estructura del Proyecto

```
CursosPlatform/
├── Courses-web/                    # Frontend Angular
│   ├── src/
│   │   ├── app/
│   │   │   ├── app.html          # Componente principal
│   │   │   ├── app.ts            # Lógica del componente
│   │   │   ├── app.module.ts     # Módulo principal
│   │   │   └── app-routing-module.ts # Configuración de rutas
│   │   ├── main.ts               # Punto de entrada
│   │   └── index.html            # HTML principal
│   ├── package.json              # Dependencias de Node.js
│   ├── angular.json              # Configuración de Angular
│   └── tsconfig.json             # Configuración de TypeScript
│
├── Courses.API/                   # API REST
│   ├── Controllers/              # Controladores API
│   │   ├── CategoriesController.cs
│   │   ├── CourseControllers.cs
│   │   ├── CourseSectionsController.cs
│   │   ├── RoleControllers.cs
│   │   ├── UserControllers.cs
│   │   └── UserRolesController.cs
│   ├── Program.cs                # Configuración de la aplicación
│   ├── appsettings.json          # Configuración de la aplicación
│   └── Courses.API.csproj        # Proyecto .NET
│
├── Courses.Application/           # Lógica de Negocio
│   ├── Services/                 # Servicios de lógica de negocio
│   │   ├── CategoriesService.cs
│   │   ├── CoursesServices.cs
│   │   ├── CourseSectionsService.cs
│   │   ├── RoleService.cs
│   │   ├── UserRolesServices.cs
│   │   ├── UserServices.cs
│   │   └── Argon2PasswordHasherService.cs
│   ├── DTOs/                     # Data Transfer Objects
│   │   ├── Categories/
│   │   ├── Courses/
│   │   ├── CourseSections/
│   │   ├── Roles/
│   │   ├── UserRoles/
│   │   └── Users/
│   ├── Interfaces/              # Interfaces de servicios y repositorios
│   └── Courses.Application.csproj
│
├── Courses.Domain/                # Entidades del Dominio
│   ├── Entities/                 # Entidades del modelo de datos
│   │   ├── Category.cs
│   │   ├── Course.cs
│   │   ├── CourseSection.cs
│   │   ├── Enrollment.cs
│   │   ├── Lesson.cs
│   │   ├── LessonProgress.cs
│   │   ├── Role.cs
│   │   ├── User.cs
│   │   └── UserRole.cs
│   └── Courses.Domain.csproj
│
├── Courses.Infrastructure/        # Acceso a Datos
│   ├── Data/                     # Configuración de base de datos
│   │   ├── CoursesDbContext.cs   # Contexto de Entity Framework
│   │   ├── Configurations/       # Configuraciones de entidades
│   │   └── Migrations/           # Migraciones de base de datos
│   ├── Repositories/             # Implementación de repositorios
│   │   ├── CategoryRepository.cs
│   │   ├── CourseRepository.cs
│   │   ├── CourseSectionRepository.cs
│   │   ├── RoleRepository.cs
│   │   ├── UserRepository.cs
│   │   └── UserRoleRepository.cs
│   └── Courses.Infrastructure.csproj
│
├── DB/                            # Scripts de base de datos
│   └── SQLQuery1.sql
│
├── CursosPlatform.slnx           # Solución de Visual Studio
├── README.md                     # Este archivo
└── .gitignore                    # Archivos ignorados por Git
```

## ⚙️ Configuración del Entorno

### Requisitos Previos

- **.NET 10.0 SDK**: [Descargar aquí](https://dotnet.microsoft.com/download)
- **Node.js 18+**: [Descargar aquí](https://nodejs.org/)
- **SQL Server**: [Descargar aquí](https://www.microsoft.com/sql-server)
- **Visual Studio 2022** o **VS Code**: [Descargar VS Code](https://code.visualstudio.com/)

### Configuración de Base de Datos

1. Crear una base de datos en SQL Server llamada `CoursesDB`
2. Configurar la cadena de conexión en `Courses.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tu_servidor;Database=CoursesDB;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "AllowedOrigins": ["http://localhost:4200"]
}
```

3. Ejecutar las migraciones de Entity Framework:

```bash
cd Courses.API
dotnet ef database update
```

## 🚀 Instalación y Ejecución

### Backend (ASP.NET Core API)

1. Navegar al directorio de la API:
```bash
cd Courses.API
```

2. Restaurar dependencias:
```bash
dotnet restore
```

3. Ejecutar la API:
```bash
dotnet run
```

La API estará disponible en `https://localhost:7000` (o el puerto configurado)

### Frontend (Angular)

1. Navegar al directorio del frontend:
```bash
cd Courses-web
```

2. Instalar dependencias:
```bash
npm install
```

3. Ejecutar el servidor de desarrollo:
```bash
npm start
```

La aplicación estará disponible en `http://localhost:4200`

### Documentación de la API

Una vez ejecutada la API, puedes acceder a la documentación de Swagger en:
```
https://localhost:7000/swagger
```

## 🌐 API Endpoints

### Categorías
- `GET /api/Categories` - Obtener todas las categorías
- `GET /api/Categories/{id}` - Obtener una categoría por ID
- `POST /api/Categories` - Crear una nueva categoría
- `PUT /api/Categories/{id}` - Actualizar una categoría
- `DELETE /api/Categories/{id}` - Eliminar una categoría

### Cursos
- `GET /api/Courses` - Obtener todos los cursos
- `GET /api/Courses/{id}` - Obtener un curso por ID
- `POST /api/Courses` - Crear un nuevo curso
- `PUT /api/Courses/{id}` - Actualizar un curso
- `DELETE /api/Courses/{id}` - Eliminar un curso

### Secciones de Cursos
- `GET /api/CourseSections` - Obtener todas las secciones
- `GET /api/CourseSections/{id}` - Obtener una sección por ID
- `POST /api/CourseSections` - Crear una nueva sección
- `PUT /api/CourseSections/{id}` - Actualizar una sección
- `DELETE /api/CourseSections/{id}` - Eliminar una sección

### Usuarios
- `GET /api/Users` - Obtener todos los usuarios
- `GET /api/Users/{id}` - Obtener un usuario por ID
- `POST /api/Users` - Crear un nuevo usuario
- `PUT /api/Users/{id}` - Actualizar un usuario
- `DELETE /api/Users/{id}` - Eliminar un usuario

### Roles
- `GET /api/Roles` - Obtener todos los roles
- `GET /api/Roles/{id}` - Obtener un rol por ID
- `POST /api/Roles` - Crear un nuevo rol
- `PUT /api/Roles/{id}` - Actualizar un rol
- `DELETE /api/Roles/{id}` - Eliminar un rol

### Roles de Usuario
- `GET /api/UserRoles` - Obtener todas las asignaciones de roles
- `GET /api/UserRoles/{id}` - Obtener una asignación por ID
- `POST /api/UserRoles` - Asignar un rol a un usuario
- `PUT /api/UserRoles/{id}` - Actualizar una asignación
- `DELETE /api/UserRoles/{id}` - Eliminar una asignación

## 🗄️ Modelo de Datos

### Entidades Principales

#### User
- `Id`: Identificador del usuario
- `FirstName`: Nombre del usuario
- `LastName`: Apellido del usuario
- `Email`: Correo electrónico
- `PasswordHash`: Hash de la contraseña (Argon2)
- `CreatedAt`: Fecha de creación
- `UpdatedAt`: Fecha de actualización
- `IsActive`: Estado del usuario

#### Role
- `RoleId`: Identificador del rol
- `Name`: Nombre del rol
- `Description`: Descripción del rol

#### UserRole
- `UserRoleId`: Identificador de la relación
- `UserId`: ID del usuario
- `RoleId`: ID del rol
- `AssignedAt`: Fecha de asignación

#### Category
- `CategoryId`: Identificador de la categoría
- `Name`: Nombre de la categoría
- `Description`: Descripción de la categoría
- `CreatedAt`: Fecha de creación
- `IsActive`: Estado de la categoría

#### Course
- `CourseId`: Identificador del curso
- `Title`: Título del curso
- `Description`: Descripción del curso
- `CategoryId`: ID de la categoría
- `CreatedByUserId`: ID del usuario creador
- `ThumbnailUrl`: URL de la imagen del curso
- `Level`: Nivel del curso
- `Price`: Precio del curso
- `IsPublished`: Estado de publicación
- `IsActive`: Estado del curso
- `CreatedAt`: Fecha de creación
- `UpdatedAt`: Fecha de actualización

#### CourseSection
- `SectionId`: Identificador de la sección
- `CourseId`: ID del curso
- `Title`: Título de la sección
- `Description`: Descripción de la sección
- `OrderNumber`: Orden de la sección
- `CreatedAt`: Fecha de creación

#### Lesson
- `LessonId`: Identificador de la lección
- `SectionId`: ID de la sección
- `Title`: Título de la lección
- `Content`: Contenido de la lección
- `VideoUrl`: URL del video
- `Duration`: Duración en minutos
- `OrderNumber`: Orden de la lección

#### Enrollment
- `EnrollmentId`: Identificador de la inscripción
- `CourseId`: ID del curso
- `UserId`: ID del usuario
- `EnrolledAt`: Fecha de inscripción
- `CompletedAt`: Fecha de completado

#### LessonProgress
- `ProgressId`: Identificador del progreso
- `LessonId`: ID de la lección
- `UserId`: ID del usuario
- `IsCompleted`: Estado de completado
- `CompletedAt`: Fecha de completado

## 🎯 Funcionalidades Implementadas

### ✅ Backend (ASP.NET Core)

#### Gestión de Usuarios
- Sistema de autenticación con hash de contraseñas usando Argon2
- Gestión completa de usuarios (CRUD)
- Validación de datos de entrada
- Manejo de errores y excepciones

#### Gestión de Roles y Permisos
- Sistema de roles para usuarios
- Asignación de múltiples roles por usuario
- Gestión de roles (CRUD)
- Relación muchos-a-muchos entre usuarios y roles

#### Gestión de Categorías
- Organización de cursos por categorías
- Validación de nombres únicos
- Soft delete (desactivación en lugar de eliminación)
- Gestión completa de categorías (CRUD)

#### Gestión de Cursos
- Creación y gestión de cursos completos
- Asignación a categorías
- Control de publicación (IsPublished)
- Gestión de precios y niveles
- Relación con usuarios creadores
- Soft delete de cursos

#### Gestión de Secciones de Cursos
- Estructura de cursos en secciones
- Ordenamiento de secciones
- Relación con cursos
- Gestión completa de secciones (CRUD)

#### Configuración de la API
- Configuración de CORS para comunicación con Angular
- Documentación automática con Swagger
- Inyección de dependencias
- Configuración de Entity Framework Core
- Manejo de conexiones a base de datos

### ✅ Frontend (Angular)

#### Aplicación Base
- Proyecto Angular 22 configurado
- Sistema de routing básico
- Componente principal con diseño moderno
- Configuración de TypeScript
- Sistema de estilos CSS moderno

#### Estructura del Proyecto
- Módulos organizados correctamente
- Configuración de Angular CLI
- Sistema de componentes
- Servicios HTTP preparados para consumo de API

## 🔐 Seguridad

- **Hash de Contraseñas**: Implementación de Argon2 para hash seguro de contraseñas
- **CORS**: Configuración de Cross-Origin Resource Sharing para permitir comunicación segura entre frontend y backend
- **Validación de Datos**: Validación en todos los endpoints de la API
- **Manejo de Errores**: Captura y manejo adecuado de excepciones

## 📝 Notas de Desarrollo

- El proyecto utiliza **.NET 10.0** como framework principal
- La base de datos utiliza **Entity Framework Core** con migraciones
- El frontend está construido con **Angular 22** usando las últimas características
- Se sigue una arquitectura limpia con separación de responsabilidades
- La API está documentada automáticamente con Swagger

## 🚧 Próximas Mejoras

- [ ] Implementar autenticación JWT
- [ ] Agregar sistema de archivos para almacenamiento de contenido multimedia
- [ ] Implementar sistema de paginación en la API
- [ ] Agregar tests unitarios y de integración
- [ ] Implementar sistema de notificaciones
- [ ] Agregar sistema de calificaciones y reseñas
- [ ] Implementar dashboard de administración
- [ ] Agregar sistema de búsqueda y filtros avanzados

## 👤 Autor

Desarrollado como proyecto de plataforma de gestión de cursos online.

## 📄 Licencia

Este proyecto es para fines educativos y de desarrollo.
