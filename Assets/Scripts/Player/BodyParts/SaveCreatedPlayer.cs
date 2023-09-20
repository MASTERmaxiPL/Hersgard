using UnityEngine;

public class SaveCreatedPlayer : MonoBehaviour
{
    [SerializeField]
    private SO_CharacterBody characterBody;
    private static SaveCreatedPlayer instance;

    private void Awake()
    {
        instance = this;
    }

    public static SaveCreatedPlayer GetInstance()
    {
        return instance;
    }


    public void SaveCreatedCharacter(RuntimeAnimatorController[] controllers, Color32[] colors)
    {
        saveControllers(controllers);
        saveColors(colors);
    }

    private void saveControllers(RuntimeAnimatorController[] controllers)
    {
        for(int i = 0; i < controllers.Length; i++)
        {
            characterBody.characterBodyParts[i] = controllers[i];
        }
    }
    private void saveColors(Color32[] colors)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            characterBody.colorList[i] = colors[i];
        }
    }
}