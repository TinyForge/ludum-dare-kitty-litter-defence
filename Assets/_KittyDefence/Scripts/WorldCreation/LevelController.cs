using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;
    public PlayerMovement Player;
    public AbilityManager AbilityManager;
    public Camera InventoryCamera;
    public int TotalLives = 10;
    public LevelDatabase[] Levels;
    public Action<int> OnLivesLost;
    public Action<bool, bool> OnGameCompleted;
    public Action<int> OnWaveStart;
    public Action OnWaveEnd;

    public PlayerMovement CurrentPlayer
    {
        get { return _player; }
    }

    private BoardGenerator PreviousBoard { get { return Levels[_currentLevel-1].BoardInfo; } }
    private BoardGenerator Board { get { return Levels[_currentLevel].BoardInfo; } }
    private WaveDatabase WaveInfo { get { return Levels[_currentLevel].WaveInfo; } }
    private WaveDatabase.WaveInformation[] Waves { get { return WaveInfo.Waves; } }


    private EnemySpawner[] _spawners;
    private List<EnemyMovement> _currentEnemies;
    private PlayerMovement _player;
    private AbilityManager _currentAbilityManager;

    private int _currentLives;
    private int _currentWave = 0;
    private int _currentLevel = 0;
    private int _totalEnemies = 0;

    private void Start()
    {
        Instance = this;
    }

    public void StartLevel()
    {
        Instance = this;
        _currentLives = TotalLives;
        _currentWave = 0;
        _totalEnemies = 0;
        _currentEnemies = null;
        _spawners = null;
        OnLivesLost = null;
        if (_currentLevel > 0)
            PreviousBoard.CleanupBoard();
        Board.LoadBoard();
        if (_player != null)
            Destroy(_player.gameObject);
        if (_currentAbilityManager != null)
            Destroy(_currentAbilityManager.gameObject);
        BoardGenerator.OnCompleted = null;
        BoardGenerator.OnCompleted += SetupLevel;
    }

    public List<EnemyMovement> GetCurrentEnemies()
    {
        if (_currentEnemies != null)
            return _currentEnemies;
        return new List<EnemyMovement>();
    }

    private void SetupLevel(Vector3 playerPos)
    {
        _spawners = FindObjectsOfType<EnemySpawner>();
        SpawnPlayer(playerPos);
        StartCoroutine(LevelLoop());
        InventoryCamera.enabled = true;
    }

    private void SpawnPlayer(Vector3 playerPos)
    {
        _player = Instantiate(Player, playerPos, Quaternion.identity);
        _currentAbilityManager = Instantiate(AbilityManager, InventoryCamera.transform, false);
    }

    private IEnumerator LevelLoop()
    {
        yield return new WaitForSeconds(1f);
        while (_currentWave != Waves.Length && _currentLives > 0)
        {
            _totalEnemies = 0;
            _currentEnemies = new List<EnemyMovement>();
            yield return new WaitForSeconds(5f);
            if (OnWaveStart != null)
                OnWaveStart.Invoke(_currentWave);

            yield return SpawnUnits(WaveInfo.EnemiesSmall, Waves[_currentWave].TotalSmall, Waves[_currentWave].SpawnDelay);
            yield return SpawnUnits(WaveInfo.EnemiesMedium, Waves[_currentWave].TotalMedium, Waves[_currentWave].SpawnDelay);
            yield return SpawnUnits(WaveInfo.EnemiesLarge, Waves[_currentWave].TotalLarge, Waves[_currentWave].SpawnDelay);

            while (_totalEnemies != 0)
            {
                yield return null;
            }

            if (OnWaveEnd != null)
                OnWaveEnd.Invoke();

            _currentWave++;
        }
        if (OnGameCompleted != null)
            OnGameCompleted.Invoke(_currentLives > 0, _currentLevel >= Levels.Length - 1);
        if (_currentLives <= 0)
            Debug.Log("You Lost!");
        else
        {
            _currentLevel++;
            Debug.Log("You Won!");
        }

        InventoryCamera.enabled = false;
    }

    private IEnumerator SpawnUnits(EnemyGroup enemies, int total, float delay)
    {
        for (int i = 0; i < total; i++)
        {
            var enemy = Instantiate(enemies.GetRandomEnemy(), GetNextSpawner(), Quaternion.identity);
            enemy.SetDestination(Board.GetStartPosition());
            enemy.OnDestinationReached += OnEnemyDestinationReached;
            enemy.HealthManager.OnDeath += OnEnemyKilled;
            enemy.HealthManager.SetHitPoints(enemies.HP);
            enemy.Agent.speed = enemies.Speed;
            enemy.Agent.angularSpeed = enemies.RotationSpeed;
            _currentEnemies.Add(enemy);
            _totalEnemies++;
            yield return new WaitForSeconds(delay);
        }
    }

    private void OnEnemyDestinationReached(EnemyMovement enemy)
    {
        _currentLives--;
        _totalEnemies--;
        _currentEnemies.Remove(enemy);
        if (OnLivesLost != null)
            OnLivesLost.Invoke(_currentLives);
    }

    private void OnEnemyKilled(HealthManager enemy)
    {
        _totalEnemies--;
        _currentEnemies.Remove(enemy.GetComponent<EnemyMovement>());
    }

    private Vector3 GetNextSpawner()
    {
        if (_spawners.Length == 1)
            return _spawners[0].transform.position;
        var value = UnityEngine.Random.Range(0, _spawners.Length);

        return _spawners[value].transform.position;
    }
}
