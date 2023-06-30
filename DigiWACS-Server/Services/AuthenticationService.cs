using DigiWACS.Authentication;
using DigiWACS.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace DigiWACS.Services {
    public class AuthenticationService : DigiWACS.Protos.Authentication.AuthenticationBase {
        private readonly ILogger<AuthenticationService> _logger;
        
        public AuthenticationService(ILogger<AuthenticationService> logger) {
            _logger = logger;
        }

        [AllowAnonymous]
        public override Task<JwtToken> Authenticate(Credential c, ServerCallContext con) {
            var outToken = "";
            if (c.Pass == "halsey") {
                if (con != null) {
                    var jwtOptions = con.GetHttpContext().RequestServices.GetService<DigiWACS.Authentication.JwtOptions>();
                    var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtOptions!.SigningKey);
                    var symmetricKey = new SymmetricSecurityKey(keyBytes);

                    var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
                    
                    var claims = new List<Claim>() {
                        new Claim("sub", c.User),
                        new Claim("name", c.User),
                        new Claim("aud", jwtOptions.Audience)
                    };

                    claims.Add(new Claim("role", "Administrator"));

                    var token = new JwtSecurityToken(
                        issuer: jwtOptions.Issuer,
                        audience: jwtOptions.Audience,
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: signingCredentials
                    );
                    outToken = new JwtSecurityTokenHandler().WriteToken(token);
                }
                
            }
            return Task.FromResult(new JwtToken { JwtToken_ = outToken });
        }
        
    }
}
