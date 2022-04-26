using System;
using Helpers;
using JetBrains.Annotations;
using UnityEngine;

namespace Lightning
{
    public class LightingZap : MonoBehaviour
    {
        public Vector2 from;
        public Vector2 to;
        private LineRenderer _lineRenderer;
        private Timer _timer;

        [CanBeNull]
        public event EventHandler OnZapped;
    
        //TODO enque lightning only fater this is done (On Time) and not immeadiatly. 
        
        // Start is called before the first frame update
        void Start()
        {
            _lineRenderer = this.GetComponentStrict<LineRenderer>();
            _lineRenderer.SetPositions(new Vector3[]
            {
                from,
                to
            });

            _timer = new Timer(0.5f);
            _timer.Start();
            _timer.OnTime += (_, _) =>
            {
                OnZapped?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);                
            };
        }

        // Update is called once per frame
        void Update()
        {
            _timer.Update();
        }
    }
}
