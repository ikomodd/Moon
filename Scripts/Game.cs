using Godot;
using System;

public partial class Game : Node2D
{
	private Node2D Structures = null;

	public Camera GameCamera = null;
	public Gui GameGui = null;

	public long Money = 1000000000;
	
	public float EletricProduction = 0;
	public float EletricUse = 0;

	public Godot.Collections.Dictionary<string, FastNoiseLite> MineralMap = new Godot.Collections.Dictionary<string, FastNoiseLite> { };
	public Godot.Collections.Dictionary<string, float> MineRarity = new Godot.Collections.Dictionary<string, float>
	{
		{ "Helium-3", 0.5f },
		{ "Iron",     0.2f },
		{ "Rock",     0.01f },
	};

	public Godot.Collections.Dictionary<string, long> Storage = new Godot.Collections.Dictionary<string, long> { };

	public Structure PlacingStructure = null;
	public bool DestroyMode = false;

	// Economic 

	public void UpdateMoney(long MoneyQuantity)
	{
		Money += MoneyQuantity;

		GameGui.UpperBarUi.UpdateMoneyLabel(Money);
	}

	// Terrain

	private int GetStringSeed(string Input)
	{
		int Hash = 17;

		foreach (char Letter in Input)
		{
			Hash *= 31 + Letter;
		}
		return Math.Abs(Hash);
	}

	private void CreateMineralNoise()
	{
		foreach (var Pair in MineRarity)
		{
			FastNoiseLite MineralNoise = new FastNoiseLite
			{
				Seed = GetStringSeed(Pair.Key),
				Frequency = 0.01f,
				NoiseType = FastNoiseLite.NoiseTypeEnum.Simplex,
				FractalOctaves = 3,
				FractalGain = 0.5f,
				FractalLacunarity = 2.0f
			};

			MineralMap.Add(Pair.Key, MineralNoise);
		}
	}
	
	// Structure

	public void TriggerDestroyMode(bool State)
	{
		if (State == true)
		{
			DestroyMode = true;

			ShaderMaterial ShaderElement = GD.Load<ShaderMaterial>("res://Materials/DestroyMaterial.tres");

			foreach (Node2D StructureElement in Structures.GetChildren())
			{
				StructureElement.GetNode<Sprite2D>("StructureSprite").Material = ShaderElement;
			}
		} else
		{
			DestroyMode = false;

			foreach (Node2D StructureElement in Structures.GetChildren())
			{
				StructureElement.GetNode<Sprite2D>("StructureSprite").Material = null;
			}
		}
	}

	public void CreateStructure(string StructureName)
	{
		if (StructureName == "Destroy") TriggerDestroyMode(true);
		else if (PlacingStructure == null)
		{
			PackedScene StructureScene = GD.Load<PackedScene>("res://Actors/Structures/" + StructureName + ".tscn");

			Structure StructureElement = StructureScene.Instantiate<Structure>();

			GetTree().CurrentScene.GetNode<Node2D>("Structures").AddChild(StructureElement);

			StructureElement._StartPlaceAction();

			StructureElement.GlobalPosition = GetViewport().GetCamera2D().GetGlobalMousePosition();

			PlacingStructure = StructureElement;

			if (DestroyMode) TriggerDestroyMode(false);
		}
		else if (PlacingStructure.Mandatory == false)
		{
			PlacingStructure.QueueFree();
			PlacingStructure = null;

			CreateStructure(StructureName);
		}
	}
	public void PlaceStructure()
	{
		if (Money < PlacingStructure.Price)
		{
			return;
		}

		//

		PhysicsDirectSpaceState2D Space = GetWorld2D().DirectSpaceState;

		PhysicsShapeQueryParameters2D Query = new PhysicsShapeQueryParameters2D
		{
			Shape = PlacingStructure.GetNode<CollisionShape2D>("PlaceArea/CollisionShape2D").Shape,
			Transform = PlacingStructure.GetNode<Area2D>("PlaceArea").GlobalTransform,

			CollideWithAreas = true,
			CollideWithBodies = true
		};

		Godot.Collections.Array<Godot.Collections.Dictionary> Result = Space.IntersectShape(Query);

		bool CanPlace = true;

		foreach (Godot.Collections.Dictionary CurrentResult in Result)
		{
			Node2D CurrentNode = CurrentResult["collider"].As<Node2D>();

			if (CurrentNode.Name == "TileMap" || CurrentNode.Name == "PlaceArea" && CurrentNode.GetParent() != PlacingStructure) CanPlace = false;
		}

		if (CanPlace)
		{
			UpdateMoney(PlacingStructure.Price * -1);

			PlacingStructure.Place();
			PlacingStructure = null;
		}
	}

	// Storage

	public void AddInStorage(string MaterialName, int Quantity)
	{
		if (Storage.ContainsKey(MaterialName)) Storage[MaterialName] += Quantity;
		else Storage.Add(MaterialName, Quantity);

		GetNode<Gui>("Gui").GetNode<RightBar>("RightBar").UpdateStorageLabel(MaterialName, Storage[MaterialName]);
	}

	//

	public override void _Ready()
	{
		base._Ready();

		CreateMineralNoise();

		Structures = GetNode<Node2D>("Structures");

		GameCamera = GetNode<Camera>("Camera");
		GameGui = GetNode<Gui>("Gui");

		//

		CreateStructure("LunarShip");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		var CurrentTick = Time.GetTicksMsec();

		foreach (Structure StructureElement in Structures.GetChildren())
		{
			if (StructureElement.Placed && CurrentTick - StructureElement.CurrentLastTick > StructureElement.ActionDelay * 1000 || StructureElement.Placed && !StructureElement.Working)
			{
				StructureElement._RunAction();
				StructureElement.CurrentLastTick = CurrentTick;
			} else if (!StructureElement.Placed)
			{
				StructureElement._PlacingAction();
			}
		}

		if (PlacingStructure != null)
		{
			Vector2 StructurePosition = GetViewport().GetCamera2D().GetGlobalMousePosition();

			PlacingStructure.GlobalPosition = StructurePosition.Round();
			PlacingStructure.GlobalPosition = StructurePosition.Round();
		}

	}
}
