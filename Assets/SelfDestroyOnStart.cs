using UnityEngine;

public class SelfDestroyOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
