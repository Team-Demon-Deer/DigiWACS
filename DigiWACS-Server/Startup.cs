using DigiWACS.Services;
using DigiWACS.Server.Authorization.Requirements;
using DigiWACS.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DigiWACS.Server {
    public class Startup {
        record UserDto(string UserName, string Password);
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
           var jwtOptions = _configuration.GetSection("JwtOptions").Get<JwtOptions>();
            services.AddSingleton(jwtOptions!);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    byte[] signingKeyBytes = Encoding.UTF8.GetBytes(jwtOptions!.SigningKey);
                    opts.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
                    };
                });
            services.AddAuthorization((options) => {
                options.AddPolicy("RequireElevated", policy =>
                {
                    policy.RequireRole("Administrator");
                });
            });

            services.AddGrpc();
            services.AddGrpcReflection(); 
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapGrpcService<Services.AuthenticationService>();
                endpoints.MapGrpcService<Services.UnitsService>();
                endpoints.MapGrpcReflectionService();
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"); });

            });
        } 
    }
}
