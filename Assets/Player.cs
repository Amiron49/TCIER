#nullable enable
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
	private ControlManager _controlManager = null!;
	private IStateMachine _movementStateMachine = null!;
	public float speed = 4;
	public float dodgeSpeed = 30;
	public float dodgeDuration = 0.5f;

	// Start is called before the first frame update
	void Start()
	{
		_controlManager = new ControlManager();

		var movementStateMachineBuilder = new StateMachineBuilder();
		var normalMovementState = new NormalMovementState("Normal", this, _controlManager);
		var rollingDodge = new RollingDodgeState("RollingDodge", this, _controlManager);
		
		movementStateMachineBuilder.AddState(normalMovementState);
		movementStateMachineBuilder.AddState(rollingDodge);
		
		movementStateMachineBuilder.AddTriggerTransition("Press Dodge", normalMovementState.Key, rollingDodge.Key, () => _controlManager.Dodge);
		movementStateMachineBuilder.AddEventTransition("", rollingDodge.Key, normalMovementState.Key);

		_movementStateMachine = movementStateMachineBuilder.Build("Movement");
		_movementStateMachine.SetState(normalMovementState.Key);
	}

	// Update is called once per frame
	void Update()
	{
		_controlManager.Update();
		_movementStateMachine.Update();
	}

	private class NormalMovementState : StateBase
	{
		private readonly Player _player;
		private readonly ControlManager _controlManager;
		private readonly Transform _transform; 
		
		public NormalMovementState(string key, Player player, ControlManager controlManager) : base(key)
		{
			_player = player;
			_controlManager = controlManager;
			_transform = player.GetComponent<Transform>();
		}

		public override void Update()
		{
			var currentPosition = _transform.position;
			var targetPosition = CalculateNextPosition(currentPosition);
		
			_transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime);
		}
		
		private Vector3 CalculateNextPosition(Vector3 currentPosition)
		{
			return currentPosition + _controlManager.MoveDirection * _player.speed;
		}
	}
	
	private class RollingDodgeState : TimedStateBase
	{
		private readonly Player _player;
		private readonly ControlManager _controlManager;
		private readonly Transform _transform; 
		private Vector3 _dodgeDirection; 
		
		public RollingDodgeState(string key, Player player, ControlManager controlManager) : base(key, player.dodgeDuration)
		{
			_player = player;
			_controlManager = controlManager;
			_transform = player.GetComponent<Transform>();
		}

		protected override void OnEnterInternal(object? @event)
		{
			_dodgeDirection = _controlManager.MoveDirection.normalized;
		}

		protected override void UpdateInternal()
		{
			var currentPosition = _transform.position;
			var targetPosition = CalculateNextPosition(currentPosition);
			_transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime);
		}
		
		private Vector3 CalculateNextPosition(Vector3 start)
		{
			return start + _dodgeDirection * _player.dodgeSpeed;
		}
	}
}



public class ControlManager
{
	public Vector3 MoveDirection { get; private set; }
	public bool Dodge { get; private set; }

	public void Update()
	{
		var vertical = Input.GetAxis("Vertical");
		var horizontal = Input.GetAxis("Horizontal");

		MoveDirection = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1);
		MoveDirection.Normalize();
		Dodge = Input.GetKeyDown(KeyCode.Space);
	}
}
