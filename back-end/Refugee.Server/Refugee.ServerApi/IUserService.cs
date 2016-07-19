using Refugee.Rest.Dto.Output;

namespace Refugee.ServerApi
{
    public interface IUserService
    {
        UserOutputDto GetMe();
    }
}