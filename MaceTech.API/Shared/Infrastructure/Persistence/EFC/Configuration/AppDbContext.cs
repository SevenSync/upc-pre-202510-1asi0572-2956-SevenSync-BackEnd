using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using MaceTech.API.Analytics.Domain.Model.Aggregates;
using MaceTech.API.ARM.Domain.Model.Aggregates;
using MaceTech.API.IAM.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.Aggregates;
using MaceTech.API.Planning.Domain.Model.ValueObjects;
using MaceTech.API.Profiles.Domain.Model.Aggregates;
using MaceTech.API.Profiles.Domain.Model.ValueObjects;
using MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using MaceTech.API.SP.Domain.Model.Aggregates;
using MaceTech.API.Watering.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Plant> Plants { get; set; }

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
        builder.Entity<Profile>().Property(p => p.UserId).IsRequired();
        builder.Entity<Profile>().OwnsOne<PersonName>(p => p.Name, n =>
        {
            n.WithOwner().HasForeignKey("Id");
            n.HasKey("Id");
            n.Property(p => p.FirstName);
            n.Property(p => p.LastName);
        });
        builder.Entity<Profile>().OwnsOne<PersonAddress>(p => p.Address, a => {
            a.WithOwner().HasForeignKey("Id");
            a.HasKey("Id");
            a.Property(s => s.Street);
            a.Property(s => s.BuildingNumber);
            a.Property(s => s.City);
            a.Property(s => s.PostalCode);
            a.Property(s => s.Country);
        });
        builder.Entity<Profile>().OwnsOne<PhoneNumber>(p => p.PhoneNumber,
        a =>
        {
            a.WithOwner().HasForeignKey("Id");  
            a.HasKey("Id");
            a.Property(s => s.CountryCode);
            a.Property(s => s.Number);
        });

        //  |: Subscriptions Context
        builder.Entity<Subscription>().HasKey(s => s.Id);
        builder.Entity<Subscription>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Subscription>().Property(s => s.Uid).IsRequired();
        builder.Entity<Subscription>().Property(s => s.Plan).IsRequired();

        //  |: Pot Context
        builder.Entity<Pot>().HasKey(s => s.Id);
        builder.Entity<Pot>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Pot>().Property(s => s.UserId).IsRequired();
        builder.Entity<Pot>().Property(s => s.PlantId).IsRequired();
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

        //  |: Analytics Context
        builder.Entity<Alert>().ToTable("Alerts");
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Alert>().Property(a => a.DeviceId).IsRequired();
        builder.Entity<Alert>().Property(a => a.AlertType).IsRequired();
        builder.Entity<Alert>().Property(a => a.TriggerValue).IsRequired();
        builder.Entity<Alert>().Property(a => a.Timestamp).IsRequired();
        
        builder.Entity<Alert>()
        .OwnsOne(a => a.GeneratedRecommendation, r =>
        {
            r.WithOwner().HasForeignKey("Id");
            r.HasKey("Id");
            r.Property(re => re.Text).IsRequired().HasMaxLength(500).HasColumnName("recommendation_text");
            r.Property(re => re.Urgency).IsRequired().HasMaxLength(50).HasColumnName("recommendation_urgency");
            r.Property(re => re.GuideUrl).HasMaxLength(255).HasColumnName("recommendation_guide_url");
        });

        builder.Entity<PotRecord>().ToTable("PotRecords");
        builder.Entity<PotRecord>().HasKey(pr => pr.Id);
        builder.Entity<PotRecord>().Property(pr => pr.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PotRecord>().Property(pr => pr.DeviceId).IsRequired().HasMaxLength(50);
        builder.Entity<PotRecord>().Property(pr => pr.Temperature).IsRequired();
        builder.Entity<PotRecord>().Property(pr => pr.Humidity).IsRequired();
        builder.Entity<PotRecord>().Property(pr => pr.Light).IsRequired();
        builder.Entity<PotRecord>().Property(pr => pr.Salinity).IsRequired();
        builder.Entity<PotRecord>().Property(pr => pr.Ph).IsRequired();
        builder.Entity<PotRecord>().Property(pr => pr.CreatedAt).IsRequired();

        //  |: Planning Context
        builder.Entity<Plant>().ToTable("Plants");
        builder.Entity<Plant>().HasKey(p => p.Id);
        builder.Entity<Plant>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Plant>().Property(p => p.CommonName).IsRequired().HasMaxLength(100);
        builder.Entity<Plant>().Property(p => p.ScientificName).HasMaxLength(150);
        builder.Entity<Plant>().Property(p => p.ImageUrl).HasMaxLength(255);
        builder.Entity<Plant>().Property(p => p.Description).HasMaxLength(1000);
        
       var stringListComparer = new ValueComparer<List<string>>(
            (c1, c2) => (c1 ?? Enumerable.Empty<string>()).SequenceEqual(c2 ?? Enumerable.Empty<string>()),
            c => (c ?? Enumerable.Empty<string>()).Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());
                                                                                                      
        builder.Entity<Plant>().OwnsOne<OptimalParameters>(p => p.OptimalParameters,
            parameters =>
            {
                parameters.WithOwner().HasForeignKey("Id");
                parameters.HasKey("Id");

                parameters.OwnsOne<Range<double>>(po => po.Temperature, temp =>
                {
                    temp.WithOwner().HasForeignKey("Id");
                    temp.HasKey("Id");
                    temp.Property(t => t.Min).HasColumnName("temperature_min");
                    temp.Property(t => t.Max).HasColumnName("temperature_max");
                });

                parameters.OwnsOne<Range<double>>(po => po.Humidity, hum =>
                {
                    hum.WithOwner().HasForeignKey("Id");
                    hum.HasKey("Id");
                    hum.Property(h => h.Min).HasColumnName("humidity_min");
                    hum.Property(h => h.Max).HasColumnName("humidity_max");
                });

                parameters.OwnsOne<Range<double>>(po => po.Light, lum =>
                {
                    lum.WithOwner().HasForeignKey("Id");
                    lum.HasKey("Id");
                    lum.Property(l => l.Min).HasColumnName("light_min");
                    lum.Property(l => l.Max).HasColumnName("light_max");
                });

                parameters.OwnsOne<Range<double>>(po => po.Salinity, sal =>
                {
                    sal.WithOwner().HasForeignKey("Id");
                    sal.HasKey("Id");
                    sal.Property(s => s.Min).HasColumnName("salinity_min");
                    sal.Property(s => s.Max).HasColumnName("salinity_max");
                });

                parameters.OwnsOne<Range<double>>(po => po.Ph, ph =>
                {
                    ph.WithOwner().HasForeignKey("Id");
                    ph.HasKey("Id");
                    ph.Property(p => p.Min).HasColumnName("ph_min");
                    ph.Property(p => p.Max).HasColumnName("ph_max");
                });
            });
                                                                                                             
        builder.Entity<Plant>().OwnsOne<SearchFilters>(p => p.SearchFilters, sf =>
        {
            sf.WithOwner().HasForeignKey("Id");
            sf.HasKey("Id");
            sf.Property(s => s.Difficulty).HasColumnName("search_difficulty");
            sf.Property(s => s.Light).HasColumnName("search_light");
            sf.Property(s => s.SizePotential).HasColumnName("search_size_potential");
            sf.Property(s => s.Ubication).HasColumnName("search_ubicacion")
            .HasConversion(
                v => string.Join(",", v),                                                                          
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),                                 
                stringListComparer                                                                                 
            );

            sf.Property(s => s.Tags).HasColumnName("search_tags")
                .HasConversion(
                v => string.Join(",", v), 
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                stringListComparer
            );
        });
        
        builder.Entity<Plant>().OwnsOne<VisualIdentification>(p => p.VisualIdentification, vi =>
        {                                                                                                      
            vi.WithOwner().HasForeignKey("Id");
            vi.HasKey("Id");

            vi.Property(v => v.GrowthHabit).HasColumnName("growth_habit");

            vi.OwnsOne<Leaf>(v => v.Leaf, leaf =>
            {                                                                                                       
                leaf.WithOwner() .HasForeignKey("Id");
                leaf.HasKey("Id");
                leaf.Property(l => l.Shape)
                    .HasColumnName("leaf_shape");
                leaf.Property(l => l.RelativeSize)
                    .HasColumnName("leaf_relative_size");
                leaf.Property(l => l.Edge)
                    .HasColumnName("leaf_edge");
                leaf.Property(l => l.Pattern)
                    .HasColumnName("leaf_pattern");
                leaf.Property(l => l.MainColors)
                    .HasColumnName("leaf_main_colors");
                leaf.Property(l => l.Texture)
                    .HasColumnName("leaf_texture")
                    .HasConversion(
                    v => string.Join(",", v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                    stringListComparer
            );

        leaf.Property(l => l.SecondaryColor)
            .HasColumnName("leaf_secondary_color")
            .HasConversion(
            v => string.Join(",", v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            stringListComparer
            );
        });

        vi.OwnsOne<Flower>(v => v.Flower, flower =>
        {
        flower.WithOwner() .HasForeignKey("Id");
        flower.HasKey("Id");
        flower.Property(f => f.Present)
            .HasColumnName("flower_present");
        flower.Property(f => f.Shape)
            .HasColumnName("flower_shape");
        flower.Property(f => f.Fragance)
            .HasColumnName("flower_fragrance");
        flower.Property(f => f.Color)
            .HasColumnName("flower_color")
            .HasConversion(
            v => string.Join(",", v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            stringListComparer
            );
        });

        vi.OwnsOne<Fruit>(v => v.Fruit, fruit => {
        fruit.WithOwner().HasForeignKey("Id");
        fruit.HasKey("Id");
        fruit.Property(f => f.Present)
            .HasColumnName("fruit_present");
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

        //  |: Planning Context
        builder.Entity<WateringLog>().ToTable("WateringLogs");
        builder.Entity<WateringLog>().HasKey(dl => dl.Id);
        builder.Entity<WateringLog>().Property(dl => dl.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<WateringLog>().Property(dl => dl.DeviceId).IsRequired();
        builder.Entity<WateringLog>().Property(dl => dl.DurationSeconds).IsRequired();
        builder.Entity<WateringLog>().Property(dl => dl.WaterVolumeMl).IsRequired();
        builder.Entity<WateringLog>().Property(dl => dl.WasSuccessful).IsRequired();
        builder.Entity<WateringLog>().Property(dl => dl.Reason).IsRequired().HasMaxLength(500);
        builder.Entity<WateringLog>().Property(dl => dl.Timestamp).IsRequired();
        
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
    }
}
