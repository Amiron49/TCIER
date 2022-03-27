#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using UnityEngine;

public class State
{
	public ITimeFrame GameTime { get; } = new CustomTime("Game");
	public Player Player { get; set; }
	public Inventory Inventory { get; }
	public int Money { get; private set; }
	public event MoneyChange? OnMoneyChange;
	public Dictionary<IEnemyConfiguration, EnemyStatistic> EnemyStatistics { get; }

	public State(IEnumerable<IEnemyConfiguration> enemies)
	{
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
	public float Scale { get; set; }


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