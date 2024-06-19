using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PangoPandemonium
{
    public class Player : MonoBehaviour
    {
        public Pogotile startingTile;
        public Material associatedColorMaterial;
        public Pogotile CurrentStandingPogoTile { get; private set; }
        [SerializeField] private ActionHandler _actionHandler;
        [SerializeField] private float _moveTickInSecond = 1f;
        private float _currentTickMove = 0;

        void Awake()
        {
            Arena.Instance.onGameSetup.AddListener(PlayerSetup);
        }

        void Update()
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
            _actionHandler.ProcessDirection(this, MoveDirection.East);
        }

        public void SetCurrentPogotile(Pogotile pogotile)
        {
            pogotile.SetOwner(this);
            CurrentStandingPogoTile = pogotile;
        }

        void OnDestroy()
        {
            Arena.Instance.onGameSetup.RemoveListener(PlayerSetup);
        }
    }
}
