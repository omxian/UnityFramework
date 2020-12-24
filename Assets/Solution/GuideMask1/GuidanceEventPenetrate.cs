
using UnityEngine;

public class GuidanceEventPenetrate : MonoBehaviour, ICanvasRaycastFilter
{
	private RectTransform mTagrget;

	public void SetTarget(RectTransform target)
	{
		mTagrget = target;
	}
	public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
	{
		if (mTagrget == null)
			return true;

		return !RectTransformUtility.RectangleContainsScreenPoint(mTagrget, sp, eventCamera);
	}
}