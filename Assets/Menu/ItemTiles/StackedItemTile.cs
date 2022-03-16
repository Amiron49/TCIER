using System;
using Helpers;
using TMPro;
using UnityEngine;

namespace Menu.ItemTiles
{
    public class StackedItemTile : MonoBehaviour
    {
        public int CurrentAmount = 1;
        public TMP_Text AmountDisplay;
    
        // Start is called before the first frame update
        void Start()
        {
            var itemTile = this.GetComponentStrict<ItemTile>();
            if (itemTile.itemController == null)
            {
                throw new Exception("No ItemController defined");
            }
        
            itemTile.itemController = new StackedItemController(this, itemTile.itemController);

            var componentStrict = AmountDisplay.GetComponentStrict<Canvas>();
            componentStrict.overrideSorting = true;
            componentStrict.sortingLayerName = "ExtraUI";
        }

        public void Increment()
        {
            SetAmount(CurrentAmount + 1);
        }
    
        public void Decrement()
        {
            if (CurrentAmount == 0)
                Debug.LogError("Used item beyond usages. Fix your code");
        
            SetAmount(CurrentAmount - 1);
        }
    
        public void SetAmount(int amount)
        {
            CurrentAmount = amount;
            AmountDisplay.text = $"{amount}X";
        }
    
    }
}
