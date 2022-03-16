using Helpers;
using InternalLogic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmitterRotationEditor : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler
{
    public Gun EditorFor;
    public RectTransform RectTransformToRotate;
    private RectTransform _parentRectTransform;
    private Camera _associatedCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        _associatedCamera = RectTransformToRotate.root.GetComponent<Canvas>().rootCanvas.worldCamera;
        _parentRectTransform = RectTransformToRotate.parent.GetComponentStrict<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_parentRectTransform, eventData.position, _associatedCamera, out var localPoint);
        RectTransformToRotate.LookAt2d(localPoint);
        EditorFor.Rotation = RectTransformToRotate.rotation;
    }
    
    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
}
