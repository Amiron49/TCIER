using System;
using InternalLogic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Menu
{
	public class BuyTile : MonoBehaviour, IPointerEnterHandler
	{
		[FormerlySerializedAs("enemyDefinition")] public EnemyDefinition EnemyDefinition;
		[FormerlySerializedAs("priceDisplay")] public TMP_Text PriceDisplay;
		[FormerlySerializedAs("killCountDisplay")] public TMP_Text KillCountDisplay;
		[FormerlySerializedAs("nameDisplay")] public TMP_Text NameDisplay;
		[FormerlySerializedAs("graphicParent")] public GameObject GraphicParent;

		public event EventHandler<EnemyDefinition> OnHover;
		
		// Start is called before the first frame update
		void Start()
		{
			NameDisplay.text = EnemyDefinition.Name;
			var enemyHusk = Instantiate(EnemyDefinition.MenuHuskPrefab, GraphicParent.transform);
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

			var statistics = Game.Instance.State.EnemyStatistics[EnemyDefinition];
			statistics.KillCount /= 2;
			statistics.BuyCount += 1;
			Game.Instance.State.Inventory.Add(new EnemyAsEquipment
			{
				BodyEquipment = EnemyDefinition.AsBodyEquipment,
				GunEquipment = EnemyDefinition.AsGunEquipment,
				AsBulletEquipment = EnemyDefinition.AsBulletEquipment
			});
		}

		private void Awake()
		{
			RefreshState();
		}

		public void RefreshState()
		{
			if (EnemyDefinition == null)
				return;

			var statistics = Game.Instance.State.EnemyStatistics[EnemyDefinition];
			var price = CalculatePrice();

			PriceDisplay.text = price.ToString();
			KillCountDisplay.text = statistics.KillCount.ToString();
		}

		private int CalculatePrice()
		{
			var statistics = Game.Instance.State.EnemyStatistics[EnemyDefinition];
			var price = EnemyDefinition.BasePrice - statistics.KillCount;

			return price;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			OnHover?.Invoke(this, EnemyDefinition);
		}
	}
}