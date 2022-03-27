using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using Menu.ItemTiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu
{
	public class BulletSelection : MonoBehaviour
	{
		private Gun SelectionFor { get; set; }

		[FormerlySerializedAs("statsDisplayUI")]
		public PropertiesDisplay propertiesDisplay;

		public GunIndexProvider gunIndexProvider;
		public ItemTile itemTilePrefab;

		private readonly List<IEnemyConfiguration> _alreadyRenderedConfigurations = new();

		private ChildrenSync<Component, IEnemyConfiguration> _childrenSync;

		// Start is called before the first frame update
		void Start()
		{
			SelectionFor = Game.Instance.State.Inventory.Body.Guns[gunIndexProvider.gunIndex];
			_childrenSync = new ChildrenSync<Component, IEnemyConfiguration>(CreateTile);
			Refresh();
		}

		private void OnEnable()
		{
			if (_childrenSync == null)
				return;
			
			Refresh();
		}

		private void Refresh()
		{
			var everBoughtEnemies = Game.Instance.State.EnemyStatistics
				.Where(x => x.Key.AsBulletEquipment != null && x.Value.BuyCount > 0)
				.Select(x => x.Key).ToList();

			_childrenSync.Update(everBoughtEnemies);
		}

		private Component CreateTile(IEnemyConfiguration enemyConfiguration)
		{
			var controller = new ItemController();

			if (enemyConfiguration.AsBulletEquipment == null)
			{
				throw new Exception($"No defined bulletEquip on {enemyConfiguration.Name}");
			}

			controller.OnUseSuccess += (_, _) => SelectionFor.Equip(enemyConfiguration.AsBulletEquipment!);
			controller.OnHoverStart += (_, _) =>
			{
				var preview = SelectionFor.PreviewEquip(enemyConfiguration.AsBulletEquipment!).ToStringDictionary();
				propertiesDisplay.DisplayPreview(SelectionFor.Properties, preview, SelectionFor.IsPositiveChangeMap);
			};
			controller.OnHoverEnd += (_, _) => { propertiesDisplay.DisplayNormal(SelectionFor.Properties); };

			var itemTile = Instantiate(itemTilePrefab, transform);
			itemTile.itemController = controller;
			itemTile.SetItemHusk(enemyConfiguration.MenuHuskPrefab);

			return itemTile;
		}
	}
}