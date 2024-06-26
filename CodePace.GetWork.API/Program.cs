using CodePace.GetWork.API.IAM.Application.Internal.CommandServices;
using CodePace.GetWork.API.IAM.Application.Internal.OutboundServices;
using CodePace.GetWork.API.IAM.Application.Internal.QueryServices;
using CodePace.GetWork.API.IAM.Domain.Repositories;
using CodePace.GetWork.API.IAM.Domain.Services;
using CodePace.GetWork.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using CodePace.GetWork.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using CodePace.GetWork.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using CodePace.GetWork.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using CodePace.GetWork.API.IAM.Infrastructure.Tokens.JWT.Services;
using CodePace.GetWork.API.IAM.Interfaces.ACL;
using CodePace.GetWork.API.IAM.Interfaces.ACL.Services;
using CodePace.GetWork.API.Plans.Application.Internal;
using CodePace.GetWork.API.Plans.Domain.Repositories;
using CodePace.GetWork.API.Plans.Domain.Service;
using CodePace.GetWork.API.Plans.Infrastructure.Persistence.EFC;
using CodePace.GetWork.API.Profiles.Application.Internal.CommandServices;
using CodePace.GetWork.API.Profiles.Application.Internal.QueryServices;
using CodePace.GetWork.API.Profiles.Domain.Repositories;
using CodePace.GetWork.API.Profiles.Domain.Services;
using CodePace.GetWork.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using CodePace.GetWork.API.Profiles.Interfaces.ACL;
using CodePace.GetWork.API.Profiles.Interfaces.ACL.Services;
using CodePace.GetWork.API.contest.Application.Internal.CommandServices;
using CodePace.GetWork.API.contest.Application.Internal.QueryServices;
using CodePace.GetWork.API.contest.Domain.Model.Entities;
using CodePace.GetWork.API.contest.Domain.Repositories;
using CodePace.GetWork.API.contest.Domain.Services;
using CodePace.GetWork.API.contest.Infrastructure.Persistence.EFC.Repositories;
using CodePace.GetWork.API.CourseContest.Application.Internal.CommandServices;
using CodePace.GetWork.API.CourseContest.Application.Internal.QueryServices;
using CodePace.GetWork.API.CourseContest.Domain.Repositories;
using CodePace.GetWork.API.CourseContest.Domain.Services;
using CodePace.GetWork.API.CourseContest.Infrastructure.Persistence.EFC.Repositories;
using CodePace.GetWork.API.Shared.Domain.Repositories;
using CodePace.GetWork.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using CodePace.GetWork.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using CodePace.GetWork.API.Shared.Interfaces.ASP.Configuration;
using CodePace.GetWork.API.TechnicalEvaluation.Application.Internal.CommandServices;
using CodePace.GetWork.API.TechnicalEvaluation.Application.Internal.QueryServices;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Model.Aggregates;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Repositories;
using CodePace.GetWork.API.TechnicalEvaluation.Domain.Services;
using CodePace.GetWork.API.TechnicalEvaluation.Infrastructure.Persistence.EFC.Repositories;
//using CodePace.GetWork.API.TechnicalEvaluation.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using DefaultNamespace;  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configure Database Context and Logging Levels
builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        if (connectionString != null)
            if (builder.Environment.IsDevelopment())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            else if (builder.Environment.IsProduction())
                options.UseMySQL(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Error)
                    .EnableDetailedErrors();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "CodePace.GetWork.API",
                Version = "v1",
                Description = "CodePace Get Work Platform API",
                TermsOfService = new Uri("https://codepace-getwork.com/tos"),
                Contact = new OpenApiContact
                {
                    Name = "CodePace",
                    Email = "contact@codepace.com"
                },
                License = new OpenApiLicense
                {
                    Name = "Apache 2.0",
                    Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                }
            });
        c.EnableAnnotations();
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    });
// Configure Lowercase URLs
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Dependency Injection

// Shared Bounded Context Injection Configuration
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// Contest Bounded Context Injection Configuration
builder.Services.AddScoped<IContestQueryService, ContestQueryService>();
builder.Services.AddTransient<IContestCommandService, ContestCommandService>();
builder.Services.AddScoped<IContestRepository, ContestRepository>();
builder.Services.AddScoped<IWeeklyContestRepository, WeeklyContestRepository>();
builder.Services.AddScoped<IDetailQueryService, DetailQueryService>();
builder.Services.AddScoped<IDetailCommandService, DetailCommandService>();
builder.Services.AddScoped<ICourseDetailRepository, CourseDetailRepository>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();
builder.Services.AddScoped<IGoalQueryService, GoalQueryService>();
builder.Services.AddScoped<IGoalCommandService, GoalCommandService>();
builder.Services.AddScoped<IGoalRepository, GoalRepository>();

// Technical Evaluation Bounded Context Injection Configuration
builder.Services.AddScoped<ITechnicalTaskRepository, TechnicalTaskRepository>();
builder.Services.AddScoped<ITechnicalTestRepository, TechnicalTestRepository>();
builder.Services.AddScoped<ITechnicalTaskCommandService, TechnicalTaskCommandService>();
builder.Services.AddScoped<ITechnicalTaskQueryService, TechnicalTaskQueryService>();
builder.Services.AddScoped<ITechnicalTestQueryService, TechnicalTestQueryService>();

// Register Tutor services and repository
builder.Services.AddSingleton<ITutorRepository, TutorRepository>();
builder.Services.AddTransient<TutorCommandService>();
builder.Services.AddTransient<TutorQueryService>();

//Subscription services and repository
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ISubscriptionCommandService, SubscriptionCommandService>();
builder.Services.AddScoped<ISubscriptionQueryService, SubscriptionQueryService>();

// Profiles Bounded Context Injection Configuration
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();
builder.Services.AddScoped<IProfilesContextFacade, ProfilesContextFacade>();

// IAM Bounded Context Injection Configuration
// TokenSettings Configuration
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();

var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllPolicy");

// Add Authorization Middleware to Pipeline
app.UseRequestAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
