using System;

public class Timer
{
	public Timer(float time)
	{
		Time = time;
	}

	public float Time { get; }
	public float Progress => ElapsedTime / Time;
	public float ElapsedTime { get; private set; } = 0;
	public bool Running { get; private set; } = false;
	public ITimeFrame TimeFrame { get; set; } = Game.Instance.State.GameTime;

	public event EventHandler<float>? OnTime;

	public void Update()
	{
		if (TimeFrame.Paused || !Running)
			return;

		if (ElapsedTime < Time)
		{
			ElapsedTime += TimeFrame.DeltaTime;
		}
		else
		{
			Reset();
			OnTime?.Invoke(this, Time);
		}
	}

	public void Reset()
	{
		ElapsedTime = 0;
	}

	public void Stop()
	{
		Running = false;
	}

	public void Start()
	{
		Running = true;
	}

	public void ReStart()
	{
		Running = true;
		ElapsedTime = 0;
	}
}