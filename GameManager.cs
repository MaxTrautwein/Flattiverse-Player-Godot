using System;
using System.Collections.Generic;
using Flattiverse.Connector;
using Flattiverse.Connector.Events;
using Flattiverse.Connector.Hierarchy;
using Flattiverse.Connector.MissionSelection;
using Godot;

public static class GameManager
{

    private static Galaxy _galaxy = null;
    private static Universe _universe = null;
    private static Controllable _PlayerShip = null;

    public static Galaxy Galaxy => _galaxy;
    public static Universe Universe => _universe;
    public static async void init()
    {
        _universe = new Universe();
        //GD.Print($"Connected to {_galaxy.Name}");
    }

    private static string _galaxyName = "Beginners Course";
    private static string _teamName = "Plebs";
    public static async void Connect()
    {
        var targetGalaxy = _universe.Galaxies[_galaxyName];
        _galaxy = await targetGalaxy.Join(apiKey, targetGalaxy.Teams[_teamName]);
        
        var Players = _galaxy.Players;
        foreach (var player in Players)
        {
            GD.Print($"Player: {player.Name} in {player.Team.Name}");
        }

        //_galaxy.
        
        PrintControllabels();
        CreateShip();
        PrintControllabels();
        
        while (true)
        {
            FlattiverseEvent @event = await _galaxy.NextEvent();

            GD.Print(@event);
        }


    }

    private static void PrintControllabels()
    {
        var controllables = _galaxy.Controllables;
        foreach (var thing in controllables)
        {
            GD.Print($" {thing.Id} - {thing.Name}");
        }
    }

    public static async void CreateShip()
    {
        _PlayerShip = await _galaxy.RegisterShip("MaxShipName", _galaxy.ShipsDesigns["Cruiser"]);

        GD.Print($"Ship: {_PlayerShip.Name}, maxEnergy={_PlayerShip.Energy}/{_PlayerShip.EnergyMax}");

        await _PlayerShip.Continue();
    }

 
    
    
    private const string apiKey = "ADD_Key";

}