using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public enum MoveDirection
    {
        None = 0,
        North = 1,
        East = 2,
        South = 3,
        West = 4
    }

    public static class MoveDirectionExtensions
    {
        public static int GetYRotationFromDirection(MoveDirection dir)
        {
            if (dir == MoveDirection.North || dir == MoveDirection.None)
            {
                return 0;
            }
            if (dir == MoveDirection.East)
            {
                return 90;
            }
            if (dir == MoveDirection.South)
            {
                return 180;
            }
            if (dir == MoveDirection.East)
            {
                return 270;
            }
            return 0;
        }
    }
}