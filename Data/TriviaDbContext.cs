using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TriviaQuizMVC.Data
{
    public class TriviaDbContext : IdentityDbContext<IdentityUser>
    {
        public TriviaDbContext(DbContextOptions<TriviaDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
