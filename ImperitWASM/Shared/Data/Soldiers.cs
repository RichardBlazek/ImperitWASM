﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ImperitWASM.Shared.Data
{
	public record Soldiers
	{
		public int Id { get; private set; }
		public virtual IList<Regiment> Regiments { get; private set; }
		public Soldiers() => Regiments = new List<Regiment>();
		public Soldiers(IList<Regiment> regiments) => Regiments = regiments;
		public Soldiers(IEnumerable<Regiment> regiments) => Regiments = regiments.ToList();
		public Soldiers(SoldierType type, int count) => Regiments = new List<Regiment> { new Regiment(type, count) };

		public Soldiers Add(Soldiers s)
		{
			return new Soldiers(Regiments.Combine(s.Regiments.Where(p => p.Count > 0), (a, b) => a.Type == b.Type, (a, b) => new Regiment(a.Type, a.Count + b.Count)));
		}
		public Soldiers Subtract(Soldiers s)
		{
			return new Soldiers(Regiments.Combine(s.Regiments.Where(p => p.Count > 0), (a, b) => a.Type == b.Type, (a, b) => new Regiment(a.Type, a.Count - b.Count)));
		}
		public bool Contains(Soldiers s)
		{
			return s.Regiments.All(s2 => Regiments.Any(s1 => s1.Type == s2.Type && s1.Count >= s2.Count));
		}
		public static Soldiers operator +(Soldiers s1, Soldiers s2) => s1.Add(s2);
		public static Soldiers operator -(Soldiers s1, Soldiers s2) => s1.Subtract(s2);
		public Soldiers Multiply(int mul) => new Soldiers(Regiments.Select(s => new Regiment(s.Type, s.Count * mul)));
		public IEnumerable<SoldierType> Types => Regiments.Select(p => p.Type);
		public int AttackPower => Regiments.Sum(p => p.AttackPower);
		public int DefensePower => Regiments.Sum(p => p.DefensePower);
		public int Power => Regiments.Sum(p => p.Power);
		public int Weight => Regiments.Sum(p => p.Weight);
		public int Price => Regiments.Sum(p => p.Price);
		public int Count => Regiments.Sum(p => p.Count);
		public bool Any => Regiments.Any(p => p.Count > 0);

		public int Length => Regiments.Count;
		public Regiment this[int index] => Regiments[index];

		public int CountOf(SoldierType type) => Regiments.FirstOrDefault(regiment => regiment.Type == type)?.Count ?? 0;
		public int Capacity(Provinces provinces, Province from, Province to)
		{
			return Regiments.Sum(p => p.CanMove(provinces, from, to) - p.Weight);
		}
		public bool CanMove(Provinces provinces, Province from, Province to)
		{
			return Any && from.Has(this) && Capacity(provinces, from, to) >= 0;
		}
		public bool CanAnyMove(Provinces provinces, Province from, Province to)
		{
			return Regiments.Any(reg => reg.CanMoveAlone(provinces, from, to));
		}
		public bool CanSurviveIn(Province province) => Regiments.Sum(p => p.CanSustain(province) - p.Weight) >= 0;

		int[] Fight(int enemy, Func<SoldierType, int> powerof)
		{
			int me = Regiments.Sum(regiment => powerof(regiment.Type)), died = 0;
			int[] remaining = new int[Regiments.Count];
			if (enemy > me)
			{
				return remaining;
			}
			for (int i = 0; i < remaining.Length; ++i)
			{
				if (powerof(Regiments[i].Type) > 0)
				{
					remaining[i] = Regiments[i].Count - (Regiments[i].Count * enemy / me);
					died += (Regiments[i].Count - remaining[i]) * powerof(Regiments[i].Type);
				}
				else
				{
					remaining[i] = Regiments[i].Count;
				}
			}
			for (int i = 0; died < enemy && i < remaining.Length; ++i)
			{
				if (powerof(Regiments[i].Type) > 0)
				{
					int d = Math.Min(remaining[i], ((enemy - died) / powerof(Regiments[i].Type)) + 1);
					remaining[i] -= d;
					died += d * powerof(Regiments[i].Type);
				}

			}
			return remaining;
		}
		public Soldiers FightAgainst(int power, Func<SoldierType, int> powerof)
		{
			return new Soldiers(Fight(power, powerof).Select((count, i) => new Regiment(Regiments[i].Type, count)).Where(pair => pair.Count > 0));
		}
		public Soldiers AttackedBy(Soldiers s2)
		{
			int defensePower = DefensePower, attackPower = s2.AttackPower;
			return defensePower >= attackPower
					? FightAgainst(attackPower, type => type.DefensePower)
					: s2.FightAgainst(defensePower, type => type.AttackPower);
		}
		public Soldiers MaxMovable(Provinces provinces, Province from, Province to)
		{
			var result = new Soldiers(Regiments.Where(p => p.CanMoveAlone(provinces, from, to)));
			foreach (var p in Regiments.Where(p => !p.CanMoveAlone(provinces, from, to)).OrderBy(p => p.Type.Weight - p.Type.CanMove(provinces, from, to)))
			{
				result += new Soldiers(p.Type, Math.Min(result.Capacity(provinces, from, to) / (p.Type.Weight - p.Type.CanMove(provinces, from, to)), p.Count));
			}
			return result;
		}
		public override string ToString() => string.Join("", Regiments);
	}
}
