﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WeepingSnake.Game.Player
{
    /// <summary>
    /// A concrete action a player wants to do at a concrete time
    /// </summary>
    public sealed class PlayerAction
    {
        private readonly Player _player;
        private readonly PlayerOrientation _newOrientation;
        private readonly Action _action;

        public PlayerAction(Player player, PlayerOrientation orientation, Action action)
        {
            _player = player;
            _action = action;
            _newOrientation = orientation.ApplyAndMove(action);
        }

        public PlayerOrientation NewOrientation => _newOrientation;

        public enum Action
        {
            CHANGE_NOTHING,
            TURN_LEFT,
            TURN_RIGHT,
            SPEED_UP,
            SLOW_DOWN,
            JUMP,
        }
    }
}
