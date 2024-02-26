using Godot;
using System;
using System.Collections.Generic;
using Flattiverse.Connector.MissionSelection;

public partial class GalaxiesListBox : ItemList
{
	
	private bool initDone = false;

	private int lastIndex = -1;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.Universe != null && !initDone)
		{
			initDone = true;
			foreach (KeyValuePair<string, GalaxyInfo> gInfo in GameManager.Universe.Galaxies)
			{
				AddItem(string.Format($"{gInfo.Key}"));
			}
		}else if (initDone && this.GetSelectedItems().Length > 0)
		{
			var indx = this.GetSelectedItems()[0];

			if (lastIndex != indx)
			{
				GameManager.GalaxyName = GetItemText(indx);
				GD.Print($"Selected: {GameManager.GalaxyName}");
				lastIndex = indx;
			}
		}
	}
}
