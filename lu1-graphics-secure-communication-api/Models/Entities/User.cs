using Microsoft.AspNetCore.Identity;

namespace lu1_graphics_secure_communication_api.Models.Entities;

public class User : IdentityUser
{
    public string Name { get; set; } = default!;

    public int Age { get; set; }

    public string? DoctorName { get; set; }

    public string? TreatmentDetails { get; set; }

    public DateOnly? TreatmentDate { get; set; }

    public int CurrentLevel { get; set; } = 1;
}