using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
