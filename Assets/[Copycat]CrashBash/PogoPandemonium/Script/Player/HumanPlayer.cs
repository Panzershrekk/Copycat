using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class HumanPlayer : Player
    {
        protected override void Update()
        {
            Vector2 dir = _inputActions.PogoPandemonium.Move.ReadValue<Vector2>();
            Debug.Log("dir " + dir);
            if (dir.x > 0.5)
            {
                _currenMoveDirection = MoveDirection.East;

            }
            else if (dir.x < -0.5)
            {
                _currenMoveDirection = MoveDirection.West;

            }
            else if (dir.y > 0.5)
            {
                _currenMoveDirection = MoveDirection.North;

            }
            else if (dir.y < -0.5)
            {
                _currenMoveDirection = MoveDirection.South;
            }
            else
            {
                _currenMoveDirection = MoveDirection.None;
            }
            base.Update();
        }
    }
}
