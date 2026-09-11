CREATE DATABASE CursosDb;
GO

USE CursosDb;
GO

/* =========================================================
   ROLES
   ========================================================= */

CREATE TABLE Roles
(
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT UQ_Roles_Name UNIQUE (Name)
);
GO


/* =========================================================
   USUARIOS
   ========================================================= */

CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,

    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,

    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL,

    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT UQ_Users_Email UNIQUE (Email)
);
GO


/* =========================================================
   USUARIOS - ROLES
   ========================================================= */

CREATE TABLE UserRoles
(
    UserId INT NOT NULL,
    RoleId INT NOT NULL,

    AssignedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT PK_UserRoles
        PRIMARY KEY (UserId, RoleId),

    CONSTRAINT FK_UserRoles_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_UserRoles_Roles
        FOREIGN KEY (RoleId)
        REFERENCES Roles(RoleId)
);
GO


/* =========================================================
   CATEGORÍAS
   ========================================================= */

CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,

    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    IsActive BIT NOT NULL DEFAULT 1,

    CONSTRAINT UQ_Categories_Name UNIQUE (Name)
);
GO


/* =========================================================
   CURSOS
   ========================================================= */

CREATE TABLE Courses
(
    CourseId INT IDENTITY(1,1) PRIMARY KEY,

    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,

    CategoryId INT NOT NULL,

    CreatedByUserId INT NOT NULL,

    ThumbnailUrl NVARCHAR(500) NULL,

    Level NVARCHAR(50) NULL,

    Price DECIMAL(10,2) NOT NULL DEFAULT 0,

    IsPublished BIT NOT NULL DEFAULT 0,
    IsActive BIT NOT NULL DEFAULT 1,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Courses_Categories
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId),

    CONSTRAINT FK_Courses_CreatedByUser
        FOREIGN KEY (CreatedByUserId)
        REFERENCES Users(UserId),

    CONSTRAINT CK_Courses_Price
        CHECK (Price >= 0)
);
GO


/* =========================================================
   MÓDULOS / SECCIONES DEL CURSO
   ========================================================= */

CREATE TABLE CourseSections
(
    SectionId INT IDENTITY(1,1) PRIMARY KEY,

    CourseId INT NOT NULL,

    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500) NULL,

    SectionOrder INT NOT NULL,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CONSTRAINT FK_CourseSections_Courses
        FOREIGN KEY (CourseId)
        REFERENCES Courses(CourseId),

    CONSTRAINT CK_CourseSections_Order
        CHECK (SectionOrder > 0)
);
GO


/* =========================================================
   LECCIONES
   ========================================================= */

CREATE TABLE Lessons
(
    LessonId INT IDENTITY(1,1) PRIMARY KEY,

    SectionId INT NOT NULL,

    Title NVARCHAR(200) NOT NULL,

    Description NVARCHAR(1000) NULL,

    ContentType NVARCHAR(50) NOT NULL,

    ContentUrl NVARCHAR(1000) NULL,

    LessonOrder INT NOT NULL,

    DurationMinutes INT NULL,

    IsPreview BIT NOT NULL DEFAULT 0,

    CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UpdatedAt DATETIME2 NULL,

    CONSTRAINT FK_Lessons_CourseSections
        FOREIGN KEY (SectionId)
        REFERENCES CourseSections(SectionId),

    CONSTRAINT CK_Lessons_Order
        CHECK (LessonOrder > 0),

    CONSTRAINT CK_Lessons_Duration
        CHECK (DurationMinutes IS NULL OR DurationMinutes >= 0)
);
GO


/* =========================================================
   INSCRIPCIONES
   ========================================================= */

CREATE TABLE Enrollments
(
    EnrollmentId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,
    CourseId INT NOT NULL,

    EnrollmentDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),

    CompletionPercentage DECIMAL(5,2) NOT NULL DEFAULT 0,

    CompletedAt DATETIME2 NULL,

    CONSTRAINT FK_Enrollments_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Enrollments_Courses
        FOREIGN KEY (CourseId)
        REFERENCES Courses(CourseId),

    CONSTRAINT UQ_Enrollments_User_Course
        UNIQUE (UserId, CourseId),

    CONSTRAINT CK_Enrollments_Completion
        CHECK (CompletionPercentage >= 0
               AND CompletionPercentage <= 100)
);
GO


/* =========================================================
   PROGRESO DE LECCIONES
   ========================================================= */

CREATE TABLE LessonProgress
(
    LessonProgressId INT IDENTITY(1,1) PRIMARY KEY,

    UserId INT NOT NULL,
    LessonId INT NOT NULL,

    IsCompleted BIT NOT NULL DEFAULT 0,

    ProgressPercentage DECIMAL(5,2) NOT NULL DEFAULT 0,

    LastAccessedAt DATETIME2 NULL,
    CompletedAt DATETIME2 NULL,

    CONSTRAINT FK_LessonProgress_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_LessonProgress_Lessons
        FOREIGN KEY (LessonId)
        REFERENCES Lessons(LessonId),

    CONSTRAINT UQ_LessonProgress_User_Lesson
        UNIQUE (UserId, LessonId),

    CONSTRAINT CK_LessonProgress_Percentage
        CHECK (ProgressPercentage >= 0
               AND ProgressPercentage <= 100)
);
GO


INSERT INTO Roles (Name, Description)
VALUES
(
    'Admin',
    'Administrador de la plataforma'
),
(
    'User',
    'Usuario que puede inscribirse y realizar cursos'
);
GO

INSERT INTO Roles (Name, Description)
VALUES
('Instructor', 'Creador y administrador de cursos'),
('Moderator', 'Moderador de contenido'),
('Editor', 'Editor de cursos y contenido');

INSERT INTO Categories (Name, Description)
VALUES
('Programación', 'Cursos relacionados con programación y desarrollo de software'),
('Ciberseguridad', 'Cursos relacionados con seguridad informática'),
('Bases de Datos', 'Cursos de SQL, NoSQL y administración de bases de datos'),
('Diseño', 'Cursos de diseño gráfico y UI/UX'),
('Marketing', 'Cursos de marketing digital y estrategias comerciales'),
('Idiomas', 'Cursos para aprender diferentes idiomas');
GO

select * from Roles