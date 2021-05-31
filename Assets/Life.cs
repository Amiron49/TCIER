using UnityEngine;

public class Life : MonoBehaviour, ITakeDamage
{
    public bool invincible = false;
    public Team team;
    public int health = 100;
    public int CurrentHealth { get; set; } = 100;
    public GameObject onDeathDestroy;
    public event DamageReceive OnDamageTaken;
    public event HealthChange OnHealthChange;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(IDamageSource source)
    {
        if (invincible || source.For != team)
            return;

        var oldHealth = CurrentHealth;
        CurrentHealth -= source.Damage;
        OnDamageTaken?.Invoke(this, source);
        OnHealthChange?.Invoke(this, oldHealth, CurrentHealth);
        
        if (CurrentHealth <= 0)
            Destroy(onDeathDestroy);
    }
}

public delegate void DamageReceive(object sender, IDamageSource source);
public delegate void HealthChange(object sender, int from, int to);

public interface ITakeDamage
{
    void TakeDamage(IDamageSource source);
}
