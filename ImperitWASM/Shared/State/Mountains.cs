using ImperitWASM.Shared.Motion;
using System.Collections.Immutable;

namespace ImperitWASM.Shared.State
{
	public class Mountains : Province
	{
		public Mountains(int id, string name, Shape shape, Army army, Army defaultArmy, ImmutableList<IAction> actions, Settings set)
			: base(id, name, shape, army, defaultArmy, 0, actions, set) { }
		public override Color Stroke => settings.MountainsColor;
		public override int StrokeWidth => settings.MountainsWidth;
		public override Province GiveUpTo(Army army) => new Mountains(Id, Name, Shape, army, DefaultArmy, Actions, settings);
		protected override Province WithActions(ImmutableList<IAction> new_actions) => new Mountains(Id, Name, Shape, Army, DefaultArmy, new_actions, settings);
	}
}