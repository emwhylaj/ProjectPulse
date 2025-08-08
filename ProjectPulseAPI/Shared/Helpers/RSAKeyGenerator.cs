using System.Security.Cryptography;

namespace ProjectPulseAPI.Shared.Helpers
{
    public static class RSAKeyGenerator
    {
        public static void GenerateKeys(string privateKeyPath, string publicKeyPath)
        {
            using var rsa = RSA.Create(2048);
            
            // Export private key
            var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
            File.WriteAllText(privateKeyPath, privateKey);
            
            // Export public key
            var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
            File.WriteAllText(publicKeyPath, publicKey);
        }

        public static RSA LoadPrivateKey(string privateKeyPath)
        {
            if (!File.Exists(privateKeyPath))
                throw new FileNotFoundException($"Private key file not found: {privateKeyPath}");

            var keyData = File.ReadAllText(privateKeyPath);
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(keyData), out _);
            return rsa;
        }

        public static RSA LoadPublicKey(string publicKeyPath)
        {
            if (!File.Exists(publicKeyPath))
                throw new FileNotFoundException($"Public key file not found: {publicKeyPath}");

            var keyData = File.ReadAllText(publicKeyPath);
            var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(keyData), out _);
            return rsa;
        }
    }
}