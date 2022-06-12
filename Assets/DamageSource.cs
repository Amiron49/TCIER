using System;
using UnityEngine;

public class DamageSource : MonoBehaviour, IDamageSource
{
    public float damage;

    public float Damage
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
    public float Damage { get; set; }
    public Team For { get; set; }
}

[Flags]
public enum Team
{
    None = 0,
    Player = 1 << 0,
    Enemy = 1 << 1
}

public static class TeamHelper {
    public static int DamageToLayer(this Team team)
    {
        return team switch
        {
            Team.Enemy => LayerMask.NameToLayer("Player Bullets"),
            Team.Player => LayerMask.NameToLayer("Enemy Bullets"),
            Team.Player | Team.Enemy => throw new NotImplementedException("Didn't make a combination layer yet for that."),
            Team.None => throw new InvalidOperationException("None case has no layer yet"),
            _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
        };
    }
    
    public static int FoundOnLayer(this Team team)
    {
        return team switch
        {
            Team.Enemy => LayerMask.GetMask("Enemy Swarmers", "Enemy Others"),
            Team.Player => LayerMask.GetMask("Player"),
            Team.Player | Team.Enemy => LayerMask.GetMask("Enemy Swarmers", "Enemy Others", "Player"),
            Team.None => throw new InvalidOperationException("None case has no layer yet"),
            _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
        };
    }
}
