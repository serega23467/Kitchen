using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenes : MonoBehaviour
{
    static Scenes instance;
    private void Awake()
    {
        if (instance == null)
        { 
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
