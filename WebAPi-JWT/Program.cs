
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebAPi_JWT.Infrastructure;
using WebAPi_JWT.WebApi.Core.ServiceContract;
using WebAPi_JWT.WebApi.Core.Services;
using WebAPi_JWT.WebApi.Infrastructure.Repository;

namespace WebAPi_JWT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                var securityDefinition = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Jwt Authentication using Bearer Scheme"
                };
                opt.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityDefinition);
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            new string[] {}
                        }
                    });
            });
            builder.Services.AddAuthorization();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(
                                builder.Configuration["Jwt:Key"]
                            )
                        ),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddDbContext<DataAccess>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"));
            });
            builder.Services.AddScoped<ITokenProvider, TokenProvider>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ITokenRepository, TokenRepository>();
            var cs = builder.Configuration.GetConnectionString("DefaultConnectionString");
            builder.Services.AddAuthorization();
            Console.WriteLine("CS: " + cs);

            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
