using UnityEngine;

public class PhysicalBullet : MonoBehaviour, IBullet
{
    public Vector3 direction = Vector3.up;
    public Vector3 Direction
    {
        get => direction;
        set => direction = value;
    }

    public float velocity = 10f;
    private Transform _transform;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        var currentPosition = _transform.position;
        var targetPosition = CalculateNextPosition(currentPosition);
        _transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime);
    }
    
    private Vector3 CalculateNextPosition(Vector3 start)
    {
        return start + direction * velocity;
    }

}

public interface IBullet
{
    public Vector3 Direction { get; set; }
}
