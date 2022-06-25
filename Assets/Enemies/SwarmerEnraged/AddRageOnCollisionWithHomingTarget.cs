using Helpers;
using UnityEngine;

public class AddRageOnCollisionWithHomingTarget : MonoBehaviour
{
    public GameObject RageAura;
    public float SpeedMultiplierMultiplier = 1.3f;
    private WackyHoming _wackyHoming;

    private void Start()
    {
        _wackyHoming = GetComponent<WackyHoming>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        HandleCollision(other);
    }

    private void HandleCollision(Collider2D other)
    {
        if (other.gameObject != _wackyHoming.to)
            return;


        Instantiate(RageAura, other.gameObject.transform);
        var speedMultiplier = other.gameObject.GetComponentStrict<SpeedMultiplier>();
        speedMultiplier.Multiplier *= SpeedMultiplierMultiplier;
        Destroy(gameObject);
    }
}