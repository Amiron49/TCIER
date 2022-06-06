using Helpers;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class OnDeathSpawnEnemy : MonoBehaviour
{
    public Enemy EnemyPrefab;
    public int Amount;
    public float SpawnPositionVariance = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        var life = this.GetComponentStrict<Life>();
        
        life.OnDeath += (_, _) =>
        {
            var random = Random.CreateFromIndex((uint)gameObject.GetHashCode());
            var position = transform.position;
            for (int i = 0; i < Amount; i++)
            {
                var nextFloat2 = random.NextFloat2(SpawnPositionVariance);
                var varianceVector = new Vector2(nextFloat2.x, nextFloat2.y);
                Instantiate(EnemyPrefab, position + (Vector3)varianceVector, Quaternion.identity);
            }
        };
    }
}
