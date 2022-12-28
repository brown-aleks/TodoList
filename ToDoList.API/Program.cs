using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ToDoList.API.Authorization;
using ToDoList.API.Controllers;
using ToDoList.API.Data;
using ToDoList.API.Handlers;
using ToDoList.API.Models;
using ToDoList.API.Services;

namespace ToDoList.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //add OpenLoop db context
            builder.Services.AddDbContext<OpenLoopDbContext>(options =>
                options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQLOpenLoopsConnection"]));

            //add Auth db context
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQLAuthConnection"]));
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;              //  Требовать хотя бы одну цифру
                options.Password.RequireLowercase = false;          //  Требовать хотя бы одну маленькую букву
                options.Password.RequireNonAlphanumeric = false;    //  Требовать хотя бы один символ отличающийся от буквенно-цифрового
                options.Password.RequireUppercase = false;          //  Требовать хотя бы одну заглавную букву
                options.Password.RequiredLength = 6;                //  Требовать минимальную длинну пароля
                options.Password.RequiredUniqueChars = 1;           //  Требовать минимальное количество уникальных символов
            });

            // Adding services to the container.
            builder.Services.AddControllers();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "SpecialScheme"; //  Используется в качестве схемы по умолчанию
                options.DefaultChallengeScheme = "SpecialScheme";
                options.DefaultScheme = "SpecialScheme";
            })
                            .AddScheme<SpecialAuthenticationSchemeOptions, SpecialAuthHandler>(
                                "SpecialScheme", options => { });


            // Learn more about configuring Swagger/Open API at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "ToDoList.API.xml");
                option.IncludeXmlComments(xmlPath);

                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Demo API",
                    Description = "Демонстрационный API с идентификацией и пользовательским токеном JWT",
                    Contact = new OpenApiContact
                    {
                        Name = "Пишите мне в Телеграмм",
                        Url = new Uri("https://t.me/brown_aleks")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Тип лицензии ИКХССВ - Используйте Кто Хотите, Сами Себе Виноваты. =)",
                        Url = new Uri("https://example.com/license")
                    }
                });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            
            builder.Services.AddScoped<OpenLoopService>();
            builder.Services.AddScoped<AuthenticatedUser>();
            builder.Services.AddScoped<IAuthorizationHandler, IsOpenLoopOwnerHandler>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CanEditOpenLoop",
                    policyBuilder => policyBuilder
                        .AddRequirements(new IsOpenLoopOwnerRequirement()));
            });


            var app = builder.Build();

            // Set up an HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}