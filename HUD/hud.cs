using Godot;
using System;

public partial class hud : CanvasLayer
{

	private LineEdit _chatMsgLine;
	private Label _fpsDisplay;
	private Label _statusLine;
	private RichTextLabel _chatBox;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_chatMsgLine = GetNode<LineEdit>("ChatLine");
		_fpsDisplay = GetNode<Label>("FPS_Cnt");
		_statusLine = GetNode<Label>("StatusBar");
		_chatBox = GetNode<RichTextLabel>("ChatBox");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
