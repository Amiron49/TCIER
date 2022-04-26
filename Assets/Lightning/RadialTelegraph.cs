using UnityEngine;

#nullable enable
namespace Lightning
{
    public class RadialTelegraph : MonoBehaviour
    {
        public GameObject MaxIndicator = null!;
        public GameObject CurrentIndicator = null!;
        public float MaxRadius;
        private Transform _currentIndicatorTransform = null!;
    
        // Start is called before the first frame update
        void Start()
        {
        
            MaxIndicator.transform.localScale = Vector3.one * MaxRadius;
            _currentIndicatorTransform = CurrentIndicator.transform;
            _currentIndicatorTransform.localScale = Vector3.zero;
        }

        public void SetProgress(float progress)
        {
            _currentIndicatorTransform.localScale = Vector3.Lerp(Vector3.zero,  Vector3.one * MaxRadius, progress);
        }
    
        public void SetMax(float radius)
        {
            MaxRadius = radius;
            MaxIndicator.transform.localScale = Vector3.one * radius;
        }
    }
}
