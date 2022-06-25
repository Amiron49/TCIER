using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;

public class SpreadRageOnDeath : MonoBehaviour
{
    public WackyHoming HomingRageParticle;
    
    void Start()
    {
        var life = this.GetComponentStrict<Life>();
        life.OnDeath += (_, _) =>
        {
            var targets = GetPotentialTargets();
            var ownPosition = transform.position;

            foreach (var target in targets)
            {
                var homingParticle = Instantiate(HomingRageParticle,ownPosition, Quaternion.identity);
                homingParticle.to = target.gameObject;
            }
        };
    }

    IEnumerable<GameObject> GetPotentialTargets()
    {
        var targets = new Collider2D[5];
        Physics2D.OverlapCircleNonAlloc(transform.position, 10f, targets, Team.Enemy.FoundOnLayer());

        return targets.Where(x => x != null).Select(x => x.gameObject);
    }
}
