using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Image settings;
    public void OpenSettings()
    {
        if (settings != null)
        {
            settings.gameObject.SetActive(true);
        }
    }
    public void CloseSettings() 
    {
        if (settings != null)
        {
            settings.gameObject.SetActive(false);
        }
    }
}
