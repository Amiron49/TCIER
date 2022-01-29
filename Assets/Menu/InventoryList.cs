using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using UnityEngine;

namespace Menu
{
    public class InventoryList : MonoBehaviour
    {
        private List<StackedItemTile> currentTiles = new();
        public StackedItemTile tilePrefab;
        public ItemTypeFilter filter = ItemTypeFilter.Whatever;

        private void Awake()
        {
            Game.Instance.State.Inventory.UnusedChange += (_, _) => Refresh();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        void Refresh()
        {
            var fullList = Game.Instance.State.Inventory.Unused.Where(CreateSearch()).GroupBy(x => x.InventoryHusk).ToList();

            foreach (var currentTile in currentTiles)
                Destroy(currentTile.gameObject);

            currentTiles.Clear();
            
            foreach (var group in fullList)
            {
                var tileStack = Instantiate(tilePrefab, gameObject.transform);
                var tile = tileStack.GetComponent<ItemTile>();
                tileStack.SetAmount(group.Count());
                tile.itemHusk = group.First().InventoryHusk;
                tile.itemController = new BodyEquipController(group.First());
                currentTiles.Add(tileStack);
            }
        }

        private Func<IEnemyAsEquipment, bool> CreateSearch()
        {
            return filter switch
            {
                ItemTypeFilter.Whatever => _ => true,
                ItemTypeFilter.Body => x => x.BodyEquipment != null,
                ItemTypeFilter.Gun => x => x.GunEquipment != null,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

    public enum ItemTypeFilter
    {
        Whatever,
        Body,
        Gun
    }

    public class BodyEquipController: IItemController
    {
        private readonly IEnemyAsEquipment _equipment;

        public BodyEquipController(IEnemyAsEquipment equipment)
        {
            _equipment = equipment;
        }
        
        public bool Use(ItemTile tile)
        {
            var error = Game.Instance.State.Inventory.Body.Equip(_equipment.BodyEquipment);

            if (error != null)
            {
                var rectTransform = tile.GetComponent<RectTransform>();
                
                tile.gameObject.BubbleTextOnMe(error,  new Vector3(0, rectTransform.sizeDelta.y / 2, 0) );
            }
            
            return error == null;
        }
    }
}

