using System;
using System.Collections.Generic;
using System.Linq;
using Flattiverse.Connector;
using Flattiverse.Connector.Events;
using Flattiverse.Connector.Hierarchy;
using Flattiverse.Connector.MissionSelection;
using Flattiverse.Connector.Units;
using Godot;

public static class GameManager
{

    private static Galaxy _galaxy = null;
    private static Universe _universe = null;
    private static Controllable _PlayerShip = null;

    public static Controllable PlayerShip => _PlayerShip;
    public static Galaxy Galaxy => _galaxy;
    public static Universe Universe => _universe;
    public static async void init()
    {
        _universe = new Universe();
        //GD.Print($"Connected to {_galaxy.Name}");
    }

    private static List<Unit> knownUnits = new List<Unit>();
    private static string _galaxyName = "Beginners Course";
    private static string _teamName = "Plebs";

    private static Player MyPlayer = null;
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
        _galaxy = await targetGalaxy.Join(apiKey, targetGalaxy.Teams[_teamName]);
        
        var Players = _galaxy.Players;
        foreach (var player in Players)
        {
            GD.Print($"Player: {player.Name} in {player.Team.Name}");
        }

        //_galaxy.
        CreateShip();
        MyPlayer = _galaxy.Players.First(P => P.Name == "matrit07");
        
        
        while (true)
        {
            FlattiverseEvent @event = await _galaxy.NextEvent();
            Unit _unit = null;
            switch (@event.Kind)
            {
                case EventKind.UnitAdded:
                    _unit = ((UnitEvent)@event).Unit;
                   // knownUnits.Add(_unit);
                    if (_unit.Name == ShipName && _unit.Kind == UnitKind.PlayerUnit) break;
                    game.RegisterUnit(_unit);
                    GD.Print($"Added {_unit.Name} @{_unit.Position} - {_unit.Radius} - {_unit.Gravity}");
                    break;
                case EventKind.UnitUpdated:
                    _unit = ((UnitEvent)@event).Unit;
                    //GD.Print($"{_unit.Name} @{_unit.Position}");
                    //From what i understood the logic for is is handeled automaticlly
                    break;
                case EventKind.UnitVanished:
                    _unit = ((UnitEvent)@event).Unit;
                    //knownUnits.Remove(_unit);
                    game.DeRegisterUnit(_unit);

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
                default:
                    throw new ArgumentOutOfRangeException();
            }
            //GD.Print(@event);
        }
    }

    private static string ShipName => "MaxShipName";
    public static async void CreateShip()
    {
        _PlayerShip = await _galaxy.RegisterShip(ShipName, _galaxy.ShipsDesigns["Cruiser"]);
        
        GD.Print($"Ship: {_PlayerShip.Name}, maxEnergy={_PlayerShip.Energy}/{_PlayerShip.EnergyMax}");

        await _PlayerShip.Continue();
        
        
    }

 
    
    
    private static string apiKey = "SetMe";
    public static string ApiKey
    {
        set => apiKey = value;
    }

}