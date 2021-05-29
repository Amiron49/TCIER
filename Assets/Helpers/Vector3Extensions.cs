using UnityEngine;

namespace Helpers
{
	public static class Vector3Extensions
	{
		public const float StandardZ = 3;
		
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
}