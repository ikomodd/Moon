using Godot;
using System;

public partial class StructurePanel : Control
{
	Button ExitButton = null;

	//

	public override void _Ready()
	{
		base._Ready();

		ExitButton = GetNode<Button>("Panel/ExitButton");

		ExitButton.Pressed += _Exit;
	}

	//

	public virtual void _Open()
	{
		Visible = true;
	}

	public virtual void _Exit()
	{
		Visible = false;
	}
}
