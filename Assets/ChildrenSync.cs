using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

#nullable enable
public class ChildrenSync<T> where T : Component
{
	private readonly GameObject _self;
	private readonly List<T> _managedChildren = new();
	private readonly Func<int, T> _childFactory;
	public event EventHandler<(T GameObject, int Index)>? OnDestroy;

	public ChildrenSync(GameObject self, Func<int, T> childFactory)
	{
		_self = self;
		_childFactory = childFactory;
	}

	public void Change(int difference)
	{
		switch (difference)
		{
			case > 0:
				AddNew(difference);
				break;
			case < 0:
				RemoveLastN(math.abs(difference));
				break;
		}
	}

	public void RememberAllCurrentChildren()
	{
		var childrenCount = _self.transform.childCount;

		for (var i = 0; i < childrenCount; i++)
		{
			var existing = _self.transform.GetChild(i).gameObject;
			var asTyped = existing.GetComponent<T>();
			if (asTyped == null)
			{
				throw new Exception($"Expected all children to be of type {typeof(T).Name}. But it wasn't");
			}

			_managedChildren.Add(asTyped);
		}
	}

	private void AddNew(int count)
	{
		var currentMaxIndex = _managedChildren.Count - 1;

		for (var indexOffset = 1; indexOffset <= count; indexOffset++)
		{
			_managedChildren.Add(_childFactory(currentMaxIndex + indexOffset));
		}
	}

	private void RemoveLastN(int count)
	{
		Debug.Log($"count: {count}");
		var removalStartIndex = _managedChildren.Count - count;
		var toBeRemovedEditors = _managedChildren.ToArray()[removalStartIndex..^(count - 1)];
		Debug.Log($"toBeRemovedEditors count: {toBeRemovedEditors.Length}");
		Debug.Log($"removalStartIndex: {removalStartIndex}");

		for (var offset = 0; offset < toBeRemovedEditors.Length; offset++)
		{
			var index = removalStartIndex + offset;
			var toBeRemoved = toBeRemovedEditors[offset];
			OnDestroy?.Invoke(this, (toBeRemoved, index));
			Object.Destroy(toBeRemoved.gameObject);
		}

		_managedChildren.RemoveRange(removalStartIndex, count);
	}
}


public class ChildrenSync<T, TKey> : ChildrenSync<T, TKey, TKey> where T : Component
{
	public ChildrenSync(Func<TKey, T> childFactory) : base((key, _) => childFactory(key), (_, _) => { })
	{
	}

	public void Update(IEnumerable<TKey> updatedList)
	{
		base.Update(updatedList.Select(x => (x, x)).ToList());
	}
}

public class ChildrenSync<T, TKey, TState> where T : Component
{
	public readonly Dictionary<TKey, (TState args, T child)> ManagedChildren = new();
	private readonly Func<TKey, TState, T> _childFactory;
	private readonly Action<T, TState>? _updateFunc;
	public event EventHandler<(TKey, T GameObject)>? OnDestroy;

	public ChildrenSync(Func<TKey, TState, T> childFactory, Action<T, TState>? updateFunc)
	{
		_childFactory = childFactory;
		_updateFunc = updateFunc;
	}

	public void Update(IList<(TKey Key, TState Args)> updatedList)
	{
		var allToAdd = updatedList.Where(
			potentiallyNew => !ManagedChildren.Any(existing => potentiallyNew.Key!.Equals(existing.Key))).ToList();
		var allToRemove = ManagedChildren.Where(existing => !updatedList.Any(updated => updated.Key!.Equals(existing.Key))).ToList();

		foreach (var toRemove in allToRemove)
		{
			Remove(toRemove.Key);
		}

		foreach (var (key, args) in allToAdd)
		{
			AddNew(key, args);
		}

		if (_updateFunc == null)
			return;

		var allToUpdate = updatedList.Except(allToAdd).ToList();

		foreach (var (key, args) in allToUpdate)
		{
			_updateFunc(ManagedChildren[key].child, args);
		}
	}

	private void AddNew(TKey key, TState args)
	{
		var toAdd = _childFactory(key, args);
		ManagedChildren[key] = (args, toAdd);
	}

	private void Remove(TKey key)
	{
		var (_, child) = ManagedChildren[key];

		OnDestroy?.Invoke(this, (key, child));
		Object.Destroy(child.gameObject);
		ManagedChildren.Remove(key);
	}
}