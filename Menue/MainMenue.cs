using Godot;
using System;
using System.Threading;
using Flattiverse;

public partial class MainMenue : Control
{
	private Button _settingsButton;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Thread thread1 = new Thread(GameManager.init);
		thread1.Start();

		_settingsButton = GetNode<Button>("SettingsButton");
		_settingsButton.Connect("pressed", Callable.From(() => GetTree().ChangeSceneToFile("res://settings.tscn")));

		GameManager.ApiKey = Settings.ReadAPIKey();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		
	}
}
