using Helpers;
using TMPro;
using UnityEngine;

public class UpdateCounter : MonoBehaviour
{
    private TMP_Text _text;
    
    // Start is called before the first frame update
    void Start()
    {
        _text = this.GetComponentInChildrenStrict<TMP_Text>();
        Game.State.OnMoneyChange += (sender, amount) =>
        {
            _text.text = Game.State.Money.ToString();
        };
    }
}
