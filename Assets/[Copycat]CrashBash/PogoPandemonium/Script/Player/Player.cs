using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PangoPandemonium
{
    public class Player : MonoBehaviour
    {
        public Pogotile startingTile;
        public Material associatedColorMaterial;
        public ActionHandler actionHandler;
        public Pogotile CurrentStandingPogoTile { get; private set; }
        [SerializeField] private float _moveTickInSecond = 1f;
        void Start()
        {
            Arena.Instance.onGameSetup.AddListener(PlayerSetup);
        }

        void Update()
        {
            
        }

        public void PlayerSetup()
        {
            startingTile.SetOwner(this);
        }

        private void ProcessAction()
        {

        }

        public void SetCurrentPogotile(Pogotile pogotile)
        {
            CurrentStandingPogoTile = pogotile;
        }

        void OnDestroy()
        {
            Arena.Instance.onGameSetup.RemoveListener(PlayerSetup);
        }
    }
}
