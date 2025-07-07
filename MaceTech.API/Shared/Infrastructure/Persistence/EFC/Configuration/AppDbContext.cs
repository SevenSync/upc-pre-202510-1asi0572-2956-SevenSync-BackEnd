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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MaceTech.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<PotRecord> PotRecords { get; set; }
    public DbSet<Alert> Alerts { get; set; }
    public DbSet<Plant> Plants { get; set; }
    public DbSet<DevicePlant> DevicePlants { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Pot> Pots { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        builder.AddCreatedUpdatedInterceptor();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);


        //                                                                                                                         | : IAM Context                    
        builder.Entity<User>().HasKey(u => u.Uid);
        builder.Entity<User>().Property(u => u.Uid).IsRequired();
        builder.Entity<User>().Property(u => u.Email).IsRequired();
        builder.Entity<User>().Property(u => u.TokenVersion).IsRequired();
        builder.Entity<User>().Property(u => u.Status).IsRequired();

        //                                                                                                                         | : Profiles Context               
        builder.Entity<Profile>().HasKey(p => p.Id);
        builder.Entity<Profile>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Profile>().Property(p => p.Uid).IsRequired();
        builder.Entity<Profile>().OwnsOne<PersonName>(p => p.Name, n =>
        {
            n.WithOwner()
              .HasForeignKey("Id");   // apunta a Profiles.id
            n.HasKey("Id");
            n.Property(p => p.FirstName);
            n.Property(p => p.LastName);
        });
        builder.Entity<Profile>().OwnsOne<PersonAddress>(p => p.Address,
        a =>
        {
            // Reutiliza Profile.Id
            a.WithOwner()
              .HasForeignKey("Id");   // apunta a Profiles.id
            a.HasKey("Id");           // esa misma columna es PK de PersonAddress
            a.Property(s => s.Street);
            a.Property(s => s.Number);
            a.Property(s => s.City);
            a.Property(s => s.PostalCode);
            a.Property(s => s.Country);
        });
        builder.Entity<Profile>().OwnsOne<PhoneNumber>(p => p.PhoneNumber,
        a =>
        {
            a.WithOwner()
                                  .HasForeignKey("Id");   // apunta a Profiles.id
            a.HasKey("Id");           // esa misma columna es PK de PersonAddress
            a.Property(s => s.CountryCode);
            a.Property(s => s.Number);
        });

        //                                                                                                                         | : Subscriptions Context          
        builder.Entity<Subscription>().HasKey(s => s.Id);
        builder.Entity<Subscription>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Subscription>().Property(s => s.Uid).IsRequired();
        builder.Entity<Subscription>().Property(s => s.Plan).IsRequired();

        //                                                                                                                         | : Pot Context                    
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

        //                                                                                                                         | : Analytics Context              
        builder.Entity<Alert>().ToTable("Alerts");
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Alert>().Property(a => a.DeviceId).IsRequired();
        builder.Entity<Alert>().Property(a => a.AlertType).IsRequired();
        builder.Entity<Alert>().Property(a => a.TriggerValue).IsRequired();
        builder.Entity<Alert>().Property(a => a.Timestamp).IsRequired();

        // Configuración de la relación con Recommendation                                                                                                            
        builder.Entity<Alert>()
        .OwnsOne(a => a.GeneratedRecommendation, r =>
        {
            r.WithOwner()
    .HasForeignKey("Id");

            r.HasKey("Id");

            r.Property(re => re.Text)
    .IsRequired()
    .HasMaxLength(500);
            r.Property(re => re.Urgency)
    .IsRequired()
    .HasMaxLength(50);
            r.Property(re => re.GuideUrl)
    .HasMaxLength(255);
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

        //                                                                                                                         | : Planning Context               
        builder.Entity<Plant>().ToTable("Plants");
        builder.Entity<Plant>().HasKey(p => p.Id);
        builder.Entity<Plant>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Plant>().Property(p => p.CommonName).IsRequired().HasMaxLength(100);
        builder.Entity<Plant>().Property(p => p.ScientificName).HasMaxLength(150);
        builder.Entity<Plant>().Property(p => p.ImageUrl).HasMaxLength(255);
        builder.Entity<Plant>().Property(p => p.Description).HasMaxLength(1000);

        // Defining the ValueComparer once to reuse it, as suggested by best practices.                                                                               
        var stringListComparer = new ValueComparer<List<string>>(
        (c1, c2) => c1.SequenceEqual(c2),
        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
        c => c.ToList());

        // Mapping of the owned type OptimalParameters                                                                                                                
        builder.Entity<Plant>().OwnsOne<OptimalParameters>(p => p.OptimalParameters,
        parameters =>
        {// Compartir PK 'Id'
            parameters.WithOwner()
              .HasForeignKey("Id");
            parameters.HasKey("Id");

            // Nested Ranges: Temperature, Humidity, Light, Salinity, Ph
            parameters.OwnsOne<Range<double>>(po => po.Temperature,
   temp =>
   {
       temp.WithOwner().HasForeignKey("Id");
       temp.HasKey("Id");
       temp.Property(t => t.Min);
       temp.Property(t => t.Max);
   });
            parameters.OwnsOne<Range<double>>(po => po.Humidity,
    hum =>
    {
        hum.WithOwner().HasForeignKey("Id");
        hum.HasKey("Id");
        hum.Property(h => h.Min);
        hum.Property(h => h.Max);
    });
            parameters.OwnsOne<Range<double>>(po => po.Light,
    lum =>
    {
        lum.WithOwner().HasForeignKey("Id");
        lum.HasKey("Id");
        lum.Property(l => l.Min);
        lum.Property(l => l.Max);
    });
            parameters.OwnsOne<Range<double>>(po => po.Salinity,
    sal =>
    {
        sal.WithOwner().HasForeignKey("Id");
        sal.HasKey("Id");
        sal.Property(s => s.Min);
        sal.Property(s => s.Max);
    });
            parameters.OwnsOne<Range<double>>(po => po.Ph,
    ph =>
    {
        ph.WithOwner().HasForeignKey("Id");
        ph.HasKey("Id");
        ph.Property(p => p.Min);
        ph.Property(p => p.Max);
    });
        });

        // Mapping of the owned type SearchFilters                                                                                                                    
        builder.Entity<Plant>().OwnsOne<SearchFilters>(p => p.SearchFilters, sf =>
        {
            sf.WithOwner().HasForeignKey("Id");
            sf.HasKey("Id");
            sf.Property(s => s.Difficulty);
            sf.Property(s => s.Light);
            sf.Property(s => s.SizePotential);
            sf.Property(s => s.Ubication)
    .HasConversion(
    v => string.Join(",", v),                                          // first argument: convert from List<string> to string                                     
    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(), // second argument: convert from string to List<string>                                    
    stringListComparer                                                  // third argument: ValueComparer for List<string>                                         
    );

            sf.Property(s => s.Tags)
    .HasConversion(
    v => string.Join(",", v),
    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
    stringListComparer
    );
        });

        // Mapping of the owned type VisualIdentification                                                                                                             
        builder.Entity<Plant>().OwnsOne<VisualIdentification>(p => p.VisualIdentification, vi =>
        {
            // 1) Que el VisualIdentification comparta la PK 'id'                                                                                                         
            vi.WithOwner()
    .HasForeignKey("Id"); // apunta a Plants.id            
            vi.HasKey("Id"); // esa misma columna es su PK    

            vi.Property(v => v.GrowthHabit);

            vi.OwnsOne<Leaf>(v => v.Leaf, leaf =>
    {
        // aquí de nuevo: que el Leaf comparta el mismo 'id'                                                                                                          
        leaf.WithOwner()
.HasForeignKey("Id");
        leaf.HasKey("Id");
        leaf.Property(l => l.Shape);
        leaf.Property(l => l.RelativeSize);
        leaf.Property(l => l.Edge);
        leaf.Property(l => l.Pattern);
        leaf.Property(l => l.MainColors);
        leaf.Property(l => l.Texture)
.HasConversion(
v => string.Join(",", v),
v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
stringListComparer
);

        leaf.Property(l => l.SecondaryColor)
.HasConversion(
v => string.Join(",", v),
v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
stringListComparer
);
    });

            vi.OwnsOne<Flower>(v => v.Flower, flower =>
    {
        flower.WithOwner()
.HasForeignKey("Id");
        flower.HasKey("Id");
        flower.Property(f => f.Present);
        flower.Property(f => f.Shape);
        flower.Property(f => f.Fragance);
        flower.Property(f => f.Color)
.HasConversion(
v => string.Join(",", v),
v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
stringListComparer
)
                                                                                                                           ;
    });

            vi.OwnsOne<Fruit>(v => v.Fruit, fruit =>
    {
        fruit.WithOwner()
.HasForeignKey("Id");
        fruit.HasKey("Id");
        fruit.Property(f => f.Present);
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

        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
    }
}
