using UnityEngine;

public class DamageSource : MonoBehaviour, IDamageSource
{
    public int damage;
    public int Damage
    {
        get => damage;
        set => damage = value;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageTakeTargets = other.GetComponents<ITakeDamage>();

        foreach (var target in damageTakeTargets)
            target.TakeDamage(this);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public interface IDamageSource
{
    public int Damage { get; set; }
}
