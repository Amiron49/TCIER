using Helpers;
using InternalLogic;
using Menu;
using UnityEngine;

public class EmitterPositionPreviewSync : MonoBehaviour
{
    public Gun AssociatedGun;
    private RectTransform _rectTransform;
    public EmitterPositionEditor RealEditor;

    // Start is called before the first frame update
    void Start()
    {
        AssociatedGun.OnOffsetChange += AssociatedGunOnOnOffsetChange;
        AssociatedGun.OnRotationChange += AssociatedGunOnOnRotationChange;
        _rectTransform = this.GetComponentStrict<RectTransform>();
        ForceSync();
    }

    private void ForceSync()
    {
        AssociatedGunOnOnRotationChange(this, AssociatedGun.Rotation);
        AssociatedGunOnOnOffsetChange(this, AssociatedGun.Offset);
    }

    private void AssociatedGunOnOnRotationChange(object sender, Quaternion e)
    {
        _rectTransform.rotation = e;
    }

    private void AssociatedGunOnOnOffsetChange(object sender, Vector2 e)
    {
        var editorOffset = CalculateEditorOffset(e);
        _rectTransform.localPosition = editorOffset;
    }
    
    private Vector2 CalculateEditorOffset(Vector2 offset)
    {
        var factor = RealEditor.RealOffsetToLocalOffsetFactor();
        return factor * offset;
    }

    private void OnDestroy()
    {
        AssociatedGun.OnOffsetChange -= AssociatedGunOnOnOffsetChange;
        AssociatedGun.OnRotationChange -= AssociatedGunOnOnRotationChange;
    }
}
