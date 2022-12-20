using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ToDoList.API.Handlers;
using ToDoList.API.Models;

namespace ToDoList.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //add Auth db context
            builder.Services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQLAuthConnection"]));
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;              //  ��������� ������ ���� �����
                options.Password.RequireLowercase = false;          //  ��������� ������ ���� ��������� �����
                options.Password.RequireNonAlphanumeric = false;    //  ��������� ������ ���� ������ ������������ �� ��������-���������
                options.Password.RequireUppercase = false;          //  ��������� ������ ���� ��������� �����
                options.Password.RequiredLength = 6;                //  ��������� ����������� ������ ������
                options.Password.RequiredUniqueChars = 1;           //  ��������� ����������� ���������� ���������� ��������
            });

            //add Article db context
            builder.Services.AddDbContext<OpenLoopDbContext>(options =>
                options.UseNpgsql(builder.Configuration["ConnectionStrings:PostgreSQLOpenLoopsConnection"]));

            // Adding services to the container.
            builder.Services.AddControllers();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "SpecialScheme"; //  ������������ � �������� ����� �� ���������
                options.DefaultChallengeScheme = "SpecialScheme";
                options.DefaultScheme = "SpecialScheme";
            })
                            .AddScheme<SpecialAuthenticationSchemeOptions, SpecialAuthHandler>(
                                "SpecialScheme", options => { });

            // Learn more about configuring Swagger/Open API at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
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