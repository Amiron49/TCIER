using System;
using UnityEngine;

public class DamageSource : MonoBehaviour, IDamageSource
{
    public int damage;

    public int Damage
    {
        get => damage;
        set => damage = value;
    }

    public Team @for;
    public Team For
    {
        get => @for;
        set => @for = value;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DishOutDamage(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DishOutDamage(other.gameObject);
    }

    private void DishOutDamage(GameObject other)
    {
        var damageTakeTargets = other.GetComponents<ITakeDamage>();

        foreach (var target in damageTakeTargets)
            target.TakeDamage(this);
    }
}
public interface IDamageSource
{
    public int Damage { get; set; }
    public Team For { get; set; }
}

[Flags]
public enum Team
{
    None = 0,
    Player = 1 << 0,
    Enemy = 1 << 1
}
