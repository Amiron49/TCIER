#nullable enable
using System;
using Helpers;
using StateMachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	private IStateMachine _movementStateMachine = null!;
	public float speed = 4;
	public float dodgeSpeed = 30;
	public float dodgeDuration = 0.5f;

	// Start is called before the first frame update
	void Start()
	{
		Game.Instance.State.Player = this;

		var movementStateMachineBuilder = new StateMachineBuilder();
		var normalMovementState = new NormalMovementState("Normal", this, Game.Instance.Controls);
		var rollingDodge = new RollingDodgeState("RollingDodge", this, Game.Instance.Controls);
		
		movementStateMachineBuilder.AddState(normalMovementState);
		movementStateMachineBuilder.AddState(rollingDodge);
		
		movementStateMachineBuilder.AddTriggerTransition("Press Dodge", normalMovementState.Key, rollingDodge.Key, () => Game.Instance.Controls.Player.Dodge.WasPressedThisFrame());
		movementStateMachineBuilder.AddEventTransition("", rollingDodge.Key, normalMovementState.Key);

		_movementStateMachine = movementStateMachineBuilder.Build("Movement");
		_movementStateMachine.SetState(normalMovementState.Key);
		PauseOnGamePause();
	}

	private void PauseOnGamePause()
	{
		Game.Instance.State.GameTime.OnPauseChange += (_, isPaused) =>
		{
			if (isPaused)
				_movementStateMachine.Pause();
			else
				_movementStateMachine.Unpause();
		};
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

    private void OnDestroy()
    {
        SceneManager.LoadScene(2);
    }

    private class NormalMovementState : StateBase
	{
		private readonly Player _player;
		private readonly TCIERControls _controls;
		private readonly Rigidbody2D _rigidbody; 
		
		public NormalMovementState(string key, Player player, TCIERControls controls) : base(key)
		{
			_player = player;
			_controls = controls;
			_rigidbody = player.GetComponentInChildrenStrict<Rigidbody2D>();
		}	

		public override void FixedUpdate()
		{
			var currentPosition = _rigidbody.position;
			var targetPosition = CalculateNextPosition(currentPosition);

			_rigidbody.MovePosition(targetPosition);
		}

		private Vector2 CalculateNextPosition(Vector2 currentPosition)
		{
			return currentPosition + _controls.Player.Move.ReadValue<Vector2>().normalized * (_player.speed * Time.deltaTime);
		}
	}
	
	private class RollingDodgeState : TimedStateBase
	{
		private readonly Player _player;
		private readonly TCIERControls _controls;
		private readonly Transform _transform; 
		private Vector2 _dodgeDirection; 
		
		public RollingDodgeState(string key, Player player, TCIERControls controls) : base(key, player.dodgeDuration)
		{
			_player = player;
			_controls = controls;
			_transform = player.GetComponentStrict<Transform>();
        }

		protected override void OnEnterInternal(object? @event)
		{
			_dodgeDirection = _controls.Player.Move.ReadValue<Vector2>().normalized;
			_player.gameObject.layer = LayerMask.NameToLayer("Dodge");
		}

		protected override void UpdateInternal()
		{
			var currentPosition = _transform.position;
			var targetPosition = CalculateNextPosition(currentPosition);
            var rigidBody = _player.GetComponent<Rigidbody2D>();
            var transformPosition = Vector2.Lerp(currentPosition, targetPosition, Game.Instance.State.GameTime.DeltaTime);
            rigidBody.MovePosition(transformPosition);
		}

		public override void OnLeave()
		{
			_player.gameObject.layer = LayerMask.NameToLayer("Player");
			base.OnLeave();
		}

		private Vector2 CalculateNextPosition(Vector2 start)
		{
			return start + _dodgeDirection * (_player.dodgeSpeed);
		}
	}
}

