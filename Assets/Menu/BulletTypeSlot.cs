using Helpers;
using InternalLogic;
using Menu;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletTypeSlot : MonoBehaviour
{
    private Gun _slotFor;
    [FormerlySerializedAs("BulletSelection")] public Popup bulletSelection;
    public GunIndexProvider gunIndexProvider;
    
    // Start is called before the first frame update
    void Start()
    {
        _slotFor = Game.Instance.State.Inventory.Body.Guns[gunIndexProvider.gunIndex];
        
        var itemTile = this.GetComponentStrict<ItemTile>();
        var itemController = new ItemController();
        itemTile.itemController = itemController;

        itemController.OnUseSuccess += (_, _) => bulletSelection.Show();


		_slotFor.OnBulletChange += (_, _) =>
		{
			//TODO actually set proper husks for the bullets
			//itemTile.SetItemHusk(config.BulletPrefab);
		};

	}
    
}
