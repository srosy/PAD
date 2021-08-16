namespace PAD.Data
{
    public interface ICryptographyService
    {
        public string Encrypt(string email, string password);
        public (string email, string hashedpass) Decrypt(string encrypedString);
    }
}
