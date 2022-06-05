using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class WaveManager : MonoBehaviour
{
	public GameObject[] SpawnPoints;
	public float CurrentDifficulty;
	[FormerlySerializedAs("TimeToNextWave")] public float SecondsToNextWave;

	[FormerlySerializedAs("WaveDelay")] public float WaveDelayInSeconds;
	public float DifficultyIncrease;
	public float LevelThreshold;

	private int _currentActiveInWave = 0;
	
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (Game.Instance.State.GameTime.Paused)
			return;

		var nextWaveIsClose = SecondsToNextWave < 5;
		var waveIsDepleted = _currentActiveInWave == 0;
		
		if (waveIsDepleted && !nextWaveIsClose)
		{
			SecondsToNextWave = 5;
		}
		
		SecondsToNextWave -= Game.Instance.State.GameTime.DeltaTime;

		if (SecondsToNextWave <= 0)
		{
			SpawnWave();
			SecondsToNextWave = WaveDelayInSeconds;
			CurrentDifficulty += DifficultyIncrease;
		}
	}

	public void SpawnWave()
	{
		var enemiesToSpawn = GenerateWaveContents();
		InstanceEnemies(enemiesToSpawn);
	}

	private void InstanceEnemies(Dictionary<EnemyDefinition, int> enemiesToSpawn)
	{
		var random = new Random();

		foreach (var (enemyDefinition, amount) in enemiesToSpawn)
		{
			for (var i = 0; i < amount; i++)
			{
				_currentActiveInWave++;
				var spawnOffset = new Vector3((float)(random.NextDouble() * 2), (float)(random.NextDouble() * 2));
				var spawnLocation = SpawnPoints[random.Next(0, SpawnPoints.Length - 1)];
				var enemyInstance = Instantiate(enemyDefinition.EnemyPrefab, spawnLocation.transform.position + spawnOffset, Quaternion.identity);
				enemyInstance.AddComponent<NotifyOnDeath>().OnDeath += (_, _) =>
				{
					Game.Instance.State.EnemyStatistics[enemyDefinition].KillCount += 1;
					_currentActiveInWave--;
				};
			}
		}
	}

	private Dictionary<EnemyDefinition, int> GenerateWaveContents()
	{
		var random = new Random();
		var computedMaxLevel = Mathf.RoundToInt(CurrentDifficulty / LevelThreshold);
		computedMaxLevel = Mathf.Max(1,computedMaxLevel);
		var targetDifficulty = CurrentDifficulty;
		var definedEnemiesOfLevel = Game.Instance.enemies.Where(x => x.Level <= computedMaxLevel).ToList();

		var enemiesToSpawn = new Dictionary<EnemyDefinition, int>();

		while (targetDifficulty > 0)
		{
			var enemyDef = definedEnemiesOfLevel[random.Next(0, definedEnemiesOfLevel.Count - 1)];
			targetDifficulty -= enemyDef.DifficultyScore;
			enemiesToSpawn.AddOrIncrement(enemyDef);
		}

		return enemiesToSpawn;
	}
}

public static class DictionaryExtensions
{
	public static void AddOrIncrement<T>(this IDictionary<T, int> dictionary, T key)
	{
		if (dictionary.ContainsKey(key))
		{
			dictionary[key]++;
		}
		else
		{
			dictionary[key] = 1;
		}
	}
}