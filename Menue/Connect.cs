using Godot;
using System;
using System.Threading;
using Flattiverse.Connector.Hierarchy;

public partial class Connect : Button
{
	public override async void _Pressed()
	{
		base._Pressed();
		Thread thread1 = new Thread(GameManager.Connect);
		thread1.Start();
		thread1.Join();
		GetTree().ChangeSceneToFile("res://Game/game.tscn");
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
