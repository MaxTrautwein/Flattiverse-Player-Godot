using System;
using System.Collections.Generic;
using System.Linq;
using Flattiverse.Connector;
using Flattiverse.Connector.Events;
using Flattiverse.Connector.Hierarchy;
using Flattiverse.Connector.MissionSelection;
using Flattiverse.Connector.Units;
using Godot;

namespace Flattiverse.Game;

public static class GameManager
{

    private static Galaxy _galaxy;
    private static Universe _universe;
    private static Controllable _playerShip;

    public static Controllable PlayerShip => _playerShip;
    public static Galaxy Galaxy => _galaxy;
    public static Universe Universe => _universe;
    public static void Init()
    {
        _universe = new Universe(); 
    }

    private static List<Unit> _knownUnits = new List<Unit>();
    private static string _galaxyName = "Beginners Course";
    private static string _teamName = "Plebs";

    private static Player _myPlayer;
    public static string GalaxyName
    {
        get => _galaxyName;
        set => _galaxyName = value;
    }
    public static string TeamName
    {
        get => _teamName;
        set => _teamName = value;
    }
    
    public static async void Connect()
    {
        var targetGalaxy = _universe.Galaxies[_galaxyName];
        
        GD.Print($"Connecting to '{_galaxyName}' and Team '{_teamName}'");
        _galaxy = await targetGalaxy.Join(_apiKey, targetGalaxy.Teams[_teamName]);
        
        var players = _galaxy.Players;
        foreach (var player in players)
        {
            GD.Print($"Player: {player.Name} in {player.Team.Name}");
        }

        //_galaxy.
        CreateShip();
        _myPlayer = _galaxy.Players.First(p => p.Name == "matrit07");
        
        
        while (true)
        {
            FlattiverseEvent @event = await _galaxy.NextEvent();
            Unit unit = null;
            switch (@event.Kind)
            {
                case EventKind.UnitAdded:
                    unit = ((UnitEvent)@event).Unit;
                    // knownUnits.Add(_unit);
                    if (unit.Name == ShipName && unit.Kind == UnitKind.PlayerUnit) break;
                    game.RegisterUnit(unit);
                    GD.Print($"Added {unit.Name} @{unit.Position} - {unit.Radius} - {unit.Gravity}");
                    break;
                case EventKind.UnitUpdated:
                    break;
                case EventKind.UnitVanished:
                    unit = ((UnitEvent)@event).Unit;
                    //knownUnits.Remove(_unit);
                    game.DeRegisterUnit(unit);

                    break;
                case EventKind.JoinedPlayer:
                    break;
                case EventKind.PartedPlayer:
                    break;
                case EventKind.GalaxyTick:
                    break;
                case EventKind.JoinedControllable:
                    break;
                case EventKind.PartedControllable:
                    break;
                case EventKind.GalaxyChat:
                case EventKind.PlayerChat:
                case EventKind.TeamChat:
                    hud.RegisterChatMsg((@event as ChatEvent)?.ToString());
                    break;

                case EventKind.DeathByShutdown:
                case EventKind.DeathBySelfDestruction:
                case EventKind.DeathByNeutralCollision:
                case EventKind.DeathByControllableCollision:
                    GD.Print($"Death Event: {@event}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //GD.Print(@event);
        }
    }

    private static string ShipName => "MaxShipName";

    private static async void CreateShip()
    {
        _playerShip = await _galaxy.RegisterShip(ShipName, _galaxy.ShipsDesigns["Cruiser"]);
        
        GD.Print($"Ship: {_playerShip.Name}, maxEnergy={_playerShip.Energy}/{_playerShip.EnergyMax}");

        await _playerShip.Continue();
        
        
    }

 
    
    
    private static string _apiKey = "SetMe";
    public static string ApiKey
    {
        set => _apiKey = value;
    }

}