using Godot;
using System;
using System.Collections.Generic;

public partial class LunarMineModule : Mine
{
	public override void _Ready()
	{
		base._Ready();

		Price = 40000000;

		Efficiency = 160;
	}
	

	public override void _EndPlaceAction()
	{
		base._EndPlaceAction();

		GameService.EletricUse += 2;
	}

	public override void _RunAction()
	{
		base._RunAction();

		if (GameService.EletricProduction >= GameService.EletricUse)
		{
			Working = true;
			RemoveCaution("NoEnergy");

			WorkMine();
		}
		else
		{
			Working = false;
			AddCaution("NoEnergy");
		}
	}
}
