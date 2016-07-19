using Refugee.Rest.Dto.Input;
using Refugee.Rest.Dto.Output;

namespace Refugee.ServerApi
{
    public interface IAuthenticationService
    {
        AuthenticationOutputDto Authenticate(AuthenticationInputDto authenticationInputDto);
    }
}