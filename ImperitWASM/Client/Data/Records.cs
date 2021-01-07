using System.Collections.Immutable;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Client.Data
{
	public sealed record Click(string P, int? From, int Clicked);
	public sealed record DonationCmd(string P, string Key, string To, int M);
	public sealed record GameInfo(Game.State? S, bool A);
	public sealed record HistoryRecord(ImmutableArray<ImmutableArray<Power>> Powers, ImmutableArray<Color> Colors, string Name, Color Color);
	public sealed record LoginResult(Session S, int I, int G);
	public sealed record MoveCmd(string P, string Key, int From, int To, ImmutableArray<int> Counts, int G);
	public sealed record MoveData(int F, int T, int G);
	public enum MoveErrors { Ok, FewSoldiers, LittleCapacity, NotPlaying, Else }
	public sealed record MoveInfo(bool Possible, string FromName, string ToName, string FromSoldiers, string ToSoldiers, ImmutableArray<int> Counts);
	public sealed record PlayerInfo(string N, Color C, bool A, int M, int Debt, int I);
	public sealed record ProvinceDisplay(ImmutableArray<Point> B, Point C, Color F, Color S, double W, ImmutableArray<string> T)
	{
		public Color GetColor() => F.Light() > 180 ? new Color(0, 0, 0) : new Color(255, 255, 255);
		public ProvinceDisplay Update(ProvinceUpdate u) => this with { F = u.F, T = u.T };
	}
	public sealed record ProvinceUpdate(ImmutableArray<string> T, Color F);
	public sealed record PurchaseCmd(string P, string Key, int L, int G);
	public sealed record PurchaseData(string P, int L);
	public sealed record PurchaseInfo(bool Possible, string Name, int Price, int Money);
	public sealed record RecruitCmd(string P, string Key, int Province, ImmutableArray<int> Counts, int G);
	public sealed record RecruitData(int W, string P);
	public sealed record RecruitInfo(string N, string S, ImmutableArray<bool> R, int M, Ratio I);
	public sealed record RegisteredPlayer(string N, string P, int S, int G);
	public enum RegistrationErrors { Ok, UsedName, NoName, InvalidStart, NoPassword, BadGame }
	public sealed record SoldierItem(string N, string S, string T, int P);
	public sealed record Switch(int? Select, View View, int? From, int? To);
	public enum View { Map, Donation, Move, Preview, Purchase, Recruit, Statistics }
	public sealed record Winner(string N, Color C);
}
