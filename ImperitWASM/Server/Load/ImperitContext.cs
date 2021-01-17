using ImperitWASM.Client.Data;
using ImperitWASM.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Load
{
	public class ImperitContext : DbContext
	{
		public DbSet<Session>? Sessions { get; set; }
		public DbSet<Game>? Games { get; set; }
		public DbSet<Player>? Players { get; set; }
		public DbSet<Province>? Provinces { get; set; }
		public DbSet<Power>? Powers { get; set; }
		public DbSet<Settings>? Settings { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder opt)
		{
			_ = opt.UseSqlite("Data Source=" + System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory ?? ".", "Files/imperit.db"));
			_ = opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
		}
		protected override void OnModelCreating(ModelBuilder mod)
		{
			_ = mod.Entity<Settings>(settings =>
			{
				_ = settings.OwnsOne(s => s.LandColor);
				_ = settings.OwnsOne(s => s.MountainsColor);
				_ = settings.OwnsOne(s => s.SeaColor);
				_ = settings.Ignore(s => s.Regions).HasMany(s => s.RegionCollection).WithOne(r => r.Settings).Required();
				_ = settings.Ignore(s => s.SoldierTypes).HasMany(s => s.SoldierTypeCollection).WithOne().Required();
			}).Entity<Province>(province =>
			{
				_ = province.HasKey(p => new { p.GameId, p.RegionId });
				_ = province.HasOne(p => p.Player).WithMany().Cascade();
				_ = province.HasOne(p => p.Settings).WithMany().Required();
				_ = province.HasOne(p => p.Soldiers).WithOne().HasPrincipalKey<Soldiers>(s => s.Id).Required();
				_ = province.HasOne(p => p.Region).WithMany().HasForeignKey(p => p.RegionId).Required();
				_ = province.HasOne<Game>().WithMany().HasForeignKey(p => p.GameId).Required();
				_ = province.Ignore(p => p.Center).Ignore(p => p.Border);
			}).Entity<Region>(region =>
			{
				_ = region.HasOne(r => r.Soldiers).WithOne().HasForeignKey<Region>(r => r.SoldiersId).Required();
				_ = region.HasMany(r => r.RegionSoldierTypes).WithOne().Required();
				_ = region.HasOne(r => r.Shape).WithOne().HasForeignKey<Region>(r => r.ShapeId).Required();
				_ = region.Ignore(r => r.Center).Ignore(r => r.Border);
			}).Entity<Player>(player =>
			{
				_ = player.OwnsOne(p => p.Color);
				_ = player.HasOne(p => p.Settings).WithMany().Required();
				_ = player.Ignore(p => p.Actions).HasMany(p => p.ActionList).WithOne().Required();
				_ = player.HasOne<Game>().WithMany().Required().HasForeignKey(p => p.GameId);
				_ = player.HasCustomKey(p => p.Name);
			}).Entity<Session>(session =>
			{
				_ = session.HasOne<Player>().WithMany().HasForeignKey(s => s.P).Required();
				_ = session.HasCustomKey(s => s.Key);
			}).Entity<Elephant>(elephant =>
			{
				_ = elephant.Property(e => e.Capacity).HasColumnName("Capacity");
				_ = elephant.Property(e => e.Speed).HasColumnName("Speed");
			}).Entity<Manoeuvre>(manoeuvre => manoeuvre.HasOne(m => m.Soldiers).WithOne().HasForeignKey<Manoeuvre>(m => m.SoldiersId).Required())
			  .Entity<Shape>(shape => shape.Ignore(s => s.Border).HasOne(s => s.Center).WithOne().HasForeignKey<Shape>(s => s.CenterId).Required())
			  .Entity<SoldierType>(type => type.HasCustomKey(t => t.Symbol))
			  .Entity<RegionSoldierType>(recruitable => recruitable.HasOne(r => r.SoldierType).WithMany().Required())
			  .Entity<Soldiers>(soldiers => soldiers.Ignore(s => s.Types).HasMany(s => s.Regiments).WithOne().Required())
			  .Entity<Regiment>(regiment => regiment.HasOne(r => r.Type).WithMany().Required())
			  .Entity<Game>(game => { }).Entity<Action>(action => { }).Entity<Loan>(loan => { })
			  .Entity<Human>(human => { }).Entity<Robot>(robot => { }).Entity<Land>(land => { })
			  .Entity<Sea>(sea => { }).Entity<Mountains>(mountains => { }).Entity<Pedestrian>(_ => { })
			  .Entity<Ship>(ship => ship.Property(s => s.Capacity).HasColumnName("Capacity"))
			  .Entity<OutlandishShip>(ship => ship.Property(os => os.Speed).HasColumnName("Speed"));

			base.OnModelCreating(mod);
		}
	}
}
