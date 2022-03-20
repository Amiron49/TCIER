using JetBrains.Annotations;
using UnityEngine;

public class GunArray : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Game.Instance.State.Inventory.Body.OnGunCountChange += (sender, difference) =>
        {

        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}