using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatApp.API.Configurations
{
    public static class JWTBearerMiddleware
    {
        public static void ConfigureJWT(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            string key, issuer, audience = "";
            if (builder.Environment.IsProduction())
            {
                key = configuration["ProdJwt:Key"];
                issuer = configuration["ProdJwt:Issuer"];
                audience = configuration["ProdJwt:Audience"];
            }
            else
            {
                key = configuration["Jwt:Key"];
                issuer = configuration["Jwt:Issuer"];
                audience = configuration["Jwt:Audience"];
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireExpirationTime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });
        }
    }
}
