using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PogoPandemonium
{
    public class Player : MonoBehaviour
    {
        public Pogotile startingTile;
        public float baseRotation;
        public Material associatedColorMaterial;
        public Pogotile CurrentStandingPogoTile { get; private set; }
        [SerializeField] private ActionHandler _actionHandler;
        [SerializeField] private float _moveTickInSecond = 1f;
        [SerializeField] private PlayerInfo _playerInfo;
        protected float _currentTickMove = 0;
        private int _point = 0;
        private bool _canMove = false;
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
            if (_currentTickMove < 0)
            {
                ProcessAction();
                _currentTickMove = _moveTickInSecond;
            }
        }

        private void LateUpdate()
        {
            _currentTickMove -= Time.deltaTime;
        }

        public void PlayerSetup()
        {
            SetPoint(0);
            startingTile.SetOccupiedByPlayer(true);
            startingTile.SetOwner(this);
            CurrentStandingPogoTile = startingTile;
            _currentTickMove = _moveTickInSecond;
        }

        private void ProcessAction()
        {
            if (_canMove == false)
            {
                _currenMoveDirection = MoveDirection.None;
            }
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

        public void PositionPlayerToStartingTile()
        {
            _actionHandler.JumpTween.Kill();
            this.transform.rotation = Quaternion.Euler(0, baseRotation, 0);
            this.transform.position = startingTile.transform.position + new Vector3(0.5f, 0, 0.5f);
        }

        public void SetPoint(int point)
        {
            _point = point;
            if (_playerInfo != null)
            {
                _playerInfo.UpdatePointText(_point);
            }

        }

        public void AddPoint(int point)
        {
            _point += point;
            if (_playerInfo != null)
            {
                _playerInfo.UpdatePointText(_point);
            }
        }

        public void AllowMovement(bool canMove)
        {
            _canMove = canMove;
        }

        public float GetSpeed()
        {
            return _moveTickInSecond;
        }

        public int GetPoint()
        {
            return _point;
        }
    }
}
