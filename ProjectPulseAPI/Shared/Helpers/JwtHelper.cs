using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace ProjectPulseAPI.Shared.Helpers
{
    public static class JwtHelper
    {
        public static RSA LoadRSAKey(string keyPath)
        {
            var rsa = RSA.Create();
            if (File.Exists(keyPath))
            {
                var keyData = File.ReadAllText(keyPath);
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(keyData), out _);
            }
            else
            {
                // Generate new key if file doesn't exist
                var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                File.WriteAllText(keyPath, privateKey);
                
                // Also save the public key
                var publicKeyPath = keyPath.Replace("private.key", "public.key");
                var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                File.WriteAllText(publicKeyPath, publicKey);
            }
            
            return rsa;
        }

        public static RSA LoadRSAPublicKey(string publicKeyPath)
        {
            var rsa = RSA.Create();
            if (File.Exists(publicKeyPath))
            {
                var keyData = File.ReadAllText(publicKeyPath);
                rsa.ImportRSAPublicKey(Convert.FromBase64String(keyData), out _);
            }
            else
            {
                throw new FileNotFoundException($"Public key file not found: {publicKeyPath}");
            }
            
            return rsa;
        }

        public static RsaSecurityKey GetRSASecurityKey(RSA rsa)
        {
            return new RsaSecurityKey(rsa);
        }
    }
}