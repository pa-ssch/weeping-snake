﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeepingSnake.Game.Geometry;
using WeepingSnake.Game.Player;
using WeepingSnake.Game.Tests.Game;

namespace WeepingSnake.Game.Tests.Player
{
    public class MockPlayer : IPlayer
    {
        public IGame AssignedGame { get; set; }

        public WeepingSnake.Game.Person.Person ControllingPerson { get; set; }

        public bool IsAlive { get; set; }

        public bool IsGuest { get; set; }

        public bool IsHuman { get; set; }

        public PlayerOrientation Orientation { get; set; }

        public Guid PlayerId { get; set; }

        public int Points { get; set; }

        public void AddAction(PlayerAction.Action action)
        {
            AddActionAction?.Invoke(action);
        }

        public void AddActions(Queue<PlayerAction.Action> actions)
        {
            if (Actions.TryGetValue("AddActions", out var action))
            {
                action();
            }
        }

        public GameDistance? ApplyOrientationAndMove(PlayerOrientation newOrientation)
        {
            if(ApplyOrientationAndMoveFunc is not null)
            {
                return ApplyOrientationAndMoveFunc(newOrientation);
            }

            return null;
        }

        public void ApplyPointsToPerson()
        {
            if (Actions.TryGetValue("ApplyPointsToPerson", out var action))
            {
                action();
            }
        }

        public void Die()
        {
            if(Actions.TryGetValue("Die", out var action))
            {
                action();
            }
        }

        public void Join(IGame game)
        {
            JoinAction?.Invoke(game);
        }

        public PlayerAction PopNextAction()
        {
            return PopNextActionFunc?.Invoke() ?? new PlayerAction(null, new PlayerOrientation(), PlayerAction.Action.CHANGE_NOTHING);
        }
        
        public Dictionary<string, Action> Actions { get; set; }

        public Func<PlayerOrientation, GameDistance?> ApplyOrientationAndMoveFunc { get; set; }

        public Action<PlayerAction.Action> AddActionAction { get; set; }
        public Action<IGame> JoinAction { get; set; }

        public Func<PlayerAction> PopNextActionFunc { get; set; }
    }
}
