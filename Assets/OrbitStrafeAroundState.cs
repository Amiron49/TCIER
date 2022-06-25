using StateMachine;
using UnityEngine;

public class OrbitStrafeAroundState : StateBase
{
	[System.Serializable]
	public class OrbitStrafeAroundStateConfig
	{
		public float BaseSpeed;
        public float Speed => BaseSpeed * SpeedMultiplier.Multiplier;
        public SpeedMultiplier SpeedMultiplier;
	}

	private readonly Transform _around;
	private readonly OrbitStrafeAroundStateConfig _configuration;
	private readonly Rigidbody2D _self;

	public OrbitStrafeAroundState(Rigidbody2D self, Transform around, OrbitStrafeAroundStateConfig configuration) : base(nameof(OrbitStrafeAroundState))
	{
		_self = self;
		_around = around;
		_configuration = configuration;
	}

    public override void Update()
    {
        var currentPosition = _self.position;
        var difference = (Vector2)_around.position - currentPosition;
        var direction = Vector2.Perpendicular(difference.normalized);

        _self.velocity = direction * _configuration.Speed;
    }
}