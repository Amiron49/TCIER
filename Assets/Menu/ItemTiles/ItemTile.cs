using Helpers;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Menu.ItemTiles
{
	public class ItemTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		[FormerlySerializedAs("initialItemHusk")] [FormerlySerializedAs("itemHusk")] [FormerlySerializedAs("enemyDefinition")]
		public GameObject InitialItemHusk;

		[FormerlySerializedAs("graphicParent")] public GameObject GraphicParent;
		[CanBeNull] public IItemController itemController;

		private bool _hovering;
		private GameObject _currentHusk;
		
		// Start is called before the first frame update
		void Start()
		{
			if (InitialItemHusk != null)
			{
				SetItemHusk(InitialItemHusk);
			}
		}

		public void SetItemHusk(GameObject huskPrefab)
		{
			if (_currentHusk != null)
			{
				Destroy(_currentHusk);
			}
			
			_currentHusk = Instantiate(huskPrefab, GraphicParent.transform);
			_currentHusk.AddComponent<ScaleWithParent>();
			_currentHusk.SetLayerRecursively(LayerMask.NameToLayer("UI"));
			_currentHusk.transform.position -= Vector3.forward * 2;
		}

		public void OnClick()
		{
			itemController?.Use(this);
		}
		
		public void OnHoverStart()
		{
			_hovering = true;
			itemController?.HoverStart();
		}
		
		public void OnHoverEnd()
		{
			_hovering = false;
			itemController?.HoverEnd();
		}

		// Update is called once per frame
		void Update()
		{
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			OnHoverStart();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			OnHoverEnd();
		}

		private void OnDestroy()
		{
			if (_hovering)
			{
				OnHoverEnd();
			}
		}
	}
}