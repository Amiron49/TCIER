using StateMachine;
using UnityEngine;

public class SeekPlayer : MonoBehaviour
{
    public SeekTargetState.SeekPlayerConfig SeekTarget;
    
    public GameObject DeathExplosion;
    private IStateMachine _seekMachine;
    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        var self = GetComponent<Rigidbody2D>();
        var playerTransform = Game.Instance.State.Player.transform;

        _seekMachine = new StateMachineBuilder().AddState(new SeekTargetState(self, playerTransform, SeekTarget)).Build("default");
        _seekMachine.SetState(nameof(SeekTargetState));
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _seekMachine.FixedUpdate();
    }

    private void OnDestroy()
    {
        Instantiate(DeathExplosion, _transform.position, _transform.rotation);
    }
}
