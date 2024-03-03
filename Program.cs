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
                //��Swagger ui �[�J ���ҳ]�w
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT���Ҵy�z"
                });
                //��Swagger ui �[�J ui�M�I�s�ɪ��]�w(�S�]swagger�|�S�aBearer)
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
            //�`�J���ҳ]�w(�`�NJwtBearerc�M��M.net��������)
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,                        
                        //���ѻ{�ҥΪ��_
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))),
                        //������ɶ��������
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddScoped<SharedFuntions>();
            //��^JSON Key�R�W�]�w
            builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //�ϥλ{��
            app.UseAuthentication();
            app.UseAuthorization();

            


            app.MapControllers();

            app.Run();
        }
    }
}