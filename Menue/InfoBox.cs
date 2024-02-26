using Godot;
using System;
using System.Collections.Generic;
using Flattiverse.Connector.MissionSelection;

public partial class InfoBox : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private string Display = "";

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.Universe != null && Display == "")
		{
			foreach (KeyValuePair<string, GalaxyInfo> gInfo in GameManager.Universe.Galaxies)
			{
				Display += string.Format($" -> {gInfo.Key} {gInfo.Value.GameMode}\n");

				foreach (KeyValuePair<string, TeamInfo> tInfo in gInfo.Value.Teams)
					Display += string.Format($"   -> {tInfo.Key} {tInfo.Value.Id}\n");

				foreach (KeyValuePair<string, PlayerInfo> pInfo in gInfo.Value.Players)
					Display += string.Format($"   => {pInfo.Key} {pInfo.Value.Id} {pInfo.Value.Team.Name}\n");
			}

			this.Text = Display;

		}
	}
}
