using FirstAPI.DTO;

namespace FirstAPI.IService
{
    public interface IAuthService
    {
        Task<Tuple<int, TokenDTO>> LoginUser(UserDTO userDto);
        Task<Tuple<int, string>> RegisterUser(UserDTO userdto);
    }
}
