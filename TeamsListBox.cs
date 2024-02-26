using Godot;
using System;
using System.Collections.Generic;
using Flattiverse.Connector.MissionSelection;

public partial class TeamsListBox : ItemList
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private string lastUniverse = "";

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.Universe != null && lastUniverse != GameManager.Universe.BaseURI)
		{
			/*lastUniverse = GameManager.Universe.BaseURI;
			foreach (KeyValuePair<string, TeamInfo> tInfo in GameManager.Galaxy.Teams)
				Console.WriteLine($"   -> {tInfo.Key} {tInfo.Value.Id}");
				*/
		}

	}
}
