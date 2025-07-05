using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.AssetAndResourceManagement.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using MaceTech.API.SubscriptionsAndPayments.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<PotRecord> PotRecords { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Plant> Plants { get; set; }
    public DbSet<DevicePlant> DevicePlants { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        builder.AddCreatedUpdatedInterceptor();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    { 
        base.OnModelCreating(builder);
        
        //  |: IAM Context
        builder.Entity<User>().HasKey(u => u.Uid);
        builder.Entity<User>().Property(u => u.Uid).IsRequired();
        builder.Entity<User>().Property(u => u.Email).IsRequired();
        builder.Entity<User>().Property(u => u.TokenVersion).IsRequired();
        builder.Entity<User>().Property(u => u.Status).IsRequired();
        
        //  |: Profiles Context
        builder.Entity<Profile>().HasKey(p => p.Id);
        builder.Entity<Profile>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Profile>().Property(p => p.Uid).IsRequired();
        builder.Entity<Profile>().OwnsOne(p => p.Name, n =>
        {
            n.WithOwner().HasForeignKey("Id");
            n.Property(p => p.FirstName).HasColumnName("FirstName");
            n.Property(p => p.LastName).HasColumnName("LastName");
        });
        builder.Entity<Profile>().OwnsOne(p => p.Address,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(s => s.Street).HasColumnName("AddressStreet");
                a.Property(s => s.Number).HasColumnName("AddressNumber");
                a.Property(s => s.City).HasColumnName("AddressCity");
                a.Property(s => s.PostalCode).HasColumnName("AddressPostalCode");
                a.Property(s => s.Country).HasColumnName("AddressCountry");
            });
        builder.Entity<Profile>().OwnsOne(p => p.PhoneNumber,
            a =>
            {
                a.WithOwner().HasForeignKey("Id");
                a.Property(s => s.CountryCode).HasColumnName("PhoneNumberCountryCode");
                a.Property(s => s.Number).HasColumnName("PhoneNumber");
            });

        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
        
        //  |: Subscriptions Context
        builder.Entity<Subscription>().HasKey(s => s.Id);
        builder.Entity<Subscription>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Subscription>().Property(s => s.Uid).IsRequired();
        builder.Entity<Subscription>().Property(s => s.CreatedAt).IsRequired();
        builder.Entity<Subscription>().Property(s => s.Plan).IsRequired();
        
        //  |: Pot Context
        builder.Entity<Pot>().HasKey(s => s.Id);
        builder.Entity<Pot>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Pot>().Property(s => s.Uid).IsRequired();
        builder.Entity<Pot>().Property(s => s.IsUserAssigned).IsRequired();
        builder.Entity<Pot>().Property(s => s.PlantId).IsRequired();
        builder.Entity<Pot>().Property(s => s.IsPlantLinked).IsRequired();
        builder.Entity<Pot>().Property(s => s.Name).IsRequired();
        builder.Entity<Pot>().Property(s => s.Location).IsRequired();
        builder.Entity<Pot>().Property(s => s.BatteryLevel).IsRequired();
        builder.Entity<Pot>().Property(s => s.WaterLevel).IsRequired();
        builder.Entity<Pot>().Property(s => s.Humidity).IsRequired();
        builder.Entity<Pot>().Property(s => s.Luminance).IsRequired();
        builder.Entity<Pot>().Property(s => s.Temperature).IsRequired();
        builder.Entity<Pot>().Property(s => s.Ph).IsRequired();
        builder.Entity<Pot>().Property(s => s.Salinity).IsRequired();
        builder.Entity<Pot>().Property(s => s.Status).IsRequired();
        builder.Entity<Pot>().Property(s => s.AssignedAt).IsRequired();
        builder.Entity<Pot>().Property(s => s.CreatedAt).IsRequired();

        
        //  |: Analytics Context
        builder.Entity<PotRecord>().ToTable("PotRecords");
        builder.Entity<PotRecord>().HasKey(p => p.Id);
        builder.Entity<PotRecord>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PotRecord>().Property(p => p.DeviceId).IsRequired();
        builder.Entity<PotRecord>().Property(p => p.Temperature).IsRequired();
        builder.Entity<PotRecord>().Property(p => p.Humidity).IsRequired();
        builder.Entity<PotRecord>().Property(p => p.Light).IsRequired();
        builder.Entity<PotRecord>().Property(p => p.Salinity).IsRequired();
        builder.Entity<PotRecord>().Property(p => p.Ph).IsRequired();
        builder.Entity<PotRecord>().Property(p => p.CreatedAt).IsRequired();
        builder.Entity<Alert>().ToTable("Alerts");
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Alert>().Property(a => a.DeviceId).IsRequired();
        builder.Entity<Alert>().Property(a => a.AlertType).IsRequired();
        builder.Entity<Alert>().Property(a => a.TriggerValue).IsRequired();
        builder.Entity<Alert>().Property(a => a.Timestamp).IsRequired();
        builder.Entity<Alert>().OwnsOne(a => a.GeneratedRecommendation, r =>
        {
            r.Property(p => p.Text).HasColumnName("RecommendationText");
            r.Property(p => p.Urgency).HasColumnName("Urgency");
            r.Property(p => p.GuideUrl).HasColumnName("GuideUrl");
        });
        
        //  |: Planning Context
        builder.Entity<Plant>().ToTable("Plants");
        builder.Entity<Plant>().HasKey(p => p.Id);
        builder.Entity<Plant>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Plant>().Property(p => p.CommonName).IsRequired().HasMaxLength(100);
        builder.Entity<Plant>().Property(p => p.ScientificName).HasMaxLength(150);
        builder.Entity<Plant>().Property(p => p.ImageUrl).HasMaxLength(255);
        builder.Entity<Plant>().Property(p => p.Description).HasMaxLength(1000);
        
        builder.Entity<Plant>().OwnsOne(p => p.OptimalParameters, parameters =>
        {
            parameters.OwnsOne(po => po.TemperaturaAmbiente, temp =>
            {
                temp.Property(t => t.Min).HasColumnName("OptimalTempMin");
                temp.Property(t => t.Max).HasColumnName("OptimalTempMax");
            });
            parameters.OwnsOne(po => po.Humedad, hum =>
            {
                hum.Property(h => h.Min).HasColumnName("OptimalHumidityMin");
                hum.Property(h => h.Max).HasColumnName("OptimalHumidityMax");
            });
            parameters.OwnsOne(po => po.Luminosidad, lum =>
            {
                lum.Property(l => l.Min).HasColumnName("OptimalLuminosityMin");
                lum.Property(l => l.Max).HasColumnName("OptimalLuminosityMax");
            });
            parameters.OwnsOne(po => po.SalinidadSuelo, sal =>
            {
                sal.Property(s => s.Min).HasColumnName("OptimalSalinityMin");
                sal.Property(s => s.Max).HasColumnName("OptimalSalinityMax");
            });
            parameters.OwnsOne(po => po.PhSuelo, ph =>
            {
                ph.Property(p => p.Min).HasColumnName("OptimalPhMin");
                ph.Property(p => p.Max).HasColumnName("OptimalPhMax");
            });
        });
        builder.Entity<DevicePlant>().ToTable("DevicePlants");
        builder.Entity<DevicePlant>().HasKey(dp => dp.Id);
        builder.Entity<DevicePlant>().Property(dp => dp.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<DevicePlant>().Property(dp => dp.DeviceId).IsRequired().HasMaxLength(50);
        builder.Entity<DevicePlant>()
            .HasOne(dp => dp.Plant)
            .WithMany()
            .HasForeignKey(dp => dp.PlantId)
            .IsRequired();
        builder.Entity<DevicePlant>().OwnsOne(dp => dp.CustomThresholds, thresholds =>
        {
            thresholds.OwnsOne(t => t.TemperaturaAmbiente, temp =>
            {
                temp.Property(p => p.Min).HasColumnName("CustomTempMin");
                temp.Property(p => p.Max).HasColumnName("CustomTempMax");
            });
             thresholds.OwnsOne(t => t.Humedad, hum =>
            {
                hum.Property(p => p.Min).HasColumnName("CustomHumidityMin");
                hum.Property(p => p.Max).HasColumnName("CustomHumidityMax");
            });
            thresholds.OwnsOne(t => t.Luminosidad, lum =>
            {
                lum.Property(p => p.Min).HasColumnName("CustomLuminosityMin");
                lum.Property(p => p.Max).HasColumnName("CustomLuminosityMax");
            });
            thresholds.OwnsOne(t => t.SalinidadSuelo, sal =>
            {
                sal.Property(p => p.Min).HasColumnName("CustomSalinityMin");
                sal.Property(p => p.Max).HasColumnName("CustomSalinityMax");
            });
            thresholds.OwnsOne(t => t.PhSuelo, ph =>
            {
                ph.Property(p => p.Min).HasColumnName("CustomPhMin");
                ph.Property(p => p.Max).HasColumnName("CustomPhMax");
            });
        });

        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
    }
}