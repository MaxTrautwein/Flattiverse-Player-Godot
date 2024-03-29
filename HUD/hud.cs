using Godot;
using System;
using System.Collections.Generic;
using Flattiverse.Game;
using Flattiverse.Utils;

public partial class hud : CanvasLayer
{
	private static hud _instance;
	public static hud Instance => _instance;

	public static bool ShouldShowRespawn = false;
	
	private LineEdit _chatMsgLine;
	private Label _fpsDisplay;
	private Label _statusLine;
	private Label _positionInfo;
	private RichTextLabel _chatBox;
	private VSlider _targetThrustForward;
	private static CanvasLayer _respawnScreen;

	private bool _usePidSettings = false;
	private SpinBox _kp;
	private SpinBox _ki;
	private SpinBox _kd;
	private SpinBox _bias;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_instance = this;
		_chatMsgLine = GetNode<LineEdit>("ChatLine");
		_fpsDisplay = GetNode<Label>("FPS_Cnt");
		_statusLine = GetNode<Label>("StatusBar");
		_positionInfo = GetNode<Label>("PositionInfo");
		_chatBox = GetNode<RichTextLabel>("ChatBox");
		_targetThrustForward = GetNode<VSlider>("ThrustSlider");
		_respawnScreen = GetNode<CanvasLayer>("RespawnOverlay");

		_kp = GetNode<SpinBox>("SpinBoxKP");
		_ki = GetNode<SpinBox>("SpinBoxKI");
		_kd = GetNode<SpinBox>("SpinBoxKD");
		_bias = GetNode<SpinBox>("SpinBoxBias");
		if (!_usePidSettings)
		{
			_kp.Hide();
			_ki.Hide();
			_kd.Hide();
			_bias.Hide();
		}
	}
	private string FormatPercentage(string name, double value, double valueMax)
	{
		return string.Format($"{name}: {value / valueMax:P2} ");
	}

	public static void ShowRespawn()
	{
		_respawnScreen.Visible = ShouldShowRespawn;
	}

	public static List<String> NewMsgs = new List<string>();
	public static void RegisterChatMsg(string msg)
	{
		//GD.Print($"CHAT: {msg}");
		NewMsgs.Add(msg);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		ShowRespawn();
		_fpsDisplay.Text = string.Format($"{Engine.GetFramesPerSecond()}/{Engine.MaxFps} FPS");
		if (GameManager.PlayerShip != null)
		{
			var ship = GameManager.PlayerShip;
			var hull = FormatPercentage("Hull", ship.Hull, ship.HullMax);
			var energy = FormatPercentage("Energy", ship.Energy, ship.EnergyMax);
			var shields = FormatPercentage("Shields", ship.Shields, ship.ShieldsMax);
			
			_statusLine.Text = string.Format($"{hull}- {energy}- {shields}- dir={ship.Direction:F}, Thrust={ship.Thruster: 0.000;-0.000}, Nozzel={ship.Nozzle:F}, TURNRATE={ship.Turnrate:F}, SPEED={ship.Movement.Length:0.0000}"); 
			
			
			_positionInfo.Text = string.Format($"Pos: {ship.Position.ToString()} ---{DisplayHelper.TransformToGamePos( DisplayHelper.MouseDisplayPos(this))} ");
			
			
			
			
			
			
			HandleSpeedSetting();

			_targetThrustForward.Step = game.GetInstance.ShipController.ThrusterForwardMax / 100;
			_targetThrustForward.MaxValue = game.GetInstance.ShipController.ThrusterForwardMax;
			
			_targetThrustForward.Value = game.GetInstance.ShipController.DesierdThrustForward;

			
			
		}


		if (Input.IsActionPressed("SendChatMsg"))
		{
			GameManager.Galaxy.Chat(_chatMsgLine.Text);
			_chatMsgLine.Text = "";
			_chatMsgLine.ReleaseFocus();
		}

		foreach (var msg in NewMsgs)
		{
			_chatBox.Text += msg + "\n";
		}
		NewMsgs.Clear();
		
	}

	private static void HandleSpeedSetting()
	{
		var speedIncrement = game.GetInstance.ShipController.ThrusterForwardMax / 10;
		if (Input.IsActionJustPressed("IncreaseSpeed"))
		{
			game.GetInstance.ShipController.DesierdThrustForward += speedIncrement;
		}
		if (Input.IsActionJustPressed("DecreaseSpeed"))
		{
			game.GetInstance.ShipController.DesierdThrustForward -= speedIncrement;
		}
	}
}
