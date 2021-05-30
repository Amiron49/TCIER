using UnityEngine;

public class Life : MonoBehaviour, ITakeDamage
{
    public bool invincible = false;
    public int health = 100;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(IDamageSource source)
    {
        if (invincible)
            return;
        
        health -= source.Damage;

        if (health <= 0)
            Destroy(gameObject);
    }
}

public interface ITakeDamage
{
    void TakeDamage(IDamageSource source);
}
