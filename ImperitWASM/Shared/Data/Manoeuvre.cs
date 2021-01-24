using System.Linq;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Manoeuvre : Action
	{
		public int RegionId { get; private set; }
		public int SoldiersId { get; private set; }
		public Soldiers Soldiers { get; private set; }
		public Manoeuvre(int regionId, Soldiers soldiers)
		{
			RegionId = regionId;
			Soldiers = soldiers;
		}

		public override (Player, Provinces, Action?) Perform(Player active, Provinces provinces, Settings settings)
		{
			return (active, provinces.With(provinces.Select(altered => altered.RegionId == RegionId ? altered.VisitedBy(active, Soldiers) : altered)), null);
		}

#pragma warning disable CS8618
		private Manoeuvre() { }
#pragma warning restore CS8618
	}
}
