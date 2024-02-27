using Flattiverse.Connector.Hierarchy;
using Flattiverse.Connector.Units;
using Flattiverse.Utils;
using Godot;

namespace Flattiverse.Game;

public partial class DisplayOtherPlayer : GameObject
{
    private PlayerUnit _playerUnit;
    public DisplayOtherPlayer(Unit unit) : base(unit)
    {
        _playerUnit = unit as PlayerUnit;
    }

    public Team PlayerTeam => _playerUnit.Team;

    protected override Color UnitColor { get => Colors.Aqua; }

    public override void _Draw()
    {
        base._Draw();
        var pos = DisplayHelper.TransformToDisplay(_playerUnit.Position.ToGodot());
        DisplayHelper.DrawDirectionIndicator(this,(float)_playerUnit.Radius * DisplayHelper.Zoom ,pos,_playerUnit.Direction,Colors.Pink);
    }
    
    
    
}