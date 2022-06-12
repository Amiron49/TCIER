using Helpers;
using UnityEngine;

public class LookTowardsBulletDirection : MonoBehaviour
{
    private Transform _transform;
    private PhysicalDirectionalBullet _bullet;

    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _bullet = this.GetComponentStrict<PhysicalDirectionalBullet>();
    }

    // Update is called once per frame
    void Update()
    {
        _transform.rotation = Quaternion.LookRotation(Vector3.forward, _bullet.direction); 
    }
}
