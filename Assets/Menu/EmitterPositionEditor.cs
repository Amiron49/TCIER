using Helpers;
using InternalLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class EmitterPositionEditor : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler
    {
        public Gun EditorFor;
        public RectTransform RectTransformToMove;
        private RectTransform _parentRectTransform;
        private Camera _associatedCamera;

        // Start is called before the first frame update
        void Start()
        {
            RectTransformToMove ??= this.GetComponentStrict<RectTransform>();
            _associatedCamera = RectTransformToMove.root.GetComponent<Canvas>().rootCanvas.worldCamera;
            _parentRectTransform = RectTransformToMove.parent.GetComponentStrict<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnMouseDrag()
        {
            Debug.Log("mouseDrag", this);
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, eventData.position, _associatedCamera, out var localPoint);
            RectTransformToMove.localPosition = localPoint;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
    }
}
