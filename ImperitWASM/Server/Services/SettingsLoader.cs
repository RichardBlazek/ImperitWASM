using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using ImperitWASM.Server.Load;
using ImperitWASM.Shared;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ImperitWASM.Server.Services
{
	public interface ISettings
	{
		Settings Settings { get; }
		ImmutableArray<SoldierType> SoldierTypes { get; }
		ImmutableArray<Region> Regions { get; }
		Graph Graph { get; }
		IEnumerable<SoldierType> RecruitableIn(int i);
		List<Province> Provinces(int gameId);
		Player CreateHuman(string name, int gameId, int i, int land, Password password);
		List<(int, Player)> GetRobots(int gameId, int previous_count, IEnumerable<int> lands, Func<string, int, string> obf);
	}
	public class SettingsLoader : ISettings
	{
		public Settings Settings { get; }
		public ImmutableArray<SoldierType> SoldierTypes { get; }
		public ImmutableArray<Region> Regions { get; }
		public Graph Graph { get; }
		static T Load<T>(params string[] parts) => JsonConvert.DeserializeObject<T>(File.ReadAllText(Path.Combine(parts)));
		public SettingsLoader(string parent, string settings, string graph)
		{
			Settings = Load<Settings>(parent, settings);
			Graph = new Graph(Load<ImmutableArray<ImmutableArray<int>>>(parent, graph));
			using var ctx = new ImperitContext();
			SoldierTypes = ctx.SoldierType!.AsTracking().OrderBy(t => t.Symbol).ToImmutableArray();
			Regions = ctx.Region!.AsTracking().Include(region => region.Shape).ThenInclude(shape => shape.Center)
				.Include(region => region.Shape).ThenInclude(shape => shape.Points).Include(region => region.RegionSoldierTypes).ThenInclude(pst => pst.SoldierType)
				.Include(region => region.Soldiers).ThenInclude(soldiers => soldiers.Regiments).ThenInclude(regiment => regiment.Type).ToImmutableArray();
		}
		public IEnumerable<SoldierType> RecruitableIn(int i) => SoldierTypes!.Where(t => t.IsRecruitable(Regions[i]));
		public List<Province> Provinces(int gameId) => Regions!.Select(reg => new Province(gameId, reg, reg.Soldiers, Settings)).ToList();
		int StartMoney(int province) => Settings.DefaultMoney - (Regions[province].Income * 4);
		
		static readonly string Vowels = "aeiouy", Consonant = "bcdfghjklmnprstvz";
		static char IndexChar(bool vowel, int i) => vowel ? Vowels[i % Vowels.Length] : Consonant[i % Consonant.Length];
		static char UpperIf(bool upper, char c) => upper ? char.ToUpperInvariant(c) : c;
		static string GenerateName(int seed) => new string((4 + seed % 6).Range(i => UpperIf(i == 0, IndexChar(seed % 2 == i % 2, (i + 1) * (seed + i) - seed / 2))).ToArray());
		static string GetName(int i, Func<string, int, string> obf) => obf(GenerateName(i), i / 6);
		Player CreateRobot(string name, int gameId, int i, int land) => new Player(name, gameId, i, StartMoney(land), alive: true, isActive: false, isHuman: false, new Password());
		public Player CreateHuman(string name, int gameId, int i, int land, Password password) => new Player(name, gameId, i, StartMoney(land), alive: true, isActive: i == 0, isHuman: true, password);
		public List<(int, Player)> GetRobots(int gameId, int previous_count, IEnumerable<int> lands, Func<string, int, string> obf) => lands.Select((land, i) => (land, CreateRobot(GetName(previous_count + i, obf), gameId, previous_count + i, land))).ToList();

	}
}
