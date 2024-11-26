using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MVCAuth;

public partial class RealEstateRentalContext : DbContext
{
    public RealEstateRentalContext()
    {
    }

    public RealEstateRentalContext(DbContextOptions<RealEstateRentalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Flat> Flats { get; set; }

    public virtual DbSet<FlatsContract> FlatsContracts { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<HotelsRoom> HotelsRooms { get; set; }

    public virtual DbSet<House> Houses { get; set; }

    public virtual DbSet<HousesContract> HousesContracts { get; set; }

    public virtual DbSet<LandLord> LandLords { get; set; }

    public virtual DbSet<LandLordsAdditionalInfo> LandLordsAdditionalInfos { get; set; }

    public virtual DbSet<Lessee> Lessees { get; set; }

    public virtual DbSet<LesseesAdditionalInfo> LesseesAdditionalInfos { get; set; }

    public virtual DbSet<RoomsContract> RoomsContracts { get; set; }



    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    ConfigurationBuilder builder = new();

    //    builder.SetBasePath(Directory.GetCurrentDirectory());
    //    builder.AddJsonFile("appsettings.json");
    //    IConfigurationRoot configuration = builder.AddUserSecrets<Program>().Build();

    //    string connectionString = "";
    //    connectionString = configuration.GetConnectionString("SQLConnection");

    //    //_ = optionsBuilder
    //    //    .UseSqlServer(connectionString)
    //    //    .Options;
    //    //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));


    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flat>(entity =>
        {
            entity.HasKey(e => e.Fid);

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.AvgMark).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.CostPerDay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Header).HasMaxLength(100);
            entity.Property(e => e.Llid).HasColumnName("LLid");

            entity.HasOne(d => d.Ll).WithMany(p => p.Flats)
                .HasForeignKey(d => d.Llid);
        });

        modelBuilder.Entity<FlatsContract>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.Llid).HasColumnName("LLid");

            entity.HasOne(d => d.FidNavigation).WithMany(p => p.FlatsContracts)
                .HasForeignKey(d => d.Fid);

            entity.HasOne(d => d.LidNavigation).WithMany(p => p.FlatsContracts)
                .HasForeignKey(d => d.Lid);

            entity.HasOne(d => d.Ll).WithMany(p => p.FlatsContracts)
                .HasForeignKey(d => d.Llid);
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Hid);

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.AvgMark).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Header).HasMaxLength(100);
            entity.Property(e => e.Llid).HasColumnName("LLid");

            entity.HasOne(d => d.Ll).WithMany(p => p.Hotels)
                .HasForeignKey(d => d.Llid);
        });

        modelBuilder.Entity<HotelsRoom>(entity =>
        {
            entity.HasKey(e => e.Rid);

            entity.Property(e => e.AvgMark).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.CostPerDay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Header).HasMaxLength(100);

            entity.HasOne(d => d.HidNavigation).WithMany(p => p.HotelsRooms)
                .HasForeignKey(d => d.Hid);
        });

        modelBuilder.Entity<House>(entity =>
        {
            entity.HasKey(e => e.Pid);

            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.AvgMark).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.CostPerDay).HasColumnType("money");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.Header).HasMaxLength(100);
            entity.Property(e => e.Llid).HasColumnName("LLid");

            entity.HasOne(d => d.Ll).WithMany(p => p.Houses)
                .HasForeignKey(d => d.Llid);
        });

        modelBuilder.Entity<HousesContract>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.Llid).HasColumnName("LLid");

            entity.HasOne(d => d.LidNavigation).WithMany(p => p.HousesContracts)
                .HasForeignKey(d => d.Lid);

            entity.HasOne(d => d.Ll).WithMany(p => p.HousesContracts)
                .HasForeignKey(d => d.Llid);

            entity.HasOne(d => d.PidNavigation).WithMany(p => p.HousesContracts)
                .HasForeignKey(d => d.Pid);
        });

        modelBuilder.Entity<LandLord>(entity =>
        {
            entity.HasKey(e => e.Llid);
            entity.Property(e => e.Llid).HasColumnName("LLid");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);

        });

        modelBuilder.Entity<LandLordsAdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.Llid);

            entity.ToTable("LandLordsAdditionalInfo");

            entity.Property(e => e.Llid)
                .ValueGeneratedNever()
                .HasColumnName("LLid");

            entity.Property(e => e.AvgMark).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.Llid).HasColumnName("LLid");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.PassportId).HasMaxLength(70);
            entity.Property(e => e.Surname).HasMaxLength(30);
            entity.Property(e => e.Telephone).HasMaxLength(20);
            entity.HasKey(e => e.Llid);

            entity.HasOne(d => d.Ll).WithOne(p => p.LandLordsAdditionalInfo)
                .HasForeignKey<LandLordsAdditionalInfo>(d => d.Llid)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });

        modelBuilder.Entity<Lessee>(entity =>
        {
            entity.HasKey(e => e.Lid);

            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
        });

        modelBuilder.Entity<LesseesAdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.Lid);

            entity.ToTable("LesseesAdditionalInfo");

            entity.Property(e => e.Lid).ValueGeneratedNever();

            entity.Property(e => e.AvgMark).HasColumnType("decimal(3, 2)");
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.PassportId).HasMaxLength(70);
            entity.Property(e => e.Surname).HasMaxLength(30);
            entity.Property(e => e.Telephone).HasMaxLength(20);

            entity.HasOne(d => d.LidNavigation).WithOne(p => p.LesseesAdditionalInfo)
                .HasForeignKey<LesseesAdditionalInfo>(d => d.Lid)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<RoomsContract>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Cost).HasColumnType("money");
            entity.Property(e => e.Llid).HasColumnName("LLid");

            entity.HasOne(d => d.LidNavigation).WithMany(p => p.RoomsContracts)
                .HasForeignKey(d => d.Lid);

            entity.HasOne(d => d.Ll).WithMany(p => p.RoomsContracts)
                .HasForeignKey(d => d.Llid);

            entity.HasOne(d => d.RidNavigation).WithMany(p => p.RoomsContracts)
                .HasForeignKey(d => d.Rid);
        });


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

