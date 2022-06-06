using System;
using UnityEngine;

public class Life : MonoBehaviour, ITakeDamage
{
    public bool invincible = false;
    public Team team;
    public float health = 100;
    public float CurrentHealth { get; set; } = 100;
    public GameObject onDeathDestroy;
    public event DamageReceive OnDamageTaken;
    public event HealthChange OnHealthChange;
    public event EventHandler OnDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        if (onDeathDestroy == null)
            onDeathDestroy = gameObject;
        
        CurrentHealth = health;

        OnDeath += (_, _) =>
        {
            Destroy(onDeathDestroy);
        };
    }

    private bool _alreadyDied = false;
    
    public void TakeDamage(IDamageSource source)
    {
        if (invincible || source.For != team)
            return;

        var oldHealth = CurrentHealth;
        CurrentHealth -= source.Damage;
        OnDamageTaken?.Invoke(this, source);
        OnHealthChange?.Invoke(this, oldHealth, CurrentHealth);
        
        if (CurrentHealth <= 0 && !_alreadyDied)
        {
            _alreadyDied = true;
            OnDeath?.Invoke(this, EventArgs.Empty);
        }
    }
}

public delegate void DamageReceive(object sender, IDamageSource source);
public delegate void HealthChange(object sender, float from, float to);

public interface ITakeDamage
{
    void TakeDamage(IDamageSource source);
}
