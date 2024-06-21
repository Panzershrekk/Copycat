using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PogoPandemonium
{
    [Serializable]
    public class LineTiles
    {
        public List<Pogotile> pogotiles = new List<Pogotile>();
    }

    [Serializable]
    public class ArenaTiles
    {
        public List<LineTiles> lineTiles = new List<LineTiles>();
    }


    public class Arena : MonoBehaviour
    {
        public static Arena Instance { get; private set; }
        [SerializeField] private PointCrate _pointBoxPrefab;
        private UnityEvent _onGameSetup = new UnityEvent();
        private ArenaTiles _arenaTiles = new ArenaTiles();
        private List<Player> _players = new List<Player>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            for (int z = 0; z < GameConstant.Z_ARENA_SIZE; z++)
            {
                _arenaTiles.lineTiles.Add(new LineTiles());
                for (int x = 0; x < GameConstant.X_ARENA_SIZE; x++)
                {
                    _arenaTiles.lineTiles[z].pogotiles.Add(null);
                }
            }
            RegisterTiles();
            RegisterPlayer();
            GameSetup();
            SpawnCrate();
        }

        private void GameSetup()
        {
            _onGameSetup?.Invoke();
        }



        public void ValidatePointForPlayer(Player player)
        {
            List<Pogotile> pogotiles = GetPlayerTiles(player);
            player.AddPoint(pogotiles.Count);
            foreach (Pogotile pogotile in pogotiles)
            {
                pogotile.SetOwner(null);
            }
        }

        private void SpawnCrate()
        {
            List<Pogotile> emptyTiles = GetEmptyTiles();
            Pogotile pogotile = emptyTiles[UnityEngine.Random.Range(0, emptyTiles.Count)];
            PointCrate crate = Instantiate(_pointBoxPrefab, pogotile.transform.position + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
            pogotile.SetOccupiedByObject(true, crate);
        }

        private void RegisterTiles()
        {
            foreach (Transform tile in transform)
            {
                Pogotile pogotile = tile.GetComponent<Pogotile>();
                if (pogotile != null)
                {
                    int x = pogotile.X;
                    int z = pogotile.Z;
                    _arenaTiles.lineTiles[z].pogotiles[x] = pogotile;

                }
            }
        }

        private void RegisterPlayer()
        {
            _players = FindObjectsByType<Player>(FindObjectsSortMode.None).ToList();
            foreach (Player player in _players)
            {
                _onGameSetup.AddListener(player.PlayerSetup);
            }
        }

        private void ResetOwnerOfAllPogoTile()
        {
            foreach (LineTiles lineTile in _arenaTiles.lineTiles)
            {
                foreach (Pogotile pogotile in lineTile.pogotiles)
                {
                    pogotile.SetOwner(null);
                }
            }
        }

        public Pogotile GetPogotileAtCoordinate(int z, int x)
        {
            if (z < 0 || z >= GameConstant.Z_ARENA_SIZE || x < 0 || x >= GameConstant.X_ARENA_SIZE)
                return null;
            return _arenaTiles.lineTiles[z].pogotiles[x];
        }

        public List<Pogotile> GetEmptyTiles()
        {
            List<Pogotile> emptyTiles = new List<Pogotile>();

            foreach (LineTiles linetile in _arenaTiles.lineTiles)
            {
                foreach (Pogotile tile in linetile.pogotiles)
                {
                    if (tile.OccupiedByObject == false && tile.OccupiedByObject == false)
                    {
                        emptyTiles.Add(tile);
                    }
                }
            }
            return emptyTiles;
        }

        public List<Pogotile> GetPlayerTiles(Player player)
        {
            List<Pogotile> playerTiles = new List<Pogotile>();

            foreach (LineTiles linetile in _arenaTiles.lineTiles)
            {
                foreach (Pogotile tile in linetile.pogotiles)
                {
                    if (tile.GetOwner() == player)
                    {
                        playerTiles.Add(tile);
                    }
                }
            }
            return playerTiles;
        }

        private void OnDestroy()
        {
            foreach (Player player in _players)
            {
                _onGameSetup.RemoveListener(player.PlayerSetup);
            }
        }
    }
}
