using Godot;

namespace Flattiverse;

public partial class Settings : Control
{
	private LineEdit _apiKeyLine;
	private Button _saveButton;

	private const string SaveFilePath = "user://settings.data";

	public static string ReadAPIKey()
	{
		var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Read);
		var key = file.GetLine();
		file.Close();
		return key;
	}
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_apiKeyLine = GetNode<LineEdit>("APIKey_Line");
		_saveButton = GetNode<Button>("SaveButton");
		_saveButton.Connect("pressed", Callable.From(() => Save()));
		
		_apiKeyLine.Text = ReadAPIKey();
	}

	public void Save()
	{
		var file = FileAccess.Open(SaveFilePath, FileAccess.ModeFlags.Write);
		file.StoreString(_apiKeyLine.Text);
		file.Close();
		GetTree().ChangeSceneToFile("res://Menue/MainMenue.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
