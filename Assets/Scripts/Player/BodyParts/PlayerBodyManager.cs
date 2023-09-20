using UnityEngine;
using UnityEngine.UI;

public class PlayerBodyManager : MonoBehaviour
{
    private static PlayerBodyManager instance;
    [SerializeField]
    private static Animator[] animator;
    [SerializeField]
    private Animator[] animatorList;

    private static SpriteRenderer[] image;
    [SerializeField]
    private SpriteRenderer[] imageList;



    [SerializeField]
    private SO_CharacterBody characterBody;

    private void Awake()
    {
        instance = this;
        animator = animatorList;
        image = imageList;
        animatorList = null;
        imageList = null;
    }

    public static PlayerBodyManager GetInstance()
    {
        return instance;
    }
    public static Animator[] GetAnims()
    {
        return animator;
    }


    public void UpdatePlayerVisuals(int partId, AnimatorOverrideController overrideController)
    {
        switch (partId)
        {
            case 1:
                if (EquipmentManager.instance.isHelmetOn)
                {
                    print("1");
                    animator[0].runtimeAnimatorController = overrideController;
                }
                break;
            case 2:
                print("2");
                animator[1].runtimeAnimatorController = overrideController;
                break;
            case 3:
                print("3");
                animator[2].runtimeAnimatorController = overrideController;
                break;
            case 4:
                print("4");
                animator[3].runtimeAnimatorController = overrideController;
                break;
            case 5:
                print("5");
                animator[4].runtimeAnimatorController = overrideController;
                break;
            case 6:
                print("6");
                animator[5].runtimeAnimatorController = overrideController;
                break;
            case 7:
                print("7");
                animator[6].runtimeAnimatorController = overrideController;
                break;
            case 8:
                print("8");
                animator[7].runtimeAnimatorController = overrideController;
                break;
            default:
                Debug.Log("Unknown body part id");
                break;
        }
    }

    public void LoadNewPlayerBodyParts()
    {
        for (int i = 0; i < characterBody.characterBodyParts.Length; i++)
        {
            if (characterBody.characterBodyParts[i] != null)
            {
                animator[i].runtimeAnimatorController = characterBody.characterBodyParts[i];
            }
        }
        for (int i = 0; i < characterBody.colorList.Length; i++)
        {
            image[i].color = characterBody.colorList[i]; 
        }
    }
}
