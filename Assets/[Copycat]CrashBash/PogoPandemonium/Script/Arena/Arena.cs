using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        [SerializeField] private ArrowBonus _arrowBonusPrefab;
        [SerializeField] private Missile _missilePrefab;
        [SerializeField] private SpeedyShoes _speedyShoesPrefabs;

        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private GameSequences _introSequence;

        private UnityEvent _onGameSetup = new UnityEvent();
        private ArenaTiles _arenaTiles = new ArenaTiles();
        private List<Player> _players = new List<Player>();

        private List<PointCrate> _pointCrates = new List<PointCrate>();
        private List<ArrowBonus> _arrowBonus = new List<ArrowBonus>();
        private List<Missile> _missile = new List<Missile>();
        private List<SpeedyShoes> _speedyShoes = new List<SpeedyShoes>();

        private float _currentTickTime = 0f;
        private float _currentRoundTime = 0f;
        public float CurrentRoundTime
        {
            get
            {
                return _currentRoundTime;
            }

            private set
            {
                _currentRoundTime = value;
                UpdateTimerText(_currentRoundTime);
            }
        }

        private bool _gameStarted = false;

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
            _introSequence.onStartSequenceOver.AddListener(StartRound);
            _introSequence.onEndSequenceOver.AddListener(GameSetup);
            RegisterTiles();
            RegisterPlayer();
            GameSetup();
        }

        private void Update()
        {
            if (_gameStarted == true)
            {
                if (_currentTickTime < 0)
                {
                    SpawnerHandler();
                    _currentTickTime = _tickCheck;
                }
                CurrentRoundTime -= Time.deltaTime;
                _currentTickTime -= Time.deltaTime;
                if (CurrentRoundTime <= 0)
                {
                    AllowPlayerMovement(false);
                    _introSequence.StartEndSequence();
                    _gameStarted = false;
                }
            }
        }

        private void GameSetup()
        {
            _gameStarted = false;

            ClearGivenPickableList(_pointCrates);
            ClearGivenPickableList(_arrowBonus);
            ClearGivenPickableList(_missile);
            ClearGivenPickableList(_speedyShoes);

            foreach (Player player in _players)
            {
                if (player != null)
                {
                    player.PositionPlayerToStartingTile();
                }
            }
            ResetAllPogoTile();
            CurrentRoundTime = GameConstant.ROUND_TIME_IN_SECOND;
            AllowPlayerMovement(false);
            _onGameSetup?.Invoke();
            _introSequence.StartIntroSequence();
        }

        private void StartRound()
        {
            _gameStarted = true;
            AllowPlayerMovement(true);
        }

        public void ValidatePointForPlayer(Player player, PointCrate pointCrate)
        {
            _pointCrates.Remove(pointCrate);
            List<Pogotile> pogotiles = GetPlayerTiles(player);
            player.AddPoint(pogotiles.Count);
            PogoPandemoniumUIManager.Instance.DisplayScore(pointCrate.transform.position, pogotiles.Count);
            foreach (Pogotile pogotile in pogotiles)
            {
                pogotile.SetOwner(null, true);
            }
        }

        private void SpawnerHandler()
        {
            int numberOfCrateToSpawn = _maxCrateOnArena - _pointCrates.Count;
            SpawnPickable(_pointBoxPrefab, _pointCrates, numberOfCrateToSpawn);

            int numberOfArrowToSpawn = 1;
            SpawnPickable(_arrowBonusPrefab, _arrowBonus, numberOfArrowToSpawn);

            int numberOfMissileToSpawn = 1;
            SpawnPickable(_missilePrefab, _missile, numberOfMissileToSpawn);

            int numberOfShoesToSpawn = 1;
            SpawnPickable(_speedyShoesPrefabs, _speedyShoes, numberOfShoesToSpawn);
        }

        private void SpawnPickable<T>(T prefab, List<T> objectList, int numberToSpawn) where T : MonoBehaviour, IPickable
        {
            List<Pogotile> emptyTiles = GetEmptyTiles();
            for (int i = 0; i < numberToSpawn; i++)
            {
                Pogotile pogotile = emptyTiles[UnityEngine.Random.Range(0, emptyTiles.Count)];
                T obj = Instantiate(prefab, pogotile.transform.position + new Vector3(0.5f, 0f, 0.5f), Quaternion.identity);
                pogotile.SetOccupiedByObject(true, obj);
                emptyTiles.Remove(pogotile);
                objectList.Add(obj);
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

        private void ResetAllPogoTile()
        {
            foreach (LineTiles lineTile in _arenaTiles.lineTiles)
            {
                foreach (Pogotile pogotile in lineTile.pogotiles)
                {
                    pogotile.SetOwner(null);
                    pogotile.SetOccupiedByObject(false, null);
                    pogotile.SetOccupiedByPlayer(false);
                }
            }
        }

        private void AllowPlayerMovement(bool canMove)
        {
            foreach (Player p in _players)
            {
                p.AllowMovement(canMove);
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
                    if (tile.OccupiedByPlayer == false && tile.OccupiedByObject == false)
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

        public void FillTilesFromTileAndDirectionForPlayer(Pogotile pogotile, MoveDirection direction, Player player)
        {
            int x = 0;
            int z = 0;
            Pogotile currentPogoTile = pogotile;

            if (direction == MoveDirection.North)
            {
                z += 1;
            }
            if (direction == MoveDirection.East)
            {
                x += 1;
            }
            if (direction == MoveDirection.South)
            {
                z -= 1;
            }
            if (direction == MoveDirection.West)
            {
                x -= 1;
            }

            bool canGoNext = true;

            while (canGoNext == true)
            {
                currentPogoTile = GetPogotileAtCoordinate(currentPogoTile.Z + z, currentPogoTile.X + x);
                if (currentPogoTile == null)
                {
                    canGoNext = false;
                    break;
                }
                currentPogoTile.SetOwner(player, true);
            }
        }

        private void UpdateTimerText(float time)
        {
            TimeSpan t = TimeSpan.FromSeconds(time);
            string formatedTime = string.Format("{0:D2} : {1:D2}", t.Minutes, t.Seconds);
            if (_timerText != null)
            {
                _timerText.text = formatedTime;
            }
        }

        private void ClearGivenPickableList<T>(List<T> pickables) where T : MonoBehaviour, IPickable
        {
            foreach (T pickable in pickables)
            {
                if (pickable != null)
                {
                    Destroy(pickable.gameObject);
                }
            }
            pickables.Clear();
        }

        public void RemoveArrowFromItsList(ArrowBonus arrowBonus)
        {
            _arrowBonus.Remove(arrowBonus);
        }

        public void RemoveMissileFromItsList(Missile missile)
        {
            _missile.Remove(missile);
        }

        public void RemoveShoesFromItsList(SpeedyShoes shoes)
        {
            _speedyShoes.Remove(shoes);
        }

        private void OnDestroy()
        {
            foreach (Player player in _players)
            {
                _onGameSetup.RemoveListener(player.PlayerSetup);
            }
            _introSequence.onStartSequenceOver.RemoveListener(StartRound);
            _introSequence.onEndSequenceOver.RemoveListener(GameSetup);
        }
    }
}
