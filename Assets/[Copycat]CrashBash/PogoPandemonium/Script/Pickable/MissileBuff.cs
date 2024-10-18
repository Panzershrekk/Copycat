using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class MissileBuff : Buff, IActiveBonus
    {
        private MissileDamager _missileDamager;

        public MissileBuff(GameObject indicatorPrefab, MissileDamager damager) : base(indicatorPrefab)
        {
            _missileDamager = damager;
        }

        public override void Apply(Player player)
        {
            base.Apply(player);
        }

        public override void Remove(Player player)
        {
            base.Remove(player);
        }

        public void Use(Player player)
        {
            MoveDirection moveDirection = player.GetCurrentMoveDirection();
            float rotationY = player.transform.rotation.eulerAngles.y;
            if (moveDirection != MoveDirection.None)
            {
                rotationY = MoveDirectionExtensions.GetYRotationFromDirection(moveDirection);
            }
            MissileDamager missileDamager = GameObject.Instantiate(_missileDamager, new Vector3(player.transform.position.x, 0.5f, player.transform.position.z), Quaternion.Euler(0, rotationY, 0), null);
            missileDamager.Setup(player);
            player.RemoveBuff();
        }
    }
}