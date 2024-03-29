﻿using System;
using System.Numerics;
using WeepingSnake.Game.Utility.Extensions;

namespace WeepingSnake.Game.Player
{
    public readonly struct PlayerDirection
    {
        private readonly Vector2 _direction;
        private readonly float _length;

        public PlayerDirection(float directionX, float directionY) : this()
        {
            _direction.X = directionX;
            _direction.Y = directionY;
            _length = _direction.Length();
        }

        public float X
        {
            get
            {
                return _direction.X;
            }
        }

        public float Y
        {
            get
            {
                return _direction.Y;
            }
        }

        public Vector2 DirectionVector
        {
            get
            {
                return _direction;
            }
        }

        public float Length
        {
            get
            {
                return _length;
            }
        }

        /// <summary>
        /// Calculates a random direction vector with Length = 1.
        /// </summary>
        public static PlayerDirection RandomPlayerDirection()
        {
            var defaultDirection = new Vector2(0, GameConfiguration.DefaultDistance);
            var random = new Random();

            var rotations = random.Next(0, 360 / GameConfiguration.MinimumRotationAngle);
            var rotatedDirection = defaultDirection.RotateRight(GameConfiguration.MinimumRotationAngle * rotations);
            return new PlayerDirection(rotatedDirection.X, rotatedDirection.Y);
        }

        public PlayerDirection Apply(PlayerAction.Action action) => action switch
        {
            PlayerAction.Action.CHANGE_NOTHING => new(X, Y),
            PlayerAction.Action.TURN_LEFT => _direction.RotateLeft(GameConfiguration.MinimumRotationAngle),
            PlayerAction.Action.TURN_RIGHT => _direction.RotateRight(GameConfiguration.MinimumRotationAngle),
            PlayerAction.Action.SPEED_UP => _direction.Increase(),
            PlayerAction.Action.SLOW_DOWN => _direction.Decrease(),
            PlayerAction.Action.JUMP => new(X, Y),
            _ => throw new ArgumentOutOfRangeException(nameof(action), $"Not expected ${nameof(action)} value: {action}"),
        };

        public override bool Equals(object obj)
        {
            return obj is PlayerDirection direction && 
                   _direction.Equals(direction._direction);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_direction, Length);
        }

        public static bool operator ==(PlayerDirection left, PlayerDirection right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PlayerDirection left, PlayerDirection right)
        {
            return !(left == right);
        }

        public static implicit operator PlayerDirection(Vector2 direction)
        {
            return new(direction.X, direction.Y);
        }

        public override string ToString()
        {
            return $"({X}|{Y})";
        }

    }
}
