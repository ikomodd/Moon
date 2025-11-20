using Godot;
using System;

public partial class SolarPainelModule : Structure
{
	public override void _Ready()
	{
		base._Ready();

		Price = 250000000;

	}
	public override void _EndPlaceAction()
	{
		base._EndPlaceAction();

		GameService.EletricProduction += 10;
	}
}
