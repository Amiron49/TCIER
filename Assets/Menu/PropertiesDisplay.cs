using System;
using System.Collections.Generic;
using UnityEngine;

namespace Menu
{
    public class PropertiesDisplay : MonoBehaviour
    {
        public PropertyDisplay propertyPrefab;

        private readonly Dictionary<string, PropertyDisplay> propertiesDisplayed = new();
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void DisplayNormal(Dictionary<string, float> properties)
        {
            if (properties.Count != propertiesDisplayed.Count)
            {
                InitProperties(properties);
            }
        
            foreach (var (key, value) in properties)
            {
                propertiesDisplayed[key].DisplayNormal(key, value);
            }
        }
    
        public void InitProperties(Dictionary<string, float> properties)
        {
            propertiesDisplayed.Clear();
            foreach (Transform children in transform)
            {
                Destroy(children.gameObject);
            }

            foreach (var property in properties)
            {
                var propertyUi = Instantiate(propertyPrefab, transform);
                propertiesDisplayed.Add(property.Key, propertyUi);
            }
        }
    
        public void DisplayPreview(Dictionary<string, float> oldProperties, Dictionary<string, float> newProperties, Dictionary<string, bool> increaseIsPositiveChangeMap)
        {
        
            if (oldProperties.Count != propertiesDisplayed.Count)
            {
                InitProperties(oldProperties);
            }
        
            foreach (var (key, newValue) in newProperties)
            {
                var oldValue = oldProperties[key];

                var change = newValue - oldValue;
                var isChange = Math.Abs(newValue - oldValue) > 0.000001;

                if (!isChange)
                {
                    propertiesDisplayed[key].DisplayNormal(key, newValue);
                    continue;
                }
            
                var increaseIsPositiveChange = increaseIsPositiveChangeMap[key];
                var isIncrease = Math.Sign(change) == 1;
                var isPositiveChange = isIncrease && increaseIsPositiveChange || !isIncrease && !increaseIsPositiveChange;
            
                propertiesDisplayed[key].DisplayPreview(key, oldValue, newValue, isPositiveChange);
            }
        }
    
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
