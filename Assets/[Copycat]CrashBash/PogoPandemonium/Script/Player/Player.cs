using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PogoPandemonium
{
    public class Player : MonoBehaviour
    {
        public Pogotile startingTile;
        public Material associatedColorMaterial;
        public Pogotile CurrentStandingPogoTile { get; private set; }
        [SerializeField] private ActionHandler _actionHandler;
        [SerializeField] private float _moveTickInSecond = 1f;
        private float _currentTickMove = 0;
        protected MoveDirection _currenMoveDirection = MoveDirection.None;
        protected CopyCatInputSystem _inputActions;

        void Start()
        {
            _inputActions = new CopyCatInputSystem();
            _inputActions.PogoPandemonium.Enable();
            _inputActions.PogoPandemonium.UseBonus.performed += TestToDeleteUseBonus;
        }

        protected virtual void Update()
        {
            _currentTickMove -= Time.deltaTime;
            if (_currentTickMove < 0)
            {
                ProcessAction();
                _currentTickMove = _moveTickInSecond;
            }
        }

        public void PlayerSetup()
        {
            startingTile.SetOwner(this);
            CurrentStandingPogoTile = startingTile;
            _currentTickMove = _moveTickInSecond;
        }

        private void ProcessAction()
        {
            _actionHandler.ProcessDirection(this, _currenMoveDirection);
        }

        public void SetCurrentPogotile(Pogotile pogotile)
        {
            pogotile.SetOwner(this);
            CurrentStandingPogoTile = pogotile;
        }

        public void TestToDeleteUseBonus(InputAction.CallbackContext callbackContext)
        {
            Debug.Log("Item used " + callbackContext.phase);
        }

        public float GetSpeed()
        {
            return _moveTickInSecond;
        }
    }
}
