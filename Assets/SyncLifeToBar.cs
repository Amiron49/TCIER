using System;
using UnityEngine;

public class SyncLifeToBar : MonoBehaviour
{
    public Life life;
    public HudBar bar;
    
    // Start is called before the first frame update
    void Start()
    {
        if (life == null)
            throw new ArgumentNullException(nameof(life));
        
        if (bar == null)
            throw new ArgumentNullException(nameof(bar));

        life.OnHealthChange += (sender, from, to) =>
        {
            var percentage = to / (float)life.health * 100f;
            bar.GradualChangeFill(percentage);
        };
    }
}
