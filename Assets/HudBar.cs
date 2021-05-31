using System;
using UnityEngine;
using UnityEngine.UI;

public class HudBar : MonoBehaviour
{
    public float targetFill = 100;
    public float fillVelocity = 5;
    public float minFillVelocity = 5;
    public Image filling;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void GradualChangeFill(float target)
    {
        var difference = CurrentFill() - target;
        var velocity = Mathf.Abs(difference * 0.5f);

        if (velocity > minFillVelocity)
            fillVelocity = velocity;
        
        targetFill = target;
    }

    // Update is called once per frame
    void Update()
    {
        var currentFill = CurrentFill();
        
        var delta = currentFill - targetFill;
        
        if (!(Mathf.Abs(delta) > 0.1)) 
            return;
        
        var nextTargetFill = currentFill - Mathf.Sign(delta) * fillVelocity;

        if (delta < minFillVelocity)
        {
            SetUiFill(targetFill);
            return;
        }

        var nextFill = Mathf.Lerp(currentFill, nextTargetFill, Time.deltaTime);

        SetUiFill(nextFill);
    }

    private void SetUiFill(float target)
    {
        var adjusted = Math.Abs(target - 100);
        filling.rectTransform.offsetMax = new Vector2(-adjusted, filling.rectTransform.offsetMax.y);
    }
    
    private float CurrentFill()
    {
        return 100 + filling.rectTransform.offsetMax.x;
    }
}
