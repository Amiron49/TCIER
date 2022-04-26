#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using UnityEngine;

public class State
{
	private readonly GameObject _parent;
	private Player _player;
	public ITimeFrame GameTime { get; } = new CustomTime("Game");

	public Player Player
	{
		get
		{
			if (_player != null)
				return _player;
			_player = _parent.scene.GetRootGameObjects().Single(x => x.CompareTag("Player")).GetComponent<Player>();
			return _player ;
		}
		set => _player = value;
	}

	public Inventory Inventory { get; }
	public int Money { get; private set; }
	public event MoneyChange? OnMoneyChange;
	public Dictionary<IEnemyConfiguration, EnemyStatistic> EnemyStatistics { get; }

	public State(GameObject parent, IEnumerable<IEnemyConfiguration> enemies)
	{
		_parent = parent;
		EnemyStatistics = enemies.Select(x => (Key: x, Value: new EnemyStatistic(x))).ToDictionary(x => x.Key, x => x.Value);
		Inventory = new Inventory();
	}

	public void AddMoney(int amount)
	{
		Money += amount;
		OnMoneyChange?.Invoke(this, amount);
	}

	public void RemoveMoney(int amount)
	{
		Money -= amount;
		OnMoneyChange?.Invoke(this, -amount);
	}

	public void TogglePause()
	{
		GameTime.Paused = !GameTime.Paused;
	}
}

public class CustomTime : ITimeFrame
{
	private bool _paused;
	public string Name { get; }

	public bool Paused
	{
		get => _paused;
		set
		{
			if (_paused != value)
				OnPauseChange?.Invoke(this, value);
			
			_paused = value;
		}
	}

	public event EventHandler<bool>? OnPauseChange;
	public float DeltaTime => Time.deltaTime * Scale;
	public float Scale { get; set; } = 1;


	public CustomTime(string name)
	{
		Name = name;
	}
}

public interface ITimeFrame
{
	public string Name { get; }
	public bool Paused { get; set; }
	public event EventHandler<bool> OnPauseChange;
	public float DeltaTime { get; }
	public float Scale { get; set; }
}