using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class MissileDamager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Destroy(this.gameObject, 5f);
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(Vector3.forward * GameConstant.MISSILE_SPEED * Time.deltaTime);
        }
    }
}