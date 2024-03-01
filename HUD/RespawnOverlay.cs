using Godot;
using System;
using Flattiverse.Game;

public partial class RespawnOverlay : CanvasLayer
{
	
	private Button _respawnButton;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_respawnButton = GetNode<Button>("RespawnButton");
		_respawnButton.Connect("pressed", Callable.From(HandleRespawn));
	}

	private void HandleRespawn()
	{
		GameManager.Respawn();
		hud.ShouldShowRespawn = false;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
