using UnityEngine;

public class Paper : MonoBehaviour
{
    void Start()
    {
        var texture = Resources.Load<Texture2D>("Levels/" + SettingsInit.CurrentLevelName + "/"  + BellFinish.Level.RecipeTextPictureName);

        if (texture != null)
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.mainTexture = texture;
            }
        }
        if (TryGetComponent(out ShowObjectInfo info))
        {
            info.ObjectInfo = BellFinish.Level.RecipeText;
        }
    }
}
