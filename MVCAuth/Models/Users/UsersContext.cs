using Microsoft.EntityFrameworkCore;

namespace MVCAuth.Models.Users
{
    public class UsersContext : DbContext
    {
        public UsersContext()
        {

        }
        public UsersContext(DbContextOptions<UsersContext> options): base(options) 
        {
        }
        
        public virtual DbSet<Admin> Admins { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new();

            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.AddUserSecrets<Program>().Build();

            string connectionString = "";
            connectionString = configuration.GetConnectionString("SQLConnection");

            _ = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Login).HasMaxLength(50);
                entity.Property(e => e.Password).HasMaxLength(100);
                entity.Property(e => e.FirstName).HasMaxLength(50);
                entity.Property(e => e.LastName).HasMaxLength(50);

            });
        }
    }
}
