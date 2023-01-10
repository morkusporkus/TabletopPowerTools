using DMPowerTools.Core.Models;
using Microsoft.EntityFrameworkCore;
using Action = DMPowerTools.Core.Models.Action;

namespace DMPowerTools.Core.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Creature> Creatures { get; set; } = null!;
    public DbSet<Ability> Abilities { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<Action> Actions { get; set; } = null!;
}
