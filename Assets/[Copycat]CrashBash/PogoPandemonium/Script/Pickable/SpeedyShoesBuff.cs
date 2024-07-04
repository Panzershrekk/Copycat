using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class SpeedyShoesBuff : Buff
    {
        public SpeedyShoesBuff(GameObject indicatorPrefab) : base(indicatorPrefab) { }

        private float _buffDuration = GameConstant.SPEEDY_SHOES_DURATION_IN_SECOND;
        private Coroutine _coroutine;
        public override void Apply(Player player)
        {
            player.SetSpeed(GameConstant.SPEEDY_SHOES_TICK_TIME);
            if (_coroutine != null)
            {
                player.StopCoroutine(_coroutine);
            }
            _coroutine = player.StartCoroutine(RemoveBuffAfterDuration(player));
            base.Apply(player);
        }

        public override void Remove(Player player)
        {
            if (_coroutine != null)
            {
                player.StopCoroutine(_coroutine);
            }
            player.SetSpeed(GameConstant.BASE_MOVE_TICK_TIME);
            base.Remove(player);
        }

        private IEnumerator RemoveBuffAfterDuration(Player player)
        {
            yield return new WaitForSeconds(_buffDuration);
            player.RemoveBuff();
        }
    }
}