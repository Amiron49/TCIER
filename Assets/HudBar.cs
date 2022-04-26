using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HudBar : MonoBehaviour
{
    [FormerlySerializedAs("_targetFill")] [SerializeField] [FormerlySerializedAs("targetFill")] private float TargetFill = 100;
    [FormerlySerializedAs("fillVelocity")] public float FillVelocity = 5;
    [FormerlySerializedAs("filling")] public Image Filling;
    [FormerlySerializedAs("outerObject")] public RectTransform OuterObject;
    private float _maxSize;
    private float _previousFill;
    private float _currentFill;
    private float _fillTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _maxSize = OuterObject.sizeDelta.x;
        _previousFill = TargetFill;
    }

    // Update is called once per frame
    void Update()
    {
        _fillTime += Time.deltaTime / 2;
        _currentFill = Mathf.Lerp(_previousFill, TargetFill, _fillTime);
        SetUiFill(_currentFill);
    }

    private void SetUiFill(float target)
    {
        var adjusted = _maxSize - target;
        Filling.rectTransform.offsetMax = new Vector2(-adjusted, Filling.rectTransform.offsetMax.y);
    }

    public void GradualChangeFill(float targetFill)
    {
        _previousFill = _currentFill;
        _fillTime = 0;
        TargetFill = _maxSize * targetFill;
    }
}
