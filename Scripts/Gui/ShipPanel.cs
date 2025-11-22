using Godot;
using System;

public partial class ShipPanel : StructurePanel
{
	Button SellButton = null;

	Control MaterialsParent = null;

	//

	void MaterialAdded(string Name, long Quantity, bool First)
	{
		GD.Print("Okay");

		if (First)
		{
			
			PackedScene MaterialPanelScene = GD.Load<PackedScene>("res://Actors/Gui/material_panel.tscn");
			MaterialPanel MaterialPanelInstance = MaterialPanelScene.Instantiate<MaterialPanel>();

			MaterialPanelInstance.MaterialReference = Name;

			MaterialsParent.AddChild(MaterialPanelInstance);

			MaterialPanelInstance.Name = Name;
		}
	}

	void Sell()
	{
		long MoneyResult = 0;

		foreach(MaterialPanel Material in MaterialsParent.GetChildren())
		{
			MoneyResult += Material.Sell();
		}

		GameService.UpdateMoney(MoneyResult);
	}

	//

	public override void _Ready()
	{
		base._Ready();

		MaterialsParent = GetNode<Control>("Panel/ShopPanel/ScrollContainer/VBoxContainer");
		SellButton = GetNode<Button>("Panel/ShipPanel/SellButton");

		GameService.MaterialAdded += MaterialAdded;
		SellButton.Pressed += Sell;
	}
}
