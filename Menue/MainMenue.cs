using Godot;
using System;
using System.Collections.Generic;
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
