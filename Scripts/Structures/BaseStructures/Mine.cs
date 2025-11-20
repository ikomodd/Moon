using Godot;
using System;
using System.Collections.Generic;

public partial class Mine : Structure
{
	Godot.Collections.Array<string> MiningElements = new Godot.Collections.Array<string> { };

	public float Efficiency = 1.0f;

	//

	public void WorkMine()
	{
		foreach (String Element in MiningElements)
		{
			int MineredSize = (int)Math.Round(Efficiency / MiningElements.Count);

			GameService.AddInStorage(Element, MineredSize);
		}
	}

	public void ScanMine()
	{
		Control MineralsLabelParent = GetNode<Sprite2D>("StructureSprite").GetNode<Panel>("Panel").GetNode<VBoxContainer>("VBoxContainer");

		foreach (KeyValuePair<string, FastNoiseLite> MineralNoise in GameService.MineralMap)
		{
			float NoiseData = MineralNoise.Value.GetNoise2D(Position.X, Position.Y);

			if (NoiseData > GameService.MineRarity[MineralNoise.Key])
			{
				if (!(MineralsLabelParent.HasNode(MineralNoise.Key)))
				{
					RichTextLabel LabelClone = MineralsLabelParent.GetNode<RichTextLabel>("BaseElement").Duplicate() as RichTextLabel;

					LabelClone.Name = MineralNoise.Key;
					LabelClone.Text = " " + MineralNoise.Key;

					LabelClone.Visible = true;

					MineralsLabelParent.AddChild(LabelClone);
				}
			}
			else
			{
				if (MineralsLabelParent.HasNode(MineralNoise.Key)) MineralsLabelParent.GetNode<RichTextLabel>(MineralNoise.Key).QueueFree();
			}
		}
	}

	//

	public override void _StartPlaceAction()
	{
		base._StartPlaceAction();



	}

	public override void _EndPlaceAction()
	{
		base._EndPlaceAction();

		foreach (KeyValuePair<string, FastNoiseLite> MineralNoise in GameService.MineralMap)
		{
			float NoiseData = MineralNoise.Value.GetNoise2D(Position.X, Position.Y);

			if (NoiseData > GameService.MineRarity[MineralNoise.Key]) MiningElements.Add(MineralNoise.Key);
		}

		GD.Print(MiningElements);

		GetNode<Sprite2D>("StructureSprite").GetNode<Panel>("Panel").QueueFree();
	}
	
	public override void _PlacingAction()
	{
		base._PlacingAction();

		ScanMine();
	}

	//
}
