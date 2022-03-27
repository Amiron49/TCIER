using System;

#nullable enable

namespace InternalLogic
{
	public class EnemyStatistic
	{
		private int _killCount;
		private int _buyCount;

		public EnemyStatistic(IEnemyConfiguration enemyDefinition)
		{
			Name = enemyDefinition.Name;
		}

		public string Name { get; }

		public int KillCount
		{
			get => _killCount;
			set
			{
				_killCount = value;
				OnKillCountChange?.Invoke(this, value);
			}
		}

		public int BuyCount
		{
			get => _buyCount;
			set
			{
				_buyCount = value;
				OnBuyCountChange?.Invoke(this, value);
			}
		}

		public event EventHandler<int>? OnKillCountChange;
		public event EventHandler<int>? OnBuyCountChange;
	}
}