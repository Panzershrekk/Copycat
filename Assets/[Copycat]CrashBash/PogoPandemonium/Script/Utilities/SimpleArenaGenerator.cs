using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PogoPandemonium
{
    public class SimpleArenaGenerator : MonoBehaviour
    {
        [SerializeField] private Pogotile _tile;

        // Start is called before the first frame update
        void Start()
        {
            Generate();
        }

        public void Generate()
        {
            for (int z = 0; z < GameConstant.Z_ARENA_SIZE; z++)
            {
                for (int x = 0; x < GameConstant.X_ARENA_SIZE; x++)
                {
                    Pogotile tile = Instantiate(_tile, new Vector3(x, 0, z), Quaternion.identity, this.transform);
                    tile.transform.name = String.Format("[{0}, {1}]", x, z);
                    tile.SetCoordinate(z, x);
                }
            }
        }
    }
}
