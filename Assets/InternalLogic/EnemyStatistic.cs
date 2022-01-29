namespace InternalLogic
{
	public class EnemyStatistic
	{
		public EnemyStatistic(IEnemyConfiguration enemyDefinition)
		{
			Name = enemyDefinition.Name;
		}

		public string Name { get; }
		public int KillCount { get; set; }
		public int BuyCount { get; set; }
	}
}