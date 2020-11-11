﻿using ImperitWASM.Shared.State;

namespace ImperitWASM.Client.Data
{
	public class ProvinceInstability
	{
		public string Name { get; set; } = "";
		public Color Color { get; set; }
		public Ratio Instability { get; set; }
		public ProvinceInstability() { }
		public ProvinceInstability(string name, Color color, Ratio instability)
		{
			Name = name;
			Color = color;
			Instability = instability;
		}
	}
}