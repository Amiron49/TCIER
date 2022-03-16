using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Menu
{
    public class PropertyDisplay : MonoBehaviour
    {
        [FormerlySerializedAs("PropertyNameText")] public TMP_Text propertyNameText;
        [FormerlySerializedAs("FromText")] public TMP_Text fromText;
        [FormerlySerializedAs("ToText")] public TMP_Text toText;
        [FormerlySerializedAs("Arrow")] public GameObject arrow;
    
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void DisplayNormal(string propertyName, float value)
        {
            propertyNameText.text = propertyName;
            toText.text = $"{value:F}";
            arrow.SetActive(false);
            fromText.gameObject.SetActive(false);
        }
    
        public void DisplayPreview(string propertyName, float from, float to, bool positive)
        {
            propertyNameText.text = propertyName;
            fromText.text = $"{from:F}";
            toText.text = positive ? $"<color=green>{to:F}</color>" : $"<color=red>{to:F}</color>";
            arrow.SetActive(true);
            fromText.gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
