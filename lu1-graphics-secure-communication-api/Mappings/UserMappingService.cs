using lu1_graphics_secure_communication_api.Mappings.Interfaces;
using lu1_graphics_secure_communication_api.Models.Dto;
using lu1_graphics_secure_communication_api.Models.Entities;

namespace lu1_graphics_secure_communication_api.Mappings
{
    public class UserMappingService : IUserMappingService
    {
        public User RegisterDtoToUser(RegisterDto registerDto)
        {
            return new User
            {
                Email = registerDto.Email,
                UserName = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Age = registerDto.Age,
                DoctorName = registerDto.DoctorName,
                TreatmentDetails = registerDto.TreatmentDetails,
                TreatmentDate = registerDto.TreatmentDate
            };
        }

        public UserDto UserToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = user.Age,
                DoctorName = user.DoctorName,
                TreatmentDetails = user.TreatmentDetails,
                TreatmentDate = user.TreatmentDate
            };
        }
    }
}
