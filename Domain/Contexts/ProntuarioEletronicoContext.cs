using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    //public class SisPfaContext : DbContext
    public class AutomationContext : AuditDbContext
    {
        public AutomationContext(DbContextOptions<AutomationContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Deixar em ordem alfabética

        }
    }
}
