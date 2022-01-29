using System;
using UnityEngine;

namespace UI
{
    public class ScaleWithParent : MonoBehaviour
    {
        private RectTransform parent;
    
        // Start is called before the first frame update
        void Start()
        {
            parent = GetComponentInParent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            var parentRectangle = parent.rect;
            var smallest = Math.Min(parentRectangle.size.y, parentRectangle.size.x) * 0.9f;

            transform.localScale = new Vector3(smallest, smallest, 1);
        }
    }
}
