using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PogoPandemonium
{
    public class HumanPlayer : Player
    {
        private  CopyCatInputSystem _inputActions;

        void Start()
        {
            _inputActions = new CopyCatInputSystem();
            _inputActions.PogoPandemonium.Enable();
            _inputActions.PogoPandemonium.UseBonus.performed += DoAction;
        }

        protected override void Update()
        {
            Vector2 dir = _inputActions.PogoPandemonium.Move.ReadValue<Vector2>();
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

        public void DoAction(InputAction.CallbackContext context)
        {
            base.UseActive();
        }
    }
}
