using Helpers;
using InternalLogic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class EmitterRotationEditor : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler
    {
        private Gun _editorFor;
        public GunIndexProvider GunIndexProvider;
        public RectTransform RectTransformToRotate;
        private RectTransform _parentRectTransform;
        private Camera _associatedCamera;
    
        // Start is called before the first frame update
        void Start()
        {
            _associatedCamera = RectTransformToRotate.root.GetComponent<Canvas>().rootCanvas.worldCamera;
            _parentRectTransform = RectTransformToRotate.parent.GetComponentStrict<RectTransform>();
            _editorFor = Game.Instance.State.Inventory.Body.Guns[GunIndexProvider.gunIndex];
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_parentRectTransform, eventData.position, _associatedCamera, out var localPoint);
            RectTransformToRotate.LookAt2d(localPoint);
            _editorFor.Rotation = RectTransformToRotate.rotation;
        }
    
        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }
    }
}
