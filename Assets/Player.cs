#nullable enable
using System;
using Helpers;
using StateMachine;
using UnityEngine;

public class Player : MonoBehaviour
{
	private IStateMachine _movementStateMachine = null!;
	public float speed = 4;
	public float dodgeSpeed = 30;
	public float dodgeDuration = 0.5f;

	// Start is called before the first frame update
	void Start()
	{
		var movementStateMachineBuilder = new StateMachineBuilder();
		var normalMovementState = new NormalMovementState("Normal", this, Game.ControlManager);
		var rollingDodge = new RollingDodgeState("RollingDodge", this, Game.ControlManager);
		
		movementStateMachineBuilder.AddState(normalMovementState);
		movementStateMachineBuilder.AddState(rollingDodge);
		
		movementStateMachineBuilder.AddTriggerTransition("Press Dodge", normalMovementState.Key, rollingDodge.Key, () => Game.ControlManager.Dodge);
		movementStateMachineBuilder.AddEventTransition("", rollingDodge.Key, normalMovementState.Key);

		_movementStateMachine = movementStateMachineBuilder.Build("Movement");
		_movementStateMachine.SetState(normalMovementState.Key);
	}

	// Update is called once per frame
	void Update()
	{
		_movementStateMachine.Update();
	}

	private void FixedUpdate()
	{
		_movementStateMachine.FixedUpdate();
	}

	private class NormalMovementState : StateBase
	{
		private readonly Player _player;
		private readonly ControlManager _controlManager;
		private readonly Rigidbody2D _rigidbody; 
		
		public NormalMovementState(string key, Player player, ControlManager controlManager) : base(key)
		{
			_player = player;
			_controlManager = controlManager;
			_rigidbody = player.GetComponentInChildrenStrict<Rigidbody2D>();
		}	

		public override void FixedUpdate()
		{
			var currentPosition = _rigidbody.position;
			var targetPosition = CalculateNextPosition(currentPosition);

			_rigidbody.MovePosition(targetPosition);
		}

		private Vector3 CalculateNextPosition(Vector3 currentPosition)
		{
			return currentPosition + _controlManager.MoveDirection * (_player.speed * Time.fixedDeltaTime);
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
			_transform = player.GetComponentStrict<Transform>();
		}

		protected override void OnEnterInternal(object? @event)
		{
			_dodgeDirection = _controlManager.MoveDirection.normalized;
			_player.gameObject.layer = LayerMask.NameToLayer("Dodge");
		}

		protected override void UpdateInternal()
		{
			var currentPosition = _transform.position;
			var targetPosition = CalculateNextPosition(currentPosition);
			_transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime);
		}

		public override void OnLeave()
		{
			_player.gameObject.layer = LayerMask.NameToLayer("Player");
			base.OnLeave();
		}

		private Vector3 CalculateNextPosition(Vector3 start)
		{
			return start + _dodgeDirection * _player.dodgeSpeed;
		}
	}
}



public static class Game
{
	public static readonly ControlManager ControlManager = new ControlManager();
}


public class ControlManager
{
	public Vector3 MoveDirection { get; private set; }
	public Vector3 MousePosition { get; private set; }
	public Vector3 MouseWorldPosition { get; private set; }
	public bool Dodge { get; private set; }
	public bool Shoot { get; private set; }

	public void Update()
	{
		var vertical = Input.GetAxis("Vertical");
		var horizontal = Input.GetAxis("Horizontal");

		MoveDirection = Vector2.ClampMagnitude(new Vector2(horizontal, vertical), 1);
		MoveDirection.Normalize();
		Dodge = Input.GetButtonDown("Jump");
		Shoot = Input.GetButton("Fire1");
		MousePosition = Input.mousePosition;
		MouseWorldPosition = Camera.main.ScreenToWorldPoint(MousePosition).NoZ(Vector3Extensions.StandardZ);
	}
}
