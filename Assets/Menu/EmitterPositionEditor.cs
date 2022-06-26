using Helpers;
using InternalLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class EmitterPositionEditor : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler
    {
        private Gun _editorFor;
        public GunIndexProvider GunIndexProvider;
        public RectTransform RectTransformToMove;
        public RectTransform PlayerStandIn;
        private RectTransform _parentRectTransform;
        private Camera _associatedCamera;

        // Start is called before the first frame update
        void Start()
        {
            RectTransformToMove ??= this.GetComponentStrict<RectTransform>();
            _associatedCamera = RectTransformToMove.root.GetComponent<Canvas>().rootCanvas.worldCamera;
            _parentRectTransform = RectTransformToMove.parent.GetComponentStrict<RectTransform>();
            _editorFor = Game.Instance.State.Inventory.Body.Guns[GunIndexProvider.gunIndex];
        }

        public void OnDrag(PointerEventData eventData)
        {
            MoveTargetRectangle(eventData);
            _editorFor.Offset = CalculateOffset();
        }

        private void MoveTargetRectangle(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentRectTransform, eventData.position, _associatedCamera, out var localPoint);
            RectTransformToMove.localPosition = localPoint;
        }

        private Vector2 CalculateOffset()
        {
            var sizeOne = PlayerStandIn.sizeDelta / 2;
            var distance = RectTransformToMove.position - PlayerStandIn.position;
            var distanceInProportionToSizeOne = distance / sizeOne;
            
            return distanceInProportionToSizeOne;
        }

        public float RealOffsetToLocalOffsetFactor()
        {
            return  RectTransformToMove.localPosition.magnitude / _editorFor.Offset.magnitude;
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
    }
}
