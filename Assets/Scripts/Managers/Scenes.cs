using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenes 
{
    public static void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
