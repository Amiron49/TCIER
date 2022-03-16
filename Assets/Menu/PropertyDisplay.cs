using TMPro;
using UnityEngine;

public class PropertyUI : MonoBehaviour
{
    public TMP_Text PropertyNameText;
    public TMP_Text FromText;
    public TMP_Text ToText;
    public GameObject Arrow;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisplayNormal(string propertyName, float value)
    {
        PropertyNameText.text = propertyName;
        ToText.text = $"{value:F}";
        Arrow.SetActive(false);
        FromText.gameObject.SetActive(false);
    }
    
    public void DisplayPreview(string propertyName, float from, float to, bool positive)
    {
        PropertyNameText.text = propertyName;
        FromText.text = $"{from:F}";
        ToText.text = positive ? $"<color=green>{to:F}</color>" : $"<color=red>{to:F}</color>";
        Arrow.SetActive(true);
        FromText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
