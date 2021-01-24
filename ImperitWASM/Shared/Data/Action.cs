using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public abstract record Action
	{
		public int Id { get; private set; }
		public abstract (Player, Provinces, Action?) Perform(Player active, Provinces provinces, Settings settings);
	}
}