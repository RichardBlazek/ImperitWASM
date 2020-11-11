﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ImperitWASM.Shared.State;

namespace ImperitWASM.Server.Load
{
	public class EntityPlayerPower : IEntity
	{
		[Key] public int Id { get; set; }
		public EntityGame EntityGame { get; set; } = new EntityGame();
		public int EntityGameId { get; set; }
		public int TurnIndex { get; set; }
		public int PlayerIndex { get; set; }
		public bool Alive { get; set; }
		public int Soldiers { get; set; }
		public int Lands { get; set; }
		public int Income { get; set; }
		public int Money { get; set; }
		public int Final { get; set; }
		public PlayerPower Convert => new PlayerPower(Alive, Soldiers, Lands, Income, Money, Final);
		public static EntityPlayerPower From(PlayerPower pp, int gameId, int turn, int player) => new EntityPlayerPower { Alive = pp.Alive, Soldiers = pp.Soldiers, Lands = pp.Lands, Income = pp.Income, Money = pp.Money, Final = pp.Final, EntityGameId = gameId, PlayerIndex = player, TurnIndex = turn };
		public static PlayersPower ConvertOne(IEnumerable<EntityPlayerPower> epps) => new PlayersPower(epps.OrderBy(pp => pp.PlayerIndex).Select(pp => pp.Convert).ToImmutableArray());
		public static ImmutableArray<PlayersPower> ConvertMore(IEnumerable<EntityPlayerPower> pps) => pps.GroupBy(pp => pp.TurnIndex).OrderBy(pps => pps.Key).Select(ConvertOne).ToImmutableArray();

	}
}
