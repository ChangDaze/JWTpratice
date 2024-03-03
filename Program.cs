using JWTpratice.UserDefinedClasses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace JWTpratice
{
    public class Program
    {
        private readonly IConfiguration _configuration;
        public Program(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                //讓Swagger ui 加入 驗證設定
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT驗證描述"
                });
                //讓Swagger ui 加入 ui和呼叫時的設定(沒設swagger會沒帶Bearer)
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            //注入驗證設定(注意JwtBearerc套件和.net版本對應)
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,                        
                        //提供認證用金鑰
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))),
                        //讓到期時間完全對齊
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddScoped<SharedFuntions>();
            //返回JSON Key命名設定
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //使用認證
            app.UseAuthentication();
            app.UseAuthorization();

            


            app.MapControllers();

            app.Run();
        }
    }
}