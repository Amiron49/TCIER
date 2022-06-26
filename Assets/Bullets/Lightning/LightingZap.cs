using System;
using Helpers;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Lightning
{
    public class LightingZap : MonoBehaviour, IZap
    {
        public Transform From 
        {
            get => _from;
            set => _from = value;
        }

        public Transform To       
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
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;

        [CanBeNull]
        public event EventHandler OnZapStart;

        [CanBeNull]
        public event EventHandler OnZapEnd;


        // Start is called before the first frame update


        void Start()
        {
            _lineRenderer = this.GetComponentStrict<LineRenderer>();

            _timer = new Timer(_duration);
            _timer.Start();
            _timer.OnTime += (_, _) =>
            {
                OnZapEnd?.Invoke(this, EventArgs.Empty);
                Destroy(gameObject);                
            };
            
            OnZapStart?.Invoke(this, EventArgs.Empty);

            _registeredFromDestructionHandler = WatchForDestruction(From);
            _registeredToDestructionHandler = WatchForDestruction(To);
        }

        private NotifyOnDestroy _registeredFromDestructionHandler;
        private NotifyOnDestroy _registeredToDestructionHandler;
        
        private NotifyOnDestroy WatchForDestruction(Component thing)
        {
            var toDestroyNotifier = thing.gameObject.GetOrAddComponent<NotifyOnDestroy>();
            toDestroyNotifier.OnOnDestroy += HandleAnchorDestruction;
            return toDestroyNotifier;
        }
        
        private void HandleAnchorDestruction(object o, EventArgs eventArgs)
        {
            _fromOrToWasDestroyed = true;
            Destroy(gameObject);
        }

        private bool _fromOrToWasDestroyed = false;
        
        // Update is called once per frame
        void Update()
        {
            if (_fromOrToWasDestroyed)
                return;
            
            _lineRenderer.SetPositions(new[]
            {
                _from.position,
                _to.position
            });
            
            _timer.Update();
        }

        private void OnDestroy()
        {
            _registeredFromDestructionHandler.OnOnDestroy -= HandleAnchorDestruction;
            _registeredToDestructionHandler.OnOnDestroy -= HandleAnchorDestruction;
        }
    }
}

public interface IZap : IProjectile
{
    public Transform From { get; set; }
    public Transform To { get; set; }
    public float Duration { get; set; }
    event EventHandler OnZapStart;
    event EventHandler OnZapEnd;
}
