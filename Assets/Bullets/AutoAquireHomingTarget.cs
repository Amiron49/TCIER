using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using JetBrains.Annotations;
using UnityEngine;

public class AutoAquireHomingTarget : MonoBehaviour
{
    private Homing _homing;
    private Transform _ownTransform;
    private GameObject _target;
    private float _maxSearchRadius = 30;
    public Team HomesInOn;

    // Start is called before the first frame update
    void Start()
    {
        _ownTransform = transform;
        _homing = GetComponent<Homing>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            var target = FindNextBestTarget();
            SetTarget(target);
        }
    }

    private void SetTarget(GameObject target)
    {
        if (target == null)
            return;

        var onDestroyNotifier = target.GetComponentOrCreate<NotifyOnDestroy>();
        onDestroyNotifier.OnOnDestroy += (_, _) =>
        {
            _target = null;
            _homing.Target = null;
        };
        
        _target = target;
        _homing.Target = target.transform;
    }

    [CanBeNull]
    private GameObject FindNextBestTarget()
    {
        var results = new Collider2D[10];
        Physics2D.OverlapCircleNonAlloc(transform.position, _maxSearchRadius, results, HomesInOn.FoundOnLayer());

        var closest = results.Where(x => x != null).OrderBy(x =>
        {
            var distance = x.gameObject.transform.position - _ownTransform.position;
            return distance.magnitude;
        }).FirstOrDefault();

        if (closest == null)
            return null;

        return closest.gameObject;
    }
}
