namespace ProjectPulseAPI.Shared.Helpers
{
    public static class Helpers
    {
        private const int WorkFactor = 12;

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }
    }
}