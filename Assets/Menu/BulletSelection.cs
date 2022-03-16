using System;
using System.Collections.Generic;
using System.Linq;
using InternalLogic;
using Menu;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletSelection : MonoBehaviour
{
	private Gun SelectionFor { get; set; }
	public StatsDisplayUI statsDisplayUI;
	public GunIndexProvider gunIndexProvider;
	public ItemTile itemTilePrefab;

	private readonly List<IEnemyConfiguration> _alreadyRenderedConfigurations = new();

	// Start is called before the first frame update
	void Start()
	{
		SelectionFor = Game.Instance.State.Inventory.Body.Guns[gunIndexProvider.gunIndex];
		Refresh();
	}

	private void OnEnable()
	{
		Refresh();
	}

	private void Refresh()
	{
		var everBoughtEnemies = Game.Instance.State.EnemyStatistics.Where(x => x.Key.AsBulletEquipment != null && x.Value.BuyCount > 0).ToArray();

		var difference = everBoughtEnemies.Length - _alreadyRenderedConfigurations.Count;
		if (difference <= 0)
			return;
		
		var newOnes = everBoughtEnemies[..difference];

		foreach (var (enemyConfiguration, _) in newOnes)
		{
			AddAsTile(enemyConfiguration);
			_alreadyRenderedConfigurations.Add(enemyConfiguration);
		}
	}

	private void AddAsTile(IEnemyConfiguration enemyConfiguration)
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
			statsDisplayUI.DisplayPreview(SelectionFor.Properties, preview, SelectionFor.IsPositiveChangeMap);
		};
		controller.OnHoverEnd += (_, _) =>
		{
			statsDisplayUI.DisplayNormal(SelectionFor.Properties);
		};
		
		var itemTile = Instantiate(itemTilePrefab, transform);
		itemTile.itemController = controller;
		itemTile.SetItemHusk(enemyConfiguration.MenuHuskPrefab);
	}

	// Update is called once per frame
	void Update()
	{
	}
}