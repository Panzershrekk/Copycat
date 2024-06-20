using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class AIPlayer : Player
    {

        // Update is called once per frame
        protected override void Update()
        {
            //Random for now
            _currenMoveDirection = (MoveDirection)Random.Range(1, 5);
            Debug.Log("_currenMoveDirection " + _currenMoveDirection);
            base.Update();
        }
    }
}