using InternalLogic;
using Menu;
using UnityEngine;

namespace UI
{
	public class SpawnBuyTiles : MonoBehaviour
	{
		public BuyTile buyTilePrefab;
    
		// Start is called before the first frame update
		void Start()
		{
			foreach (var enemyDefinition in Game.Instance.enemies)
				AddTile(enemyDefinition);
		}

		void AddTile(EnemyDefinition definition)
		{
			var tile = Instantiate(buyTilePrefab, transform);
			tile.enemyDefinition = definition;
			tile.RefreshState();
		}
    
		// Update is called once per frame
		void Update()
		{
        
		}
	}
}
