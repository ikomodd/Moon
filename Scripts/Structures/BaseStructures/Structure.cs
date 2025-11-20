
using Godot;
public partial class Structure : Node2D
{
	public Game GameService = null;

	public long Price = 0;

	public bool Working = false;
	public bool Placed = false;

	public float ActionDelay = 1;
	public ulong CurrentLastTick = 0;

	public bool Mandatory = false;

	//

	public void Place()
	{
		Tween AlignTween = CreateTween();
		AlignTween.TweenProperty(GetNode<Sprite2D>("StructureSprite"), "position", new Vector2(0, 0), 0.5).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.In);
		AlignTween.Play();

		AlignTween.Finished += () =>
		{
			Placed = true;
			_EndPlaceAction();

			GetNode<Area2D>("PlaceArea").Visible = true;

			GameService.GameCamera.Shake(0.2f, 0.2f);

			GetNode<Sprite2D>("ShadowSprite").QueueFree();
		};
	}

	public bool AddCaution(string CautionName)
	{
		Node2D Cautions = GetNode<Node2D>("Cautions");

		if (Cautions.HasNode(CautionName))
		{
			return true;
		}
		else
		{
			Sprite2D CautionInstance = new Sprite2D
			{
				Texture = GD.Load<Texture2D>("res://Sprites/" + CautionName + ".png"),
			};

			CautionInstance.Name = CautionName;

			Cautions.AddChild(CautionInstance);

			return false;
		}
	}
	public bool RemoveCaution(string CautionName)
	{
		Node2D Cautions = GetNode<Node2D>("Cautions");

		if (Cautions.HasNode(CautionName))
		{
			Cautions.GetNode<Sprite2D>(CautionName).QueueFree();
			return true;
		}
		else return false;
	}

	//

	public override void _Ready()
	{
		base._Ready();

		GameService = GetTree().CurrentScene as Game;
	}

	//
	public virtual void _Clicked() 
	{
		if (Placed)
		{
			if (GameService.DestroyMode && !Mandatory && GameService.Money > Price / 2)
			{
				GameService.UpdateMoney((Price / 2) * -1);
				QueueFree();
			}
		}
	}
	public virtual void _StartPlaceAction() { }
	public virtual void _EndPlaceAction() { }
	public virtual void _PlacingAction() { }
	public virtual void _RunAction() { }
}
