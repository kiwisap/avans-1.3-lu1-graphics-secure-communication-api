using Microsoft.EntityFrameworkCore;

namespace lu1_graphics_secure_communication_api.Data;

public class ITCureDbContext : DbContext
{
    public ITCureDbContext(DbContextOptions<ITCureDbContext> options) : base(options) { }
}
