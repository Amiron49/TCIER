using Helpers;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public class PeashooterAI : MonoBehaviour
{
	public SeekTargetState.SeekPlayerConfig SeekTarget;
	public float PreferredDistance;
	[FormerlySerializedAs("PreferredDistanceTolerance")] public float PreferredDistanceToleranceFactor;
	public float PreferredDistanceRange;
	public SeekTargetState.SeekPlayerConfig FleeTarget;
	public OrbitStrafeAroundState.OrbitStrafeAroundStateConfig OrbitStrafeAround;
	[SerializeReference] public IStateMachine _stateMachine;

	private Transform _playerTransform;
	private Transform _selfTransform;

	// Start is called before the first frame update
	void Start()
	{
		_playerTransform = Game.Instance.State.Player.transform;
		_selfTransform = transform;
		var selfRigidBody = this.GetComponentStrict<Rigidbody2D>();

		var seekTargetState = new SeekTargetState(selfRigidBody, _playerTransform, SeekTarget, "Seek");
		var fleeTargetState = new SeekTargetState(selfRigidBody, _playerTransform, FleeTarget, "Flee");
		var orbitStrafeAroundState = new OrbitStrafeAroundState(selfRigidBody, _playerTransform, OrbitStrafeAround);
		var builder = new StateMachineBuilder()
			.AddState(seekTargetState, stateBuilder =>
			{
				stateBuilder.AddTriggerTransition("CloseEnough", orbitStrafeAroundState, () => DistanceToPlayer() < MaximumDistance());
			})
			.AddState(fleeTargetState, stateBuilder =>
			{
				stateBuilder.AddTriggerTransition("FarEnough", orbitStrafeAroundState, () => DistanceToPlayer() > MinimumDistance());
			})
			.AddState(orbitStrafeAroundState, stateBuilder =>
			{
				stateBuilder.AddTriggerTransition("TooClose", fleeTargetState, () => DistanceToPlayer() < MinimumDistance(PreferredDistanceToleranceFactor));
				stateBuilder.AddTriggerTransition("TooFar", seekTargetState, () => DistanceToPlayer() > MaximumDistance(PreferredDistanceToleranceFactor));
			});
		
		_stateMachine = builder.Build("default");
		_stateMachine.SetState(orbitStrafeAroundState.Key);
	}

	private float MaximumDistance(float tolerance = 1f)
	{
		return PreferredDistance + PreferredDistanceRange * tolerance;
	}

	private float MinimumDistance(float tolerance = 1f)
	{
		return PreferredDistance - PreferredDistanceRange * tolerance;
	}

	private float DistanceToPlayer()
	{
		return (_playerTransform.position - _selfTransform.position).sqrMagnitude;
	}

	// Update is called once per frame
	void Update()
	{
		_stateMachine.Update();
	}

	private void OnDrawGizmos()
	{
		_stateMachine.Gizmo();
	}

	private void FixedUpdate()
	{
		_stateMachine.FixedUpdate();
	}
}