using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;
using TodoApi.Data;

namespace TodoApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureJwtAuthService(IServiceCollection services)  
        {  
            var audienceConfig = Configuration.GetSection("Audience");  
            var symmetricKeyAsBase64 = audienceConfig["Secret"];  
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);  
            var signingKey = new SymmetricSecurityKey(keyByteArray);  
        
            var tokenValidationParameters = new TokenValidationParameters  
            {  
                // The signing key must match!  
                ValidateIssuerSigningKey = true,  
                IssuerSigningKey = signingKey,  
        
                // Validate the JWT Issuer (iss) claim  
                ValidateIssuer = true,  
                ValidIssuer = audienceConfig["Iss"],  
        
                // Validate the JWT Audience (aud) claim  
                ValidateAudience = true,  
                ValidAudience = audienceConfig["Aud"],  
        
                // Validate the token expiry  
                ValidateLifetime = true,  
        
                ClockSkew = TimeSpan.Zero  
            };  
            
            services.AddAuthentication( ).AddJwtBearer(o =>  
            {  
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = tokenValidationParameters;  
            });  
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
            services.AddDbContext<TodoContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))
                .AddDbContext<TodoIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<TodoIdentityDbContext>()
                .AddDefaultTokenProviders();
            
            ConfigureJwtAuthService(services); 

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //use the authentication  
            app.UseAuthentication(); 
            app.UseMvc();
        }
    }
}
