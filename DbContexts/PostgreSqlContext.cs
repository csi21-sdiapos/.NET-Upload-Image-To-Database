using Microsoft.EntityFrameworkCore;
using UploadImageToDatabase.Entities;

namespace UploadImageToDatabase.DbContexts
{
    public class PostgreSqlContext : DbContext
    {
        public PostgreSqlContext(DbContextOptions options) : base(options) 
        { }

        public virtual DbSet<Alumno> DbSetAlumnos { get; set; }
    }
}
