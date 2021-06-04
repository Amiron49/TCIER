public class State
{
	public Player Player { get; set; }
	public int Money { get; private set; } = 0;
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
	
	public event MoneyChange? OnMoneyChange;
	
	public int SwarmerKillCount { get; set; } = 0;
}