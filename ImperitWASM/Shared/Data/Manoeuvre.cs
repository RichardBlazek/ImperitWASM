using System.Linq;

namespace ImperitWASM.Shared.Data
{
	public record Manoeuvre : Action
	{
		public int ProvinceId { get; private set; }
		public int SoldiersId { get; private set; }
		public Soldiers Soldiers { get; private set; }
		public Manoeuvre(int provinceId, Soldiers soldiers)
		{
			ProvinceId = provinceId;
			Soldiers = soldiers;
		}

		public override (Player, Provinces, Action?) Perform(Player active, Provinces provinces, Settings settings)
		{
			return (active, provinces.With(provinces.Select(altered => altered.Id == ProvinceId ? altered.VisitedBy(active, Soldiers) : altered)), null);
		}

#pragma warning disable CS8618
		private Manoeuvre() { }
#pragma warning restore CS8618
	}
}
