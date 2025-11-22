using Godot;
using System;

public partial class MaterialPanel : Control
{
	public Game GameService = null;

	public string MaterialReference = "";

	Slider QuantitySlider = null;

	RichTextLabel MaxTextReference = null;
	RichTextLabel QuantityTextReference = null;
	ValueLabel MoneyTextReference = null;

	public long Money = 0;
	public long Quantity = 0;

	private Tween AnimationTween = null;

	//

	private void Open()
	{
		if (AnimationTween != null) AnimationTween.Kill();

		AnimationTween = CreateTween();
		AnimationTween.TweenProperty(this, "custom_minimum_size", new Vector2(296.0f, 64.0f), 0.1);

		AnimationTween.Play();
	}

	private void Close()
	{
		if (AnimationTween != null) AnimationTween.Kill();

		AnimationTween = CreateTween();
		AnimationTween.TweenProperty(this, "custom_minimum_size", new Vector2(296.0f, 32.0f), 0.1);

		AnimationTween.Play();
	}

	void ValueChanged(double Value)
	{
		Quantity = (long)((Value / 100) * GameService.Storage[MaterialReference]);

		QuantityTextReference.Text = Quantity.ToString();

		MaxTextReference.Text = GameService.Storage[MaterialReference].ToString();

		Money = (long)(GameService.MineRarity[MaterialReference] * 1000 * Quantity);

		MoneyTextReference.Value = Money;
		MoneyTextReference.LabelUpdate();
	}

	public long Sell()
	{
		long MoneyResult = Money;

		GameService.AddInStorage(MaterialReference, Quantity * -1);

		QuantitySlider.Value = 0;

		ValueChanged(QuantitySlider.Value);

		MoneyTextReference.Value = Money;
		MoneyTextReference.LabelUpdate();

		return MoneyResult;
	}

	public override void _Ready()
	{
		base._Ready();

		GetNode<RichTextLabel>("Panel/Name").Text = MaterialReference;

		GameService = GetTree().CurrentScene as Game;

		QuantitySlider = GetNode<Slider>("QuantitySlider");

		MaxTextReference = GetNode<RichTextLabel>("Max");
		QuantityTextReference = GetNode<RichTextLabel>("Quantity");
		MoneyTextReference = GetNode<ValueLabel>("Panel/Money");

		MouseEntered += Open;
		MouseExited += Close;

		QuantitySlider.ValueChanged += ValueChanged;
	}
}
