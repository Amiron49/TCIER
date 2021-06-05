using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    public int amount = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Game.Instance.State.AddMoney(amount);
        Destroy(gameObject);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        Game.Instance.State.AddMoney(amount);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
