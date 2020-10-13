﻿using ImperitWASM.Shared.Motion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ImperitWASM.Shared.State
{
	public class PlayersAndProvinces : IProvinces, IReadOnlyList<Player>
	{
		public readonly ImmutableArray<Player> Players;
		public readonly Provinces Provinces;
		readonly int active;
		public PlayersAndProvinces(ImmutableArray<Player> players, Provinces provinces, int active)
		{
			Players = players;
			Provinces = provinces;
			this.active = active;
		}
		public bool Passable(int from, int to, int distance, Func<Province, Province, int> difficulty) => Provinces.Passable(from, to, distance, difficulty);
		public bool Passable(int from, int to, int distance = 1) => Passable(from, to, distance, (a, b) => 1);
		public int NeighborCount(Province prov) => Provinces.NeighborCount(prov.Id);
		public IEnumerable<Province> NeighborsOf(Province prov) => Provinces.NeighborsOf(prov.Id);
		public int IncomeOf(Player player) => Provinces.Where(p => p.IsAllyOf(player)).Sum(p => p.Earnings);
		public bool HasAny(Player player) => Provinces.Any(p => p.IsAllyOf(player));
		public int PlayersCount => Players.Length;
		public int ProvincesCount => Provinces.Count;
		public Player Player(int i) => Players[i];
		public Province Province(int i) => Provinces[i];
		public IEnumerable<T> EachPlayer<T>(Func<Player, T> fn) => Players.Select(p => fn(p));
		public IEnumerable<T> EachProvince<T>(Func<Province, T> fn) => Provinces.Select(p => fn(p));
		public PlayersAndProvinces Act(bool includePlayerActions = true)
		{
			var new_players = Players.ToBuilder();
			var new_provinces = Provinces.ToBuilder();
			for (int i = 0; i < Provinces.Count; ++i)
			{
				Player activeplayer = new_players[active];
				(new_provinces[i], new_players) = new_provinces[i].Act(new PlayersAndProvinces(new_players.MoveToImmutable(), Provinces, active), activeplayer);
			}
			if (includePlayerActions)
			{
				(new_provinces, new_players[active]) = new_players[active].Act(new PlayersAndProvinces(new_players.ToImmutable(), Provinces.With(new_provinces.MoveToImmutable()), active));
			}
			return new PlayersAndProvinces(new_players.MoveToImmutable(), Provinces.With(new_provinces.MoveToImmutable()), active);
		}
		public (PlayersAndProvinces, bool) Do(ICommand cmd)
		{
			if (cmd.Allowed(this))
			{
				var new_players = Players.Select(player => cmd.Perform(player, this)).ToImmutableArray();
				var new_provinces = Provinces.With(Provinces.Select(province => cmd.Perform(province)).ToImmutableArray());
				return (new PlayersAndProvinces(new_players, new_provinces, active), true);
			}
			return (new PlayersAndProvinces(Players, Provinces, active), false);
		}
		public PlayersAndProvinces RemovePlayers()
		{
			var new_provinces = Provinces.With(Provinces.Select(p => p.Revolt()).ToImmutableArray());
			return new PlayersAndProvinces(ImmutableArray<Player>.Empty, new_provinces, 0);
		}
		public PlayersAndProvinces Add(IEnumerable<(Player, Soldiers, int)> starts)
		{
			var new_provinces = Provinces.ToBuilder();
			foreach (var (player, soldiers, where) in starts)
			{
				new_provinces[where] = new_provinces[where].GiveUpTo(new Army(soldiers, player));
			}
			return new PlayersAndProvinces(Players.AddRange(starts.Select(s => s.Item1)), Provinces.With(new_provinces.MoveToImmutable()), active);
		}
		public PlayersAndProvinces Add(Player p) => new PlayersAndProvinces(Players.Add(p), Provinces, active);
		public PlayersAndProvinces Next() => new PlayersAndProvinces(Players, Provinces, Players.FirstRotated(active + 1, p => p.Alive && !(p is Savage)));
		public int LivingHumans => Players.Count(p => !(p is Robot) && !(p is Savage));
		public Player Active => Players[active];
		public override string ToString() => active.ToString(ExtMethods.Culture);
		public ImmutableArray<T> Compute<T, TP, TK>(Func<Player, TP> player, Func<IEnumerable<Province>, TK> province, Func<TP, TK, T> selector)
		{
			var builder = ImmutableArray.CreateBuilder<T>(PlayersCount);
			for (int i = 0; i < PlayersCount; ++i)
			{
				builder.Add(selector(player(Players[i]), province(Provinces.Where(p => p.IsAllyOf(Players[i])))));
			}
			return builder.MoveToImmutable();
		}

		//Interface implementations -------------------------------------------
		Provinces IProvinces.With(ImmutableArray<Province> new_provinces) => Provinces.With(new_provinces);
		ImmutableArray<Province>.Builder IProvinces.ToBuilder() => Provinces.ToBuilder();
		IEnumerator<Province> IEnumerable<Province>.GetEnumerator() => Provinces.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => Provinces.GetEnumerator();
		int IProvinces.NeighborCount(int prov) => NeighborCount(Provinces[prov]);
		IEnumerable<Province> IProvinces.NeighborsOf(int prov) => NeighborsOf(Provinces[prov]);

		int IReadOnlyCollection<Province>.Count => Provinces.Count;
		Province IReadOnlyList<Province>.this[int index] => Provinces[index];
		IEnumerator<Player> IEnumerable<Player>.GetEnumerator() => (Players as IEnumerable<Player>).GetEnumerator();
		int IReadOnlyCollection<Player>.Count => Players.Length;
		Player IReadOnlyList<Player>.this[int index] => Players[index];
	}
}
