using lu1_graphics_secure_communication_api.Models.Dto;
using lu1_graphics_secure_communication_api.Models.Entities;

namespace lu1_graphics_secure_communication_api.Mappings.Interfaces;

public interface IUserMappingService
{
    User RegisterDtoToUser(RegisterDto registerDto);

    UserDto UserToUserDto(User user);
}