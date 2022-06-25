using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FpsDisplay : MonoBehaviour
{
    private TMP_Text _text;
    private RoundQueue<(float time, int frames)> _lastXFrames;
    
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        _lastXFrames = new RoundQueue<(float time, int frames)>(120);
    }

    int _lastFrameCount = 0;
    
    // Update is called once per frame
    void Update()
    {
        var newFrames = Time.frameCount - _lastFrameCount;
        var time = Time.deltaTime;
        _lastXFrames.Add((time, newFrames));
        _lastFrameCount = Time.frameCount;

        var framesRendered = _lastXFrames.Content.Sum(x => x.frames);
        var timeFrame = _lastXFrames.Content.Sum(x => x.time);

        var fps = framesRendered / timeFrame;
        
        _text.text = $"FPS{fps};Delta:{Time.deltaTime};DeltaSmooth:{Time.smoothDeltaTime}";
    }

    private class RoundQueue<T>
    {
        public IList<T> Content => _internalQueue.ToList();
        private readonly Queue<T> _internalQueue;
        private readonly int _maxCapacity;

        public RoundQueue(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
            _internalQueue = new Queue<T>(_maxCapacity);
        }
        
        public void Add(T thing)
        {
            if (_internalQueue.Count == _maxCapacity)
                _internalQueue.Dequeue();
            
            _internalQueue.Enqueue(thing);
        }
    }
}
