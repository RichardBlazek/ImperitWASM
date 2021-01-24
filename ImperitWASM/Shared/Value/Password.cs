using System;
using System.Security.Cryptography;
using System.Text;

namespace ImperitWASM.Shared.Value
{
	public readonly struct Password : IEquatable<Password>
	{
		static readonly SHA256 sha = SHA256.Create();
		string Hash { get; }
		public Password(string hash) => Hash = hash;
		public static Password FromPassword(string str) => new Password(Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(str))));
		public bool Equals(Password other) => string.Equals(Hash, other.Hash);
		public bool IsCorrect(string other) => other is not null && Equals(FromPassword(other));
		public override bool Equals(object? obj) => obj is Password pw && Equals(pw);
		public static bool operator ==(Password x, Password y) => x.Equals(y);
		public static bool operator !=(Password x, Password y) => !x.Equals(y);
		public override int GetHashCode() => ToString().GetHashCode();
		public override string ToString() => Hash ?? string.Empty;
	}
}