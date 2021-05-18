﻿using System;
using System.Linq;
using System.Collections.Generic;
using WeepingSnake.Game.Geometry;
using WeepingSnake.Game.Player;
using WeepingSnake.Game.Structs;
using WeepingSnake.Game.Utility.Extensions;

namespace WeepingSnake.Game
{
    public sealed partial class Game
    {
        public sealed class Board : CoordinateSystem
        {
            private readonly bool _isInfinite;
            private readonly List<List<GameDistance>> _paths;

            internal Board(BoardDimensions dimensions) : base(dimensions)
            {
                _isInfinite = dimensions.IsInfinite;
                _paths = new List<List<GameDistance>>();
            }

            internal bool IsInfinite => _isInfinite;

            internal void ApplyAction(PlayerAction action)
            {
                var roundNumber = action.NewOrientation.Position.Z;

                var newPath = action.Apply();

                var currentRoundPathList = _paths.GetOrCreate(roundNumber - 1, () => new List<GameDistance>());
                currentRoundPathList.Add(newPath);

                var newPathPoints = CalculatePointsOnLine(newPath);

                #warning for the last 5 rounds: calculate points; for older rounds: die.
                for(int i = 2; i <= Math.Min(6, _paths.Count); i++)
                {
                    _paths[^i].ForEach(path =>
                    {
                        var oldPathPoints = CalculatePointsOnLine(path);
                        if (oldPathPoints.Intersect(newPathPoints).Any())
                        {
                            if(path.Player == newPath.Player)
                            {
                                if(path.EndX != newPath.StartX || path.EndY != newPath.StartY)
                                    path.Player.Points -= 10;
                            }
                            else
                            {
                                newPath.Player.Points += 2;
                                path.Player.Points -= 1;
                            }
                        }
                    });
                }
            }

            internal PlayerOrientation CalculateRandomStartOrientation()
            {
                var direction = PlayerDirection.RandomPlayerDirection();
                var position = CalculateRandomStartCoordinates();

                return new PlayerOrientation(position, direction);
            }

            private GameCoordinate CalculateRandomStartCoordinates()
            {
                var zPosition = (ushort) _paths.Count;
                var possiblePositions = new HashSet<GameCoordinate>();

#warning TODO: do not use 1 hardcoded, but use a const like "default distance"
                for (var x = 1; x < Width - 1; x++)
                    for (var y = 1; y < Height - 1; y++)
                        possiblePositions.Add(new GameCoordinate(x, y, zPosition));

                if (zPosition > 0)
                {
                    foreach (var pathPoints in _paths[^1].Select(vector => CalculatePointsOnLine(0, 0, 0, 0)))
                    {
                        foreach (var (pathX, pathY) in pathPoints)
                        {
                            var coordinate = new GameCoordinate(pathX, pathY, zPosition);
                            possiblePositions.Remove(coordinate);

                            foreach (var (neighborX, neighborY) in coordinate.MooreNeighborhood())
                            {
                                var neighbor = new GameCoordinate(neighborX, neighborY, zPosition);
                                possiblePositions.Remove(neighbor);
                            }
                        }
                    }
                }

#warning TODO: select the one in the largest cluster (Lloyd) nearest to the center
                return possiblePositions.Random();
            }

            internal List<List<GameDistance>> Paths => _paths;
        }
    }
}
