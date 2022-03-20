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
		var toBeRemovedEditors = _managedChildren.ToArray()[..count];
		var removalStartIndex = _managedChildren.Count - count;


		for (var offset = 0; offset < toBeRemovedEditors.Length; offset++)
		{
			var index = removalStartIndex - offset;
			var toBeRemoved = toBeRemovedEditors[offset];
			OnDestroy?.Invoke(this, (toBeRemoved, index));
			Object.Destroy(toBeRemoved.gameObject);
		}

		_managedChildren.RemoveRange(removalStartIndex, count);
	}
}


public class ChildrenSync<T, TKey> where T : Component
{
	private readonly GameObject _self;
	private readonly Dictionary<TKey, T> _managedChildren = new();
	private readonly Func<TKey, T> _childFactory;
	public event EventHandler<(TKey, T GameObject)>? OnDestroy;

	public ChildrenSync(GameObject self, Func<TKey, T> childFactory)
	{
		_self = self;
		_childFactory = childFactory;
	}

	public void Update(IList<TKey> updatedList)
	{
		var allToAdd = updatedList.Where(potentiallyNew => !_managedChildren.Any(existing => potentiallyNew!.Equals(existing.Key)));
		var allToRemove = _managedChildren.Where(existing => !updatedList.Any(updated => updated!.Equals(existing.Key)));

		foreach (var toRemove in allToRemove)
		{
			Remove(toRemove.Key);
		}

		foreach (var toAdd in allToAdd)
		{
			AddNew(toAdd);
		}
	}

	private void AddNew(TKey key)
	{
		var toAdd = _childFactory(key);
		_managedChildren[key] = toAdd;
	}

	private void Remove(TKey key)
	{
		var removed = _managedChildren[key];
		
		OnDestroy?.Invoke(this, (key, removed));
		Object.Destroy(removed.gameObject);
		_managedChildren.Remove(key);
	}
}