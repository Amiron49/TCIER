using Menu.ItemTiles;
using TMPro;
using UnityEngine;

public static class GameObjectExtensions
{
	public static void BubbleText(Vector3 position, string text)
	{
		var bubbleText = Object.Instantiate(Game.Instance.bubbleTextPrefab, position, Quaternion.identity);
		var textObject = bubbleText.GetComponentInChildren<TMP_Text>();
		textObject.text = text;
	}

	public static void BubbleTextOnMe(this GameObject gameObject, string text, Vector3? offset = null)
	{
		var position = gameObject.transform.position + offset ?? Vector3.zero;
		var bubbleText = Object.Instantiate(Game.Instance.bubbleTextPrefab, position, Quaternion.identity, gameObject.transform);
		var textObject = bubbleText.GetComponentInChildren<TMP_Text>();
		textObject.text = text;
	}
	
	public static void BubbleTextOnMe(this ItemTile itemTile, string text)
	{
		var rectTransform = itemTile.GetComponent<RectTransform>();
		itemTile.gameObject.BubbleTextOnMe(text,  new Vector3(0, rectTransform.sizeDelta.y / 2, 0) );
	}
}