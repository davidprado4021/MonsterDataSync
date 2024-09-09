using Microsoft.EntityFrameworkCore;

namespace EFPractica01.Models
{
    public class SkillsContext:DbContext
    {
        public DbSet<Skills> Skills { get; set; }
        public DbSet<Ranks> Ranks { get; set; } 
        public SkillsContext(DbContextOptions<SkillsContext> options)
            :base(options)
        { }
    }
}
