using System;
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
        [SerializeField] private PlayerInfo _playerInfo;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _buffIndicatorParent;

        protected IBuff _currentBuff;
        protected float _currentTickMove = 0;
        private int _point = 0;
        private bool _canMove = false;
        protected MoveDirection _currenMoveDirection = MoveDirection.None;
        protected CopyCatInputSystem _inputActions;
        protected float _currentTickMoveSpeed = 0;
        public float CurrentTickMoveSpeed
        {
            get
            {
                return _currentTickMoveSpeed;
            }
            protected set
            {
                _animator.speed = value == 0 ? GameConstant.BASE_MOVE_TICK_TIME : 1 / value;
                _currentTickMoveSpeed = value;
            }
        }

        void Start()
        {
            _inputActions = new CopyCatInputSystem();
            _inputActions.PogoPandemonium.Enable();
            _inputActions.PogoPandemonium.UseBonus.performed += UseActive;
        }

        protected virtual void Update()
        {
            if (_currentTickMove < 0)
            {
                ProcessAction();
                _currentTickMove = CurrentTickMoveSpeed;
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
            CurrentTickMoveSpeed = GameConstant.BASE_MOVE_TICK_TIME;
            _currentTickMove = CurrentTickMoveSpeed;
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

        public void UseActive(InputAction.CallbackContext callbackContext)
        {
            if (CanUseActiveBuff() == true)
            {
                ((IActiveBonus)_currentBuff).Use(this);
            }
        }

        protected bool CanUseActiveBuff()
        {
            if (_currentBuff != null && _currentBuff is IActiveBonus)
            {
                return true;
            }
            return false;
        }

        public void ApplyBuff(IBuff buff)
        {
            if (_currentBuff != null)
            {
                _currentBuff.Remove(this);
            }
            _currentBuff = buff;
            _currentBuff.Apply(this);
        }

        public void RemoveBuff()
        {
            if (_currentBuff != null)
            {
                _currentBuff.Remove(this);
                _currentBuff = null;
            }
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

        public void SetSpeed(float speed)
        {
            CurrentTickMoveSpeed = speed;
        }

        public Transform GetBuffIndicatorParent()
        {
            return _buffIndicatorParent;
        }

        public int GetPoint()
        {
            return _point;
        }

    }
}
