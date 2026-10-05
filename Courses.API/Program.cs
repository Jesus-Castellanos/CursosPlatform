using Courses.API.Handlers;
using Courses.Application.Interfaces;
using Courses.Application.Security;
using Courses.Application.Services;
using Courses.Infrastructure.Data;
using Courses.Infrastructure.Repositories;
using Courses.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// ======================================================
// MVC / API
// ======================================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT. Ejemplo: Bearer {token}"
    });

    options.AddSecurityRequirement(document =>
        new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] =
                []
        });
});

// ======================================================
// DATABASE
// ======================================================

builder.Services.AddDbContext<CoursesDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// ======================================================
// REPOSITORIES + SERVICES
// ======================================================

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICoursesService, CoursesService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Sistema anterior de hashing.
// Se mantiene porque ya forma parte del CRUD de usuarios.
builder.Services.AddScoped<
    IPasswordHasherServices,
    Argon2PasswordHasherServices>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleServices, RolesService>();

builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRolesService, UserRolesService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<ILessonsService, LessonsService>();

builder.Services.AddScoped<
    ICourseSectionRepository,
    CourseSectionRepository>();

builder.Services.AddScoped<
    ICourseSectionsService,
    CourseSectionsService>();

builder.Services.AddScoped<
    IEnrollmentRepository,
    EnrollmentRepository>();

builder.Services.AddScoped<
    IEnrollmentsService,
    EnrollmentsService>();

builder.Services.AddScoped<
    ILessonProgressRepository,
    LessonProgressRepository>();

builder.Services.AddScoped<
    ILessonProgressService,
    LessonProgressService>();

// ======================================================
// AUTHENTICATION SERVICES
// ======================================================

builder.Services.AddScoped<
    IPasswordHasher,
    Argon2PasswordHasher>();

builder.Services.AddScoped<
    IJwtTokenService,
    JwtTokenService>();

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

// ======================================================
// CORS
// ======================================================

var allowedOrigins = builder.Configuration
    .GetSection("AllowedOrigins")
    .Get<string[]>()
    ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ======================================================
// JWT AUTHENTICATION
// ======================================================

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var issuer =
            builder.Configuration["Jwt:Issuer"];

        var audience =
            builder.Configuration["Jwt:Audience"];

        var publicKey =
            builder.Configuration["Jwt:PublicKey"];

        var keyId =
            builder.Configuration["Jwt:KeyId"];

        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new InvalidOperationException(
                "No está configurado Jwt:Issuer.");
        }

        if (string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException(
                "No está configurado Jwt:Audience.");
        }

        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new InvalidOperationException(
                "No está configurado Jwt:PublicKey.");
        }

        if (string.IsNullOrWhiteSpace(keyId))
        {
            throw new InvalidOperationException(
                "No está configurado Jwt:KeyId.");
        }

        using var rsa = RSA.Create();

        rsa.ImportFromPem(publicKey);

        var rsaParameters =
            rsa.ExportParameters(false);

        var signingKey =
            new RsaSecurityKey(rsaParameters)
            {
                KeyId = keyId
            };

        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                // ------------------------------------------
                // ISSUER
                // ------------------------------------------

                ValidateIssuer = true,
                ValidIssuer = issuer,

                // ------------------------------------------
                // AUDIENCE
                // ------------------------------------------

                ValidateAudience = true,
                ValidAudience = audience,

                // ------------------------------------------
                // EXPIRATION
                // ------------------------------------------

                ValidateLifetime = true,

                // ------------------------------------------
                // SIGNATURE
                // ------------------------------------------

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // ------------------------------------------
                // ALGORITHM
                // ------------------------------------------

                ValidAlgorithms = new[]
                {
                    SecurityAlgorithms.RsaSsaPssSha512
                },

                // ------------------------------------------
                // CLAIMS
                // ------------------------------------------

                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role,

                // ------------------------------------------
                // CLOCK SKEW
                // ------------------------------------------

                ClockSkew =
                    TimeSpan.FromSeconds(30)
            };
    });

// ======================================================
// AUTHORIZATION
// ======================================================

builder.Services.AddAuthorization();

// ======================================================
// BUILD APP
// ======================================================

var app = builder.Build();

// ======================================================
// SWAGGER
// ======================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

// ======================================================
// GLOBAL EXCEPTION HANDLER
// ======================================================

app.UseExceptionHandler();

// ======================================================
// HTTPS
// ======================================================

app.UseHttpsRedirection();

// ======================================================
// CORS
// ======================================================

app.UseCors("Frontend");

// ======================================================
// AUTHENTICATION
// ======================================================

app.UseAuthentication();

// ======================================================
// AUTHORIZATION
// ======================================================

app.UseAuthorization();

// ======================================================
// CONTROLLERS
// ======================================================

app.MapControllers();

app.Run();