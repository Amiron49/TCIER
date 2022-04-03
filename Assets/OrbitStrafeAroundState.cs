using StateMachine;
using UnityEngine;

public class OrbitStrafeAroundState : StateBase
{
	[System.Serializable]
	public class OrbitStrafeAroundStateConfig
	{
		public float Speed;
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

	public override void FixedUpdate()
	{
		var currentPosition = _self.position;
		var targetPosition = CalculateNextPosition(currentPosition, _around.position);
		_self.MovePosition(targetPosition);
	}

	private Vector3 CalculateNextPosition(Vector2 currentPosition, Vector2 targetPosition)
	{
		var difference = targetPosition - currentPosition;
		var direction = Vector2.Perpendicular(difference.normalized);

		return currentPosition + direction * (_configuration.Speed * Time.fixedDeltaTime);
	}
}