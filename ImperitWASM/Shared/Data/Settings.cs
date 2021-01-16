using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public sealed record Settings
	{
		public int Id { get; private set; }
		public int CountdownSeconds { get; private set; }
		public int IntegerDefaultInstability { get; private set; }
		public int DebtLimit { get; private set; }
		public int FinalLandsCount { get; private set; }
		public int IntegerInterest { get; private set; }
		public Color LandColor { get; private set; }
		public Color MountainsColor { get; private set; }
		public float MountainsWidth { get; private set; }
		public Color SeaColor { get; private set; }
		public int DefaultMoney { get; private set; }
		public int PlayerCount { get; private set; }
		public ICollection<SoldierType>? SoldierTypeCollection { get; private set; }
		public ICollection<Region>? RegionCollection { get; private set; }
		public Settings(Color landColor, Color mountainsColor, Color seaColor)
		{
			LandColor = landColor;
			MountainsColor = mountainsColor;
			SeaColor = seaColor;
		}

		public Ratio DefaultInstability => new Ratio(IntegerDefaultInstability);
		public Ratio Interest => new Ratio(IntegerInterest);
		public ImmutableArray<SoldierType> SoldierTypes => SoldierTypeCollection!.OrderBy(type => type.Symbol).ToImmutableArray();
		public ImmutableArray<Region> Regions => RegionCollection!.OrderBy(region => region.Id).ToImmutableArray();

		public TimeSpan Countdown => TimeSpan.FromSeconds(CountdownSeconds);
		public IEnumerable<SoldierType> RecruitableIn(int i) => SoldierTypeCollection!.Where(t => t.IsRecruitable(Regions[i]));
		public IEnumerable<SoldierType> RecruitableIn(Province where) => SoldierTypeCollection!.Where(t => t.IsRecruitable(where.Region));

		public int CalculateDebt(int amount) => Interest.Interest(amount);
		public int Discount(int amount) => Interest.Discount(amount);
		public static int LandPrice(Soldiers soldiers, int earnings) => soldiers.Price + soldiers.DefensePower + earnings;
		public Ratio Instability(Soldiers now, Soldiers start) => DefaultInstability.Adjust(Math.Max(start.DefensePower - now.DefensePower, 0), start.DefensePower);

		static readonly string Vowels = "aeiou", Consonant = "bcdfghjklmnprstvz";

		static char IndexChar(bool vowel, int i) => vowel ? Vowels[i % Vowels.Length] : Consonant[i % Consonant.Length];
		static char UpperIf(bool upper, char c) => upper ? char.ToUpperInvariant(c) : c;
		static string GenerateName(int seed) => new string((4 + seed % 6).Range(i => UpperIf(i == 0, IndexChar(seed % 2 == i % 2, (i + 1) * (seed + i) - seed / 2))).ToArray());
		static string GetName(int i, Func<string, int, string> obf) => obf(GenerateName(i), i / 6);

		public static Color ColorOf(int i) => Color.Generate(i, 120.0, 1.0, 1.0);
		public Human CreateHuman(string name, int gameId, int i, int land, Password password) => new Human(name, gameId, i, ColorOf(i), StartMoney(land), true, this, i == 0, password);
		Robot CreateRobot(string name, int gameId, int i, int land) => new Robot(name, gameId, i, ColorOf(i), StartMoney(land), true, this, false);

		public IEnumerable<Province> Provinces(int gameId) => RegionCollection!.Select(reg => new Province(gameId, reg, reg.Soldiers, this));

		int StartMoney(int province) => DefaultMoney - (RegionCollection!.Single(r => r.Id == province).Income * 4);
		public IEnumerable<(int, Robot)> GetRobots(int gameId, int previous_count, IEnumerable<int> lands, Func<string, int, string> obf) => lands.Select((land, i) => (land, CreateRobot(GetName(previous_count + i, obf), gameId, previous_count + i, land)));

		public bool Equals(Settings? other) => true;
		public override int GetHashCode() => -1;
		public override string ToString() => string.Empty;

#pragma warning disable CS8618
		private Settings() { }
#pragma warning restore CS8618
	}
}