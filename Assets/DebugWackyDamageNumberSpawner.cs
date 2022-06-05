using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugWackyDamageNumberSpawner : MonoBehaviour
{
    public float Delay = 1f;
    public WackyPoppingUpNumber WackyPoppingUpPrefab;
    private Timer _timer;

    // Start is called before the first frame update
    void Start()
    {
        _timer = new Timer(Delay);
        _timer.Start();
        _timer.OnTime += (sender, f) =>
        {
            Instantiate(WackyPoppingUpPrefab, transform);
        };
    }

    // Update is called once per frame
    void Update()
    {
        _timer.Update();
    }
}
