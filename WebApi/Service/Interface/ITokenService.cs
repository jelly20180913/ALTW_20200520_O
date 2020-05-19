using WebApi.Models.CustomModel;
namespace WebApi.Service.Interface
{
    public interface ITokenService
    {
        object SetToken(LoginData loginData);
    }
}
