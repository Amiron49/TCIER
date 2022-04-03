using JetBrains.Annotations;
using StateMachine;
using UnityEngine;

public class SeekTargetState : StateBase
{
	[System.Serializable]
	public class SeekPlayerConfig
	{
		public float Speed;
	}

	private readonly Transform _target;
	private readonly SeekPlayerConfig _configuration;
	private readonly Rigidbody2D _self;

	public SeekTargetState(Rigidbody2D self, Transform target, SeekPlayerConfig configuration, [CanBeNull] string key = null) : base(key ?? nameof(SeekTargetState))
	{
		_self = self;
		_target = target;
		_configuration = configuration;
	}

	public override void FixedUpdate()
	{
		var currentPosition = _self.position;
		var targetPosition = CalculateNextPosition(currentPosition, _target.position);
		_self.MovePosition(targetPosition);
	}

	private Vector3 CalculateNextPosition(Vector3 currentPosition, Vector3 playerPosition)
	{
		var direction = (playerPosition - currentPosition).normalized;
		return currentPosition + direction * (_configuration.Speed * Time.fixedDeltaTime);
	}
}