using System.Collections.Generic;
using Flattiverse.Connector.MissionSelection;
using Flattiverse.Game;
using Godot;

namespace Flattiverse.Menue;

public partial class GalaxiesListBox : ItemList
{
	
	private bool _initDone = false;

	private int _lastIndex = -1;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (GameManager.Universe != null && !_initDone)
		{
			_initDone = true;
			foreach (KeyValuePair<string, GalaxyInfo> gInfo in GameManager.Universe.Galaxies)
			{
				AddItem(string.Format($"{gInfo.Key}"));
			}
		}else if (_initDone && this.GetSelectedItems().Length > 0)
		{
			var indx = this.GetSelectedItems()[0];

			if (_lastIndex != indx)
			{
				GameManager.GalaxyName = GetItemText(indx);
				GD.Print($"Selected: {GameManager.GalaxyName}");
				_lastIndex = indx;
			}
		}
	}
}