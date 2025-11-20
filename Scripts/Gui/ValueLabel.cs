using Godot;
using System;

public partial class ValueLabel : RichTextLabel
{
	[Export]
	public string ValueName = "X";

	[Export]
	public long Value = 0;

	private bool State = false;

	public void LabelUpdate()
	{
		string StringValue = "";

		if (!State)
		{
			if (Value >= 1000000000) StringValue = (Value / 1000000000).ToString() + "B";
			else if (Value >= 1000000) StringValue = (Value / 1000000).ToString() + "M";
			else if (Value >= 1000) StringValue = (Value / 1000).ToString() + "K";
			else StringValue = Value.ToString();

		}
		else
		{
			StringValue = Value.ToString();
		}

		Text = ValueName + ": " + StringValue;
	}

	public override void _Ready()
	{
		base._Ready();

		Visible = true;

		//

		MouseEntered += () =>
		{
			State = true;
			LabelUpdate();
		};
		MouseExited += () =>
		{
			State = false;
			LabelUpdate();
		};
	}
}
