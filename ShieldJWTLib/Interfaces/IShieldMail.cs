namespace ShieldJWTLib.Interfaces
{
    public interface IShieldMail
    {
        void SendConfirmCodeTo(string email, string name, string code);
    }
}
