namespace lu1_graphics_secure_communication_api.Models.Dto;

public class UserDto
{
    public string Email { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public int Age { get; set; }

    public string? DoctorName { get; set; }

    public string? TreatmentDetails { get; set; }

    public DateOnly? TreatmentDate { get; set; }

    public int CurrentLevel { get; set; }
}