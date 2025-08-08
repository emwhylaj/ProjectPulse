using Microsoft.IdentityModel.Tokens;
using ProjectPulseAPI.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProjectPulseAPI.Shared.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
        ClaimsPrincipal ValidateToken(string token);
        bool IsTokenValid(string token);
        RSA GetRSAKey();
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly RSA _rsa;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _rsa = RSA.Create(2048);
            
            // Load or generate RSA keys
            LoadOrGenerateKeys();
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(int.Parse(jwtSettings["ExpiryInHours"] ?? "24")),
                Issuer = jwtSettings["Issuer"] ?? "ProjectPulseAPI",
                Audience = jwtSettings["Audience"] ?? "ProjectPulseAPI",
                SigningCredentials = new SigningCredentials(new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new RsaSecurityKey(_rsa),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings["Issuer"] ?? "ProjectPulseAPI",
                    ValidateAudience = true,
                    ValidAudience = jwtSettings["Audience"] ?? "ProjectPulseAPI",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    RequireSignedTokens = true
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                
                // Verify token is JWT and uses RSA256
                if (validatedToken is not JwtSecurityToken jwtToken || 
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.RsaSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm");
                }

                return principal;
            }
            catch
            {
                throw new SecurityTokenException("Invalid token");
            }
        }

        public bool IsTokenValid(string token)
        {
            try
            {
                ValidateToken(token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public RSA GetRSAKey()
        {
            return _rsa;
        }

        private void LoadOrGenerateKeys()
        {
            var keysPath = Path.Combine(Directory.GetCurrentDirectory(), "Keys");
            Directory.CreateDirectory(keysPath);

            var privateKeyPath = Path.Combine(keysPath, "rsa_private_key.pem");
            var publicKeyPath = Path.Combine(keysPath, "rsa_public_key.pem");

            try
            {
                if (File.Exists(privateKeyPath) && File.Exists(publicKeyPath))
                {
                    // Load existing keys
                    var privateKeyPem = File.ReadAllText(privateKeyPath);
                    _rsa.ImportFromPem(privateKeyPem);
                }
                else
                {
                    // Generate new keys
                    GenerateAndSaveKeys(privateKeyPath, publicKeyPath);
                }
            }
            catch (Exception)
            {
                // If loading fails, generate new keys
                GenerateAndSaveKeys(privateKeyPath, publicKeyPath);
            }
        }

        private void GenerateAndSaveKeys(string privateKeyPath, string publicKeyPath)
        {
            // Generate new RSA key pair
            using var newRsa = RSA.Create(2048);
            
            // Export and save private key
            var privateKeyPem = newRsa.ExportRSAPrivateKeyPem();
            File.WriteAllText(privateKeyPath, privateKeyPem);
            
            // Export and save public key
            var publicKeyPem = newRsa.ExportRSAPublicKeyPem();
            File.WriteAllText(publicKeyPath, publicKeyPem);
            
            // Import the generated key into our RSA instance
            _rsa.ImportFromPem(privateKeyPem);
        }

        public void Dispose()
        {
            _rsa?.Dispose();
        }
    }
}