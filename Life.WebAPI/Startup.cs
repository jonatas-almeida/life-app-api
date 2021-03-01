using System.IO;
using System.Text;
using AutoMapper;
using Life.Domain.Identity;
using Life.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Life.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Life.WebAPI", Version = "v1" });
            });

            services.AddDbContext<LifeContext>(
                x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
            );

            //Configura��o geral
            IdentityBuilder builder = services.AddIdentityCore<User>(options =>
            {
                //SENHA
                //Resetando os valores padr�es da valida��o de senha
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
            });

            //Inst�ncia do IdentityBuilder criado anteriormente (builder.Services)
            //Consigura��es do Context, das Roles e dos Usu�rios
            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<LifeContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            //Configura��o do JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //Assinatura da chave do emissor da api
                    ValidateIssuerSigningKey = true,

                    //Descriptografa a chave que estiver definida em AppSettings
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),

                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            //Determina qual Controller ser� chamada
            services.AddMvc(options =>
            {
                //Toda vez que uma rota for chamada, ele vai requerir que o usu�rio esteja autenticado
                //Na sequ�ncia ele ir� usar o AuthorizeFilter para filtrar todas as reuisi��es que tiver
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                options.Filters.Add(new AuthorizeFilter(policy));

                //Para que a API funcione corretamente é necessário desativar o Endpoint Routing, por causa das versões dos pacotes
                options.EnableEndpointRouting = false;

            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });//Resolve qualquer looping infinito que houver entre as rela��es das entidades. Na vers�o 2.2 era feito da seguinte forma:
            /*
             .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopingHandling = Newtonsoft.Json.ReferenceLoopingHandling.Ignore);
                A vers�o que est� sendo utlizada atualmente � a 5.0.103
             */

            services.AddAutoMapper();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Life.WebAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseMvc();
        }
    }
}
