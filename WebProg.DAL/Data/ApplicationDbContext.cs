using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebProg.DAL.Entities;

namespace WebProg.DAL.Data
{
    /// <summary>
    /// Контекст базы данных Identity (ЛР 4, п. 5.2.2).
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
