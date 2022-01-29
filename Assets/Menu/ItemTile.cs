using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu
{
	public class ItemTile : MonoBehaviour
	{
		[FormerlySerializedAs("enemyDefinition")]
		public GameObject itemHusk;

		public GameObject graphicParent;
		public IItemController itemController;

		// Start is called before the first frame update
		void Start()
		{
			var enemyHusk = Instantiate(itemHusk, graphicParent.transform);
			enemyHusk.AddComponent<ScaleWithParent>();
			enemyHusk.layer = LayerMask.NameToLayer("UI");
			enemyHusk.transform.position -= Vector3.forward * 2;
		}

		public void OnClick()
		{
			itemController.Use(this);
		}

		// Update is called once per frame
		void Update()
		{
		}
	}
}