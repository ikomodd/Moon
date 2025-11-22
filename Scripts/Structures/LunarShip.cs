using Godot;
using System;

public partial class LunarShip : Structure
{
	private StructurePanel ShipPanel = null;

	//

	public override void _Clicked()
	{
		base._Clicked();

		if (Placed) ShipPanel._Open();
	}

	public override void _EndPlaceAction()
	{
		base._EndPlaceAction();

		//GameService.GameCamera.Enabled = false;

		//Tween StartCameraPositionTween = CreateTween();
		//StartCameraPositionTween.TweenProperty(GameService.GameCamera, "position", GlobalPosition, 5.0f).SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.InOut);

		//Tween StartCameraZoomTween = CreateTween();
		//StartCameraZoomTween.TweenProperty(GameService.GameCamera.GameCamera, "zoom", new Vector2(5.0f, 5.0f), 5.0f).SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.InOut);

		//StartCameraPositionTween.Play();
		//StartCameraZoomTween.Play();

		//StartCameraPositionTween.Finished += () =>
		//{
			// Animaçao do inicio do jogo aqui <<

			//Tween EndCameraZoomTween = CreateTween();
			//EndCameraZoomTween.TweenProperty(GameService.GameCamera.GameCamera, "zoom", new Vector2(1.0f, 1.0f), 5.0f).SetTrans(Tween.TransitionType.Quint).SetEase(Tween.EaseType.InOut);

			//EndCameraZoomTween.Play();

			//EndCameraZoomTween.Finished += () =>
			//{
				//GameService.GameCamera.Enabled = true;
			//};
		//};
	}

	//

	public override void _Ready()
	{
		base._Ready();

		ShipPanel = GameService.GameGui.GetStructurePanel("ShipPanel");

		Mandatory = true;
	}
}
