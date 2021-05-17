﻿using System;

namespace WeepingSnake.ConsoleClient
{
    static class Program
    {
        static void Main(string[] args)
        {
            var gctrl = new Game.GameController(2, 50);

            var playerA = gctrl.JoinGame();
            playerA.AssignedGame.OnLoopTick += PrintGameState;
            gctrl.DoAction(playerA, Game.Player.PlayerAction.Action.SPEED_UP);
        }

        private static void PrintGameState(Game.Game sender)
        {
            Console.WriteLine($"Gamestate of Game {sender.GameId}");
        }
    }
}
