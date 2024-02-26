using Godot;
using System;
using System.Threading;

public partial class MainMenue : Control
{
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Thread thread1 = new Thread(GameManager.init);
		thread1.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		
	}
}
