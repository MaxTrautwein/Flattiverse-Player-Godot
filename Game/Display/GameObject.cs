using Godot;
using System;
using System.Linq;
using Flattiverse.Connector;
using Flattiverse.Connector.Units;
using Flattiverse.Utils;

public partial class GameObject : Node2D
{

	private Unit _unit = null;
	
	// Called when the node enters the scene tree for the first time.
	public GameObject(Unit unit)
	{
		_unit = unit;
		
	}

	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		QueueRedraw();
	}

	protected virtual Color UnitColor => Colors.Red;
	protected virtual Color TextColor => Colors.White;
	protected virtual string DisplayString => string.Format($"{_unit.Name}");
	
	protected Vector2 GoDotPos => DisplayHelper.TransformToDisplay(_unit.Position.ToGodot());

	public override void _Draw()
	{
		base._Draw();
		
		DrawCircle(GoDotPos, (float)(_unit.Radius * DisplayHelper.Zoom),UnitColor);
		DrawMultilineString(ThemeDB.FallbackFont,GoDotPos,DisplayString, fontSize: Mathf.CeilToInt(16 * DisplayHelper.Zoom) , modulate: TextColor );
		
	}

	/// <summary>
	/// Kindof works but there is quite a bit to do to make this consistently nice
	/// </summary>
	/// <param name="text"></param>
	private void drawRotatedString(string text)
	{
		var increment = 360 / text.Length;
		var ang = 0;
		for (int i = 0; i < text.Length; i++)
		{
			string letter = text[i].ToString();
			drawRotated(ang, letter);
			ang += increment;
		}
		DrawSetTransform(Vector2.Zero, 0);
	}
	private void drawRotated(double rotation,string caracter)
	{
		var rad = (float)Mathf.DegToRad(rotation);
		var pos1 = GoDotPos + Vector2.Up.Rotated(rad) * (float)(_unit.Radius * DisplayHelper.Zoom - 16);
		DrawSetTransform(pos1,  rad);
		DrawChar(ThemeDB.FallbackFont,Vector2.Zero, caracter,16,Colors.Red );
	}
	
}
