using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform _player;
    private Transform _ownTransform;

    public float ZOffset;
    // Start is called before the first frame update
    void Start()
    {
        _ownTransform = transform;
        _player = Game.Instance.State.Player.transform;
        ZOffset = _ownTransform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null)
        {
            if (Game.Instance.State.Player == null)
                return;
            
            _player = Game.Instance.State.Player.transform;
        }


        var playerPosition = _player.position;
        var newPosition = new Vector3(playerPosition.x, playerPosition.y, ZOffset);
        _ownTransform.position = newPosition;
    }
}
