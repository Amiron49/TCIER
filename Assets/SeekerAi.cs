using StateMachine;
using UnityEngine;

public class SeekerAi : MonoBehaviour
{
	public SeekTargetState.SeekPlayerConfig SeekTarget;

	public GameObject DeathExplosion;
	private IStateMachine _seekMachine;
	private Transform _transform;
	private Rigidbody2D _selfRigidbody;

	// Start is called before the first frame update
	void Start()
	{
		_transform = transform;
		_selfRigidbody = GetComponent<Rigidbody2D>();
		var playerTransform = Game.Instance.State.Player.transform;

		_seekMachine = new StateMachineBuilder().AddState(new SeekTargetState(_selfRigidbody, playerTransform, SeekTarget)).Build("default");
		_seekMachine.SetState(nameof(SeekTargetState));

		Game.Instance.State.GameTime.OnPauseChange += GameTimeOnOnPauseChange;
	}

	private void GameTimeOnOnPauseChange(object sender, bool e)
	{
		_selfRigidbody.isKinematic = e;
		_seekMachine.SetPause(e);
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		_seekMachine.FixedUpdate();
	}

	private void OnDestroy()
	{
		Instantiate(DeathExplosion, _transform.position, _transform.rotation);
		Game.Instance.State.GameTime.OnPauseChange -= GameTimeOnOnPauseChange;
	}
}