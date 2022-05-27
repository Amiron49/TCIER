using System;
using Helpers;
using JetBrains.Annotations;
using UnityEngine;

namespace Lightning
{
    public class LightingZap : MonoBehaviour, IZap
    {
        public Vector2 From 
        {
            get => _from;
            set => _from = value;
        }

        public Vector2 To       
        {
            get => _to;
            set => _to = value;
        }

        public float Duration
        {
            get => _duration;
            set => _duration = value;
        }

        private LineRenderer _lineRenderer;
        private Timer _timer;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private Vector2 _from;
        [SerializeField] private Vector2 _to;

        [CanBeNull]
        public event EventHandler OnZapStart;

        [CanBeNull]
        public event EventHandler OnZapEnd;


        // Start is called before the first frame update


        void Start()
        {
            _lineRenderer = this.GetComponentStrict<LineRenderer>();
            _lineRenderer.SetPositions(new Vector3[]
            {
                _from,
                _to
            });

            _timer = new Timer(_duration);
            _timer.Start();
            _timer.OnTime += (_, _) =>
            {
                OnZapEnd?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);                
            };
            
            OnZapStart?.Invoke(this, EventArgs.Empty);
        }
        

        // Update is called once per frame
        void Update()
        {
            _timer.Update();
        }
    }
}

public interface IZap : IProjectile
{
    public Vector2 From { get; set; }
    public Vector2 To { get; set; }
    public float Duration { get; set; }
    event EventHandler OnZapStart;
    event EventHandler OnZapEnd;
}
