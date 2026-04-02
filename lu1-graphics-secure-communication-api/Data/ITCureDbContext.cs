using lu1_graphics_secure_communication_api.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace lu1_graphics_secure_communication_api.Data;

public class ITCureDbContext(DbContextOptions<ITCureDbContext> options) : IdentityDbContext<User>(options)
{

}