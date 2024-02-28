using Godot;
using System;
using System.Collections.Generic;
using Flattiverse.Game;

public partial class hud : CanvasLayer
{

	private LineEdit _chatMsgLine;
	private Label _fpsDisplay;
	private Label _statusLine;
	private RichTextLabel _chatBox;

	private SpinBox _kp;
	private SpinBox _ki;
	private SpinBox _kd;
	private SpinBox _bias;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_chatMsgLine = GetNode<LineEdit>("ChatLine");
		_fpsDisplay = GetNode<Label>("FPS_Cnt");
		_statusLine = GetNode<Label>("StatusBar");
		_chatBox = GetNode<RichTextLabel>("ChatBox");

		_kp = GetNode<SpinBox>("SpinBoxKP");
		_ki = GetNode<SpinBox>("SpinBoxKI");
		_kd = GetNode<SpinBox>("SpinBoxKD");
		_bias = GetNode<SpinBox>("SpinBoxBias");
	}

	private string FormatPercentage(string name, double value, double valueMax)
	{
		return string.Format($"{name}: {value / valueMax:P2}% ");
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
		_fpsDisplay.Text = string.Format($"{Engine.GetFramesPerSecond()}/{Engine.MaxFps} FPS");
		if (GameManager.PlayerShip != null)
		{
			var ship = GameManager.PlayerShip;
			var hull = FormatPercentage("Hull", ship.Hull, ship.HullMax);
			var energy = FormatPercentage("Energy", ship.Energy, ship.EnergyMax);
			var shields = FormatPercentage("Shields", ship.Shields, ship.ShieldsMax);
			
			_statusLine.Text = string.Format($"{hull}- {energy}- {shields}- dir={ship.Direction:F}, Thrust={ship.Thruster:0.0000}, Nozzel={ship.Nozzle:F}, TURNRATE={ship.Turnrate:F}, SPEED={ship.Movement.Length}"); 
		}


		if (Input.IsActionPressed("SendChatMsg"))
		{
			GameManager.Galaxy.Chat(_chatMsgLine.Text);
			_chatMsgLine.Text = "";
		}

		foreach (var msg in NewMsgs)
		{
			_chatBox.Text += msg + "\n";
		}
		NewMsgs.Clear();

		var nozzelControl = game.GetInstance.NozzelControl;
		nozzelControl.Ki = _ki.Value;
		nozzelControl.Kp = _kp.Value;
		nozzelControl.Kd = _kd.Value;
		nozzelControl.Bias = _bias.Value;
		


	}
}
