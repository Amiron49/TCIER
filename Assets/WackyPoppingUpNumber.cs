using System;
using Helpers;
using TMPro;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class WackyPoppingUpNumber : MonoBehaviour
{
    public int Number;
    public float Lifetime = 2f;
    public float XFanRange = 0.2f;
    public float TrajectoryForce;
    public float TrajectoryForceVariance = 0.1f;
    private Vector2 _initialTrajectory;
    private float _force;
    private Vector2 _wackTrajectory;
    private RectTransform _rectTransform;
    private  TMP_Text _numberDisplay;
    private Timer _timer;
    public Color Color = Color.white;

    // Start is called before the first frame update
    void Start()
    {
        _numberDisplay = this.GetComponentStrict<TMP_Text>();
        _numberDisplay.text = Number.ToString();
        _numberDisplay.color = Color;
        var random = Random.CreateFromIndex((uint) DateTime.Now.Millisecond * (uint)gameObject.GetHashCode());
        //var random = Random.CreateFromIndex((uint) DateTime.Now.Millisecond);
        var trajectoryForceVariance = TrajectoryForceVariance * TrajectoryForce;
        _force = random.NextFloat(TrajectoryForce - trajectoryForceVariance, TrajectoryForce + trajectoryForceVariance);
        _initialTrajectory = new Vector2(random.NextFloat(-XFanRange, XFanRange), 1).normalized;
        _rectTransform = this.GetComponentStrict<RectTransform>();
        _timer = new Timer(Lifetime);
        _timer.Start();
        _timer.OnTime += (_, _) =>
        {
            _timer.Stop();
            Destroy(gameObject);
        };
    }

    // Update is called once per frame
    void Update()
    {
        _timer.Update();
        var targetPosition = (Vector3)_initialTrajectory * (_force * EaseOutExpo(_timer.Progress));
        _rectTransform.localPosition = targetPosition;
    }
    
    float EaseOutExpo(float x)  
    {
        return Math.Abs(x - 1) < 0.0001 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }
}
