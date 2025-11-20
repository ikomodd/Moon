using Godot;
using System;

public partial class UpperBar : Node
{
	ValueLabel MoneyLabel = null;

	public void UpdateMoneyLabel(long Quantity)
	{
		MoneyLabel.Value = Quantity;
		MoneyLabel.LabelUpdate();
	}

	//

	public override void _Ready()
	{
		base._Ready();

		MoneyLabel = GetNode<ValueLabel>("Panel/HBoxContainer/EconomicPanel/MoneyCount");
	}
}
