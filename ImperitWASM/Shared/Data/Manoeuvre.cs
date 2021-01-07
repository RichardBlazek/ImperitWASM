using System.Linq;

namespace ImperitWASM.Shared.Data
{
	public record Manoeuvre : Action
	{
		public virtual Province Province { get; private set; }
		public virtual Soldiers Soldiers { get; private set; }
		public Manoeuvre(Province province, Soldiers soldiers)
		{
			Province = province;
			Soldiers = soldiers;
		}

		public override (Player, Provinces, Action?) Perform(Player active, Provinces provinces, Settings settings)
		{
			return (active, provinces.With(provinces.Select(altered => altered == Province ? altered.VisitedBy(active, Soldiers) : altered)), null);
		}
	}
}
