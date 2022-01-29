using System;
using InternalLogic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
	public class BuyTile : MonoBehaviour, IPointerEnterHandler
	{
		public EnemyDefinition enemyDefinition;
		public TMP_Text priceDisplay;
		public TMP_Text killCountDisplay;
		public TMP_Text nameDisplay;
		public GameObject graphicParent;

		public event EventHandler<EnemyDefinition> OnHover;
		
		// Start is called before the first frame update
		void Start()
		{
			nameDisplay.text = enemyDefinition.Name;
			var enemyHusk = Instantiate(enemyDefinition.MenuHuskPrefab, graphicParent.transform);
			enemyHusk.AddComponent<ScaleWithParent>();
			enemyHusk.layer = LayerMask.NameToLayer("UI");
			enemyHusk.transform.position -= Vector3.forward * 2;
		}

		public void Buy()
		{
			var price = CalculatePrice();

			if (price > Game.Instance.State.Money)
				return;

			Game.Instance.State.RemoveMoney(price);

			var statistics = Game.Instance.State.EnemyStatistics[enemyDefinition];
			statistics.KillCount /= 2;
			statistics.BuyCount += 1;
			Game.Instance.State.Inventory.Add(new EnemyAsEquipment
			{
				BodyEquipment = enemyDefinition.AsBodyEquipment,
				GunEquipment = enemyDefinition.AsGunEquipment,
				AsBulletEquipment = enemyDefinition.AsBulletEquipment
			});
		}

		private void Awake()
		{
			RefreshState();
		}

		public void RefreshState()
		{
			if (enemyDefinition == null)
				return;

			var statistics = Game.Instance.State.EnemyStatistics[enemyDefinition];
			var price = CalculatePrice();

			priceDisplay.text = price.ToString();
			killCountDisplay.text = statistics.KillCount.ToString();
		}

		private int CalculatePrice()
		{
			var statistics = Game.Instance.State.EnemyStatistics[enemyDefinition];
			var price = enemyDefinition.BasePrice - statistics.KillCount;

			return price;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			OnHover?.Invoke(this, enemyDefinition);
		}
	}
}