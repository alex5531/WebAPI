namespace WebAPI.BLL.Contracts
{
    public interface IPasswordStorage
    {
        public string CreateHash(string password);
        public bool VerifyPassword(string password, string goodHash);
    }
}
