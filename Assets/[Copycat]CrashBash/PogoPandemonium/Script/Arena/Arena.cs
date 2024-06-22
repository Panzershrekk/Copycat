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
        //Tick time to check for bonus crate etc ...
        [SerializeField] private float _tickCheck = 5f;
        [SerializeField] private int _maxCrateOnArena = 2;
        [SerializeField] private PointCrate _pointBoxPrefab;
        private UnityEvent _onGameSetup = new UnityEvent();
        private ArenaTiles _arenaTiles = new ArenaTiles();
        private List<Player> _players = new List<Player>();
        private List<PointCrate> _pointCrates = new List<PointCrate>();
        private float _currentTickTime = 0f;

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
        }

        private void Update()
        {
            if (_currentTickTime < 0)
            {
                SpawnCrates();
                _currentTickTime = _tickCheck;
            }
            _currentTickTime -= Time.deltaTime;
        }

        private void GameSetup()
        {
            _onGameSetup?.Invoke();
        }

        public void ValidatePointForPlayer(Player player, PointCrate pointCrate)
        {
            _pointCrates.Remove(pointCrate);
            List<Pogotile> pogotiles = GetPlayerTiles(player);
            player.AddPoint(pogotiles.Count);
            foreach (Pogotile pogotile in pogotiles)
            {
                pogotile.SetOwner(null);
            }
        }

        private void SpawnCrates()
        {
            int numberToSpawn = _maxCrateOnArena - _pointCrates.Count;
            List<Pogotile> emptyTiles = GetEmptyTiles();
            for (int i = 0; i < numberToSpawn; i++)
            {
                Pogotile pogotile = emptyTiles[UnityEngine.Random.Range(0, emptyTiles.Count)];
                PointCrate crate = Instantiate(_pointBoxPrefab, pogotile.transform.position + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity);
                pogotile.SetOccupiedByObject(true, crate);
                emptyTiles.Remove(pogotile);
                _pointCrates.Add(crate);
            }
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

        public List<Pogotile> GetAdjacentTileOfTile(Pogotile pogotile)
        {
            List<Pogotile> adjacentTiles = new List<Pogotile>();

            Pogotile north = GetPogotileAtCoordinate(pogotile.Z + 1, pogotile.X);
            Pogotile east = GetPogotileAtCoordinate(pogotile.Z, pogotile.X + 1);
            Pogotile south = GetPogotileAtCoordinate(pogotile.Z - 1, pogotile.X);
            Pogotile west = GetPogotileAtCoordinate(pogotile.Z, pogotile.X - 1);
            if (north != null)
            {
                adjacentTiles.Add(north);
            }
            if (east != null)
            {
                adjacentTiles.Add(east);
            }
            if (south != null)
            {
                adjacentTiles.Add(south);
            }
            if (west != null)
            {
                adjacentTiles.Add(west);
            }
            return adjacentTiles;
        }

        public MoveDirection GetRelativeDirectionFromTile(Pogotile from, Pogotile to)
        {
            MoveDirection direction = MoveDirection.None;

            int z = to.Z - from.Z;
            int x = to.X - from.X;
            if (z > 0)
            {
                direction = MoveDirection.North;
            }
            else if (x > 0)
            {
                direction = MoveDirection.East;
            }
            else if (z < 0)
            {
                direction = MoveDirection.South;
            }
            else if (x < 0)
            {
                direction = MoveDirection.West;
            }
            return direction;
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
