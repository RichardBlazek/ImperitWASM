﻿using ImperitWASM.Client.Data;
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
			string path = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory ?? ".", "Files/imperit.db");
			_ = opt.UseSqlite("Data Source=" + path);
		}
		protected override void OnModelCreating(ModelBuilder mod)
		{
			_ = mod.Entity<Settings>(settings =>
			{
				_ = settings.OwnsOne(s => s.LandColor);
				_ = settings.OwnsOne(s => s.MountainsColor);
				_ = settings.OwnsOne(s => s.SeaColor);
				_ = settings.HasCustomKey(s => s.CountdownSeconds);
			}).Entity<Province>(province =>
			{
				_ = province.HasOne(p => p.Player).WithMany().Cascade();
				_ = province.HasOne(p => p.Settings).WithMany().Required();
				_ = province.HasOne(p => p.Soldiers).WithOne().Required();
				_ = province.HasOne(p => p.Region).WithMany().Required().HasForeignKey(p => p.RegionId);
				_ = province.HasOne<Game>().WithMany().Required().HasForeignKey(p => p.GameId);
				_ = province.HasCustomKey(p => new { p.GameId, p.RegionId });
			}).Entity<Region>(region =>
			{
				_ = region.HasOne(r => r.Settings).WithMany().Required();
				_ = region.HasOne(r => r.Soldiers).WithOne().Required();
				_ = region.HasMany(r => r.ProvinceSoldierTypes).WithOne().Required();
			}).Entity<Player>(player =>
			{
				_ = player.OwnsOne(p => p.Color);
				_ = player.HasOne(p => p.Settings).WithMany().Required();
				_ = player.HasMany(p => p.ActionList).WithOne().Required();
				_ = player.HasOne<Game>().WithMany().Required().HasForeignKey(p => p.GameId);
				_ = player.HasCustomKey(p => p.Name);
			}).Entity<Session>(session =>
			{
				_ = session.HasOne<Player>().WithMany().HasForeignKey(s => s.P).Required();
				_ = session.HasCustomKey(s => s.Key);
			}).Entity<SoldierType>(type => type.HasCustomKey(t => t.Symbol))
			  .Entity<ProvinceSoldierType>(recruitable => recruitable.HasOne(r => r.SoldierType).WithMany().HasForeignKey(r => r.SoldierTypeId).Required())
			  .Entity<Soldiers>(soldiers => soldiers.HasMany(s => s.Regiments).WithOne().Required())
			  .Entity<Regiment>(regiment => regiment.HasOne(r => r.Type).WithMany().Required())
			  .Entity<Power>(power => power.HasOne<Player>().WithMany().HasForeignKey(p => p.PlayerName))
			  .Entity<Game>(game => game.HasCustomKey(g => g.Id));
		}
	}
}
