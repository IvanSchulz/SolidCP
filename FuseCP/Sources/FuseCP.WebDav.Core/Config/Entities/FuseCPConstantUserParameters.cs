namespace FuseCP.WebDav.Core.Config.Entities
{
    public class FuseCPConstantUserParameters : AbstractConfigCollection
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public FuseCPConstantUserParameters()
        {
            Login = ConfigSection.FuseCPConstantUser.Login;
            Password = ConfigSection.FuseCPConstantUser.Password;
        }
    }
}
