using Godot;
using System;
using System.Text.RegularExpressions;

public partial class RightBar : Control
{
	Game GameService = null;

	Panel ParentPanel = null;

	Panel BuildPanel = null;
	Panel StoragePanel = null;

	Button OpenMenuButton = null;

	ButtonGroup MenuButtons = null;
	ButtonGroup BuildButtons = null;

	bool MenuOpened = false;

	//

	void TriggerMenu(BaseButton CurrentButton)
	{
		if (CurrentButton.Name == "BuildButton")
		{
			BuildPanel.Visible = true;
			StoragePanel.Visible = false;
		}
		else
		{
			BuildPanel.Visible = false;
			StoragePanel.Visible = true;
		}
	}

	void ChangeStructure(BaseButton CurrentButton)
	{
		GameService.CreateStructure(CurrentButton.GetMeta("Structure").ToString());
		CurrentButton.ButtonPressed = false;
	}

	void OpenMenu()
	{
		Tween MenuTween = CreateTween();
		
		if (!MenuOpened)
		{
			MenuOpened = true;
			MenuTween.TweenProperty(ParentPanel, "position", new Vector2(-320, 64), 0.5).SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.Out);
		}
		else
		{
			MenuOpened = false;
			MenuTween.TweenProperty(ParentPanel, "position", new Vector2(0, 64), 0.25).SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.InOut);
		}

		MenuTween.Play();
	}

	public void UpdateStorageLabel(string MaterialName, long Quantity)
	{
		if (StoragePanel.GetNode<VBoxContainer>("VBoxContainer").HasNode(MaterialName + "Label"))
		{
			ValueLabel MaterialLabel = StoragePanel.GetNode<VBoxContainer>("VBoxContainer").GetNode<ValueLabel>(MaterialName + "Label");

			MaterialLabel.Value = Quantity;
			MaterialLabel.LabelUpdate();
		}
		else
		{
			PackedScene MaterialLabelTscn = GD.Load<PackedScene>("res://Actors/Gui/material_label.tscn");

			ValueLabel MaterialLabel = MaterialLabelTscn.Instantiate<ValueLabel>();
			StoragePanel.GetNode<VBoxContainer>("VBoxContainer").AddChild(MaterialLabel);

			MaterialLabel.Name = MaterialName + "Label";
			MaterialLabel.Visible = true;

			MaterialLabel.ValueName = MaterialName;
			MaterialLabel.Value = Quantity;
			MaterialLabel.LabelUpdate();
		}
	}

	//

	public override void _Ready()
	{
		base._Ready();
		
		GameService = GetTree().CurrentScene as Game;

		ParentPanel = GetNode<Panel>("Panel");

		BuildPanel = ParentPanel.GetNode<Panel>("BuildPanel");
		StoragePanel = ParentPanel.GetNode<Panel>("StoragePanel");

		OpenMenuButton = ParentPanel.GetNode<Button>("OpenButton");
		OpenMenuButton.Pressed += OpenMenu;

		MenuButtons = ParentPanel.GetNode<Button>("BuildButton").ButtonGroup;
		MenuButtons.Pressed += TriggerMenu;

		BuildButtons = BuildPanel.GetNode<ScrollContainer>("ScrollContainer").GetNode<VBoxContainer>("VBoxContainer").GetNode<GridContainer>("GridContainer").GetNode<Button>("StructureButton").ButtonGroup;
		BuildButtons.Pressed += ChangeStructure;
	}
}
