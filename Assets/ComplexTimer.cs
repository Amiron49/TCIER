using System;

public class ComplexTimer<T>
{
	private bool _onCooldown;
	public Timer Timer { get; }
	public Timer Cooldown { get; }
	public bool Running => Cooldown.Running || Timer.Running;

	public event EventHandler<float>? OnTime;

	public ComplexTimer(T parent, float time, float cooldown, ITimeFrame? timeFrame = null)
	{
		Cooldown = new Timer(cooldown)
		{
			TimeFrame = timeFrame ?? Game.Instance.State.GameTime
		};
		Timer = new Timer(time)
		{
			TimeFrame = timeFrame ?? Game.Instance.State.GameTime
		};

		Cooldown.OnTime += (_, _) =>
		{
			Timer.ReStart();
			Cooldown.Stop();
			_onCooldown = false;
		};

		Timer.OnTime += (_, triggerTime) =>
		{
			Cooldown.ReStart();
			Timer.Stop();
			Timer.Reset();
			_onCooldown = true;
			OnTime?.Invoke(parent, triggerTime);
		};
	}

	public void Update()
	{
		Cooldown.Update();
		Timer.Update();
	}

	public void Stop()
	{
		Cooldown.Stop();
		Timer.Stop();
	}

	public void Start()
	{
		if (_onCooldown)
			Cooldown.Start();
		else
			Timer.Start();
	}

	public void Reset()
	{
		_onCooldown = false;
		Cooldown.Reset();
		Timer.Reset();
	}
}