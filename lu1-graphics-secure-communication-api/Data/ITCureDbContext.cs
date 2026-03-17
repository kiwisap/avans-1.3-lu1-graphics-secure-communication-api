using Microsoft.EntityFrameworkCore;

namespace lu1_graphics_secure_communication_api.Data;

public class ITCureDbContext(DbContextOptions<ITCureDbContext> options) : DbContext(options)
{

}