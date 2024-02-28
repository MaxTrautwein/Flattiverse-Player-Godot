using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Flattiverse;
using Flattiverse.Connector.MissionSelection;
using Flattiverse.Game;

public partial class MainMenue : Control
{
	private Button _settingsButton;
	private Button _connectButton;
	private RichTextLabel _infoBox;
	private ItemList _galaxiesListBox;
	private ItemList _teamsListBox;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Thread thread1 = new Thread(GameManager.Init);
		thread1.Start();

		_settingsButton = GetNode<Button>("SettingsButton");
		_settingsButton.Connect("pressed", Callable.From(() => GetTree().ChangeSceneToFile("res://settings.tscn")));

		_connectButton = GetNode<Button>("Connect");
		_connectButton.Connect("pressed", Callable.From(Connect));

		_infoBox = GetNode<RichTextLabel>("InfoBox");
		_galaxiesListBox = GetNode<ItemList>("GalaxiesListBox");
		_teamsListBox = GetNode<ItemList>("TeamsListBox");
		
		GameManager.ApiKey = Settings.ReadApiKey();

	}

	private void Connect()
	{
		Thread thread1 = new Thread(GameManager.Connect);
		thread1.Start();
		thread1.Join();
		GetTree().ChangeSceneToFile("res://Game/game.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RenderInfoBox();

		ListBoxManager();
	}


	private Dictionary<string, List<string>> _galaxyTeams = new Dictionary<string, List<string>>();
	
	private bool _initDone = false;
	private int _lastGalaxyIndex = -1;
	private int _lastTeamIndex = -1;
	private void ListBoxManager()
	{
		if (GameManager.Universe != null && !_initDone)
		{
			_initDone = true;
			foreach (KeyValuePair<string, GalaxyInfo> gInfo in GameManager.Universe.Galaxies)
			{
				_galaxiesListBox.AddItem(string.Format($"{gInfo.Key}"));
				_galaxyTeams.Add(gInfo.Key,new List<string>());
				foreach (KeyValuePair<string, TeamInfo> tInfo in gInfo.Value.Teams)
					_galaxyTeams[gInfo.Key].Add(tInfo.Key);
			}

			_galaxiesListBox.Select(0);
		}else if (_initDone && _galaxiesListBox.GetSelectedItems().Length > 0)
		{
			var indx = _galaxiesListBox.GetSelectedItems()[0];

			if (_lastGalaxyIndex != indx)
			{
				GameManager.GalaxyName = _galaxiesListBox.GetItemText(indx);
				GD.Print($"Selected: {GameManager.GalaxyName}");
				_lastGalaxyIndex = indx;
				
				_teamsListBox.Clear();
				foreach (var galaxy in _galaxyTeams[GameManager.GalaxyName])
				{
					_teamsListBox.AddItem(galaxy);
				}
				_teamsListBox.Select(0);
				_lastTeamIndex = -1;
			}
		}

		if (_initDone && _teamsListBox.GetSelectedItems().Length > 0 &&
			_lastTeamIndex != _teamsListBox.GetSelectedItems()[0])
		{
			_lastTeamIndex = _teamsListBox.GetSelectedItems()[0];
			GameManager.TeamName = _teamsListBox.GetItemText(_lastTeamIndex);
		}
	}

	private void RenderInfoBox()
	{
		if (GameManager.Universe != null && _infoBox.Text == "")
		{
			foreach (KeyValuePair<string, GalaxyInfo> gInfo in GameManager.Universe.Galaxies)
			{
				_infoBox.Text += string.Format($" -> {gInfo.Key} {gInfo.Value.GameMode}\n");

				foreach (KeyValuePair<string, TeamInfo> tInfo in gInfo.Value.Teams)
					_infoBox.Text += string.Format($"   -> {tInfo.Key} {tInfo.Value.Id}\n");

				foreach (KeyValuePair<string, PlayerInfo> pInfo in gInfo.Value.Players)
					_infoBox.Text += string.Format($"   => {pInfo.Key} {pInfo.Value.Id} {pInfo.Value.Team.Name}\n");
			}
		}
	}
}
