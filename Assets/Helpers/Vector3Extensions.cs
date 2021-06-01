using System;
using UnityEngine;
#nullable enable
namespace Helpers
{
	public static class Vector3Extensions
	{
		public const float StandardZ = 0;
		
		public static Vector3 NoZ(this Vector3 input, float z = 0)
		{
			return new Vector3(input.x, input.y, z);
		}
	}

	public static class TransformExtensions
	{
		public static void LookAt2d(this Transform input, Vector3 target)
		{
			input.up = target - input.position;
		}
	}
	
	public static class GameObjectExtensions
	{
		public static T GetComponentInChildrenStrict<T>(this Component thing)
		{
			return thing.GetComponentInChildren<T>() ?? throw new NullReferenceException($"Couldn't find {typeof(T).Name}");
		}
		
		public static T GetComponentStrict<T>(this Component thing)
		{
			return thing.GetComponent<T>() ?? throw new NullReferenceException($"Couldn't find {typeof(T).Name}");
		}
	}
}