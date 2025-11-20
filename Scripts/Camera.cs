using Godot;
using System;

class Shake
{
	public ulong ShakeStartTick = 0;

	public Vector2 ShakePosition = Vector2.Zero;

	public float ShakeTime = 0;
	public float ShakeSize = 0;

	public Shake(float shakeTime, float shakeSize)
	{
		ShakeStartTick = Time.GetTicksMsec();

		ShakeTime = shakeTime;
		ShakeSize = shakeSize;
	}
}

public partial class Camera : Node2D
{
	public Camera2D GameCamera = null;

	private bool CanMove = false;
	private float ZoomStep = 1.1f;

	public bool Enabled = true;

	private Shake ShakeEvent = null;

	//

	bool HasGuiInMousePoint()
	{
		Control Hovered = GetViewport().GuiGetHoveredControl();

		if (Hovered != null && Hovered.GetParent() is Control) return true;

		return false;
	}

	public void Shake(float ShakeTime, float ShakeSize)
	{
		ShakeEvent = new Shake(ShakeTime, ShakeSize);
	}

	//

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		bool MouseInGui = HasGuiInMousePoint();

		if (@event is InputEventMouseButton MouseButtonEvent && Enabled)
		{
			if (MouseButtonEvent.Pressed && MouseButtonEvent.ButtonIndex == Godot.MouseButton.Right && !MouseInGui) CanMove = true;
			else if (!MouseButtonEvent.Pressed && MouseButtonEvent.ButtonIndex == Godot.MouseButton.Right) CanMove = false;

			if (!MouseInGui)
			{
				if (MouseButtonEvent.ButtonIndex == Godot.MouseButton.WheelUp) GameCamera.Zoom *= ZoomStep;
				else if (MouseButtonEvent.ButtonIndex == Godot.MouseButton.WheelDown) GameCamera.Zoom /= ZoomStep;
			}

			GameCamera.Zoom = GameCamera.Zoom.Clamp(0.7f, 10.0f);
		}

		if (@event is InputEventMouseMotion MouseMotionEvent && Enabled)
		{
			if (CanMove) Position -= MouseMotionEvent.Relative / GameCamera.Zoom.X;
		}
	}

	public override void _Ready()
	{
		base._Ready();

		GD.Randomize();

		GameCamera = GetNode<Camera2D>("Camera2D");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);

		if (ShakeEvent != null )
		{
			ulong CurrentTick = Time.GetTicksMsec();
			float TimeCounter = CurrentTick - ShakeEvent.ShakeStartTick;

			if (TimeCounter < ShakeEvent.ShakeTime * 1000)
			{
				float ShakeStepX = ShakeEvent.ShakeSize * 100;
				float ShakeStepY = ShakeEvent.ShakeSize * 100;

				ShakeEvent.ShakePosition = new Vector2
				(
					(float)GD.RandRange(-ShakeStepX, ShakeStepX),
					(float)GD.RandRange(-ShakeStepY, ShakeStepY)
				);
			}

			GameCamera.Position = Vector2.Zero.Lerp(ShakeEvent.ShakePosition, (ShakeEvent.ShakeSize) / 1.5f);

			if (TimeCounter > ShakeEvent.ShakeTime * 1000)
			{
				ShakeEvent.ShakePosition = new Vector2(0, 0);
				if (GameCamera.Position == new Vector2(0, 0)) ShakeEvent = null;
			}
		}
	}
}
