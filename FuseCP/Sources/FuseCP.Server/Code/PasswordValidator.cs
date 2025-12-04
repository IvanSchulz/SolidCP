using System.ServiceModel;
using FuseCP.Web.Services;

namespace FuseCP.Server
{
	public class PasswordValidator
	{

		public static bool Validate(string password) => password == Settings.Password;

		public static void Init()
		{
			FuseCP.Web.Services.UserNamePasswordValidator.ValidateServer = Validate;
		}

	}
}
