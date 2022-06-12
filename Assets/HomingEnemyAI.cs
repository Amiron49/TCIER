using Helpers;
using UnityEngine;

public class HomingEnemyAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var homing = this.GetComponentStrict<Homing>();
        homing.Target = Game.Instance.State.Player.transform;
    }
}
