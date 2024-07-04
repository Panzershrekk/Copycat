using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class MissileDamager : MonoBehaviour
    {
        private Player _owner;
        private bool _isSetup = false;

        // Start is called before the first frame update
        void Start()
        {
            Destroy(this.gameObject, 8f);
        }

        // Update is called once per frame
        void Update()
        {
            if (_isSetup == true)
            {
                transform.Translate(Vector3.forward * GameConstant.MISSILE_SPEED * Time.deltaTime);
            }
        }

        public void Setup(Player owner)
        {
            _owner = owner;
            _isSetup = true;
        }

        public void OnTriggerEnter(Collider other)
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player != _owner)
            {
                player.StunPlayer(true);
                Destroy(this.gameObject);
            }
        }
    }
}