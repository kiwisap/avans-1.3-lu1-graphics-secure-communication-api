using Microsoft.AspNetCore.Identity;

namespace lu1_graphics_secure_communication_api.Models.Entities;
public class User : IdentityUser
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? DoctorName { get; set; }

    public string? TreatmentDetails { get; set; }

    public DateOnly? TreatmentSDate { get; set; }
}