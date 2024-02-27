using Godot;
using System;

public partial class StatusBar : Label
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.PlayerShip != null)
		{
			var ship = GameManager.PlayerShip;
			Text = string.Format($"Hull: {ship.Hull / ship.HullMax:P2} - Energy: {ship.Energy / ship.EnergyMax:P2} - Shields: {ship.Shields / ship.ShieldsMax:P2} - DIRECTION={ship.Direction:F}, THRUSTER={ship.Thruster:0.0000}, NOZZLE={ship.Nozzle:F}, TURNRATE={ship.Turnrate:F}, SPEED={ship.Movement.Length}"); 
			
		}
	}
}
