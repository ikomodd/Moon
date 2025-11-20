using Godot;
using System;
public partial class Gui : CanvasLayer
{
	Game GameService = null;

	public UpperBar UpperBarUi = null;
	public RightBar RightBarUi = null;

	private Node StructurePanels = null;

	public bool InFullscreen = false;

	Vector2 DragxClickPosition = Vector2.Zero;

	//

	public StructurePanel GetStructurePanel(string Name)
	{
		return StructurePanels.GetNode<StructurePanel>(Name);
	}

	//

	public override void _Ready()
	{
		base._Ready();

		Visible = true;

		GameService = GetTree().CurrentScene as Game;

		UpperBarUi = GetNode<UpperBar>("UpperBar");
		RightBarUi = GetNode<RightBar>("RightBar");

		StructurePanels = GetNode<Node>("StructureGUIs");

		UpperBarUi.UpdateMoneyLabel(GameService.Money);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (@event is InputEventKey KeyEvent)
		{
			if (KeyEvent.Keycode == Godot.Key.F11 && KeyEvent.Pressed)
			{
				if (InFullscreen)
				{
					DisplayServer.WindowSetMode(DisplayServer.WindowMode.Windowed, 0);
					InFullscreen = false;
				}
				else
				{
					DisplayServer.WindowSetMode(DisplayServer.WindowMode.Fullscreen, 0);
					InFullscreen = true;
				}
			}
		}
		
		if (@event is InputEventMouseButton MouseButtonEvent)
		{
			if (MouseButtonEvent.ButtonIndex == Godot.MouseButton.Left && MouseButtonEvent.Pressed) {

				if (GameService.PlacingStructure == null)
				{
					PhysicsDirectSpaceState2D Space = GameService.GetWorld2D().DirectSpaceState;

					PhysicsPointQueryParameters2D Query = new PhysicsPointQueryParameters2D
					{
						Position = GetViewport().GetCamera2D().GetGlobalMousePosition(),

						CollideWithAreas = true,
						CollideWithBodies = false
					};

					Godot.Collections.Array<Godot.Collections.Dictionary> Result = Space.IntersectPoint(Query);

					foreach (Godot.Collections.Dictionary CurrentResult in Result)
					{
						Node2D CurrentNode = CurrentResult["collider"].As<Node2D>();

						if (CurrentNode.GetParent() is Structure StructureNode)
						{
							if (CurrentNode.Name == "ClickArea")
							{
								StructureNode._Clicked();
								break;
							}
						}
					}
				}
				else GameService.PlaceStructure();
			} 

			if (MouseButtonEvent.ButtonIndex == Godot.MouseButton.Right && MouseButtonEvent.Pressed)
			{
				DragxClickPosition = MouseButtonEvent.Position;
			}
			else if (MouseButtonEvent.ButtonIndex == Godot.MouseButton.Right && !MouseButtonEvent.Pressed)
			{
				if (DragxClickPosition == MouseButtonEvent.Position && GameService.DestroyMode) GameService.TriggerDestroyMode(false);

				if (DragxClickPosition == MouseButtonEvent.Position && GameService.PlacingStructure != null && GameService.PlacingStructure.Mandatory == false)
				{
					GameService.PlacingStructure.QueueFree();
					GameService.PlacingStructure = null;
				}
			}
		}
	}
}
