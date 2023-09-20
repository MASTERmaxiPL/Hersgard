using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterCreator : MonoBehaviour
{
    //Navigation
    [SerializeField]
    private Button backBtn, continueBtn, noBtn, yesBtn;
    [SerializeField]
    private GameObject confirmPanel;

    //Changing player
    [SerializeField]
    private Button menBtn, womenBtn, leftHairBtn, rightHairBtn, leftFacialBtn, rightFacialBtn, leftEyesBtn, rightEyesBtn,
        leftSkinBtn, rightSkinBtn, leftHairColorBtn, rightHairColorBtn, leftFacialColorBtn, rightFacialColorBtn;

    private int bodyPartId = 0, hairPartId = 0, facialPartId = 0, eyesPartId = 0;
    private bool menCheched = true;

    private Color32[] hairColors = new Color32[] { new Color32(92, 45, 19, 255), new Color32(147, 69, 27, 255), new Color32(241, 233, 65, 255), new Color32(34, 34, 34, 255), new Color32(92, 0, 0, 255), new Color32(123, 123, 123, 255) };
    private Color32[] colorList = new Color32[2];

    private int choosenHairColor = 0;
    private int choosenFacialColor = 0;

    //Rotatiing player
    [SerializeField]
    private Button rotateLeft, rotateRight;
    private int rotatePos = 1;

    //Changed anims and objects
    [SerializeField]
    private AnimatorOverrideController[] mBody, wBody, Eyes, Hair, FacialHair;
    private Animator[] animator;

    [SerializeField]
    private GameObject playerBody, playerEyes, playerHair, playerFacialHair;

    private RuntimeAnimatorController[] animList = new RuntimeAnimatorController[4];
    

    private PlayerBodyManager playerBodyManager;
    private static SaveCreatedPlayer saveCP;

    private void Start()
    {
        playerBodyManager = PlayerBodyManager.GetInstance();
        animator = PlayerBodyManager.GetAnims();
        animator[0].SetFloat("lastMoveY", -1);
}
    private void Awake()
    {
        confirmPanel.SetActive(false);

        //Navigation Buttons
        backBtn.onClick.AddListener(BackToMainMenu);
        continueBtn.onClick.AddListener(ShowConfirmPanel);
        noBtn.onClick.AddListener(BackToCharacterCreator);
        yesBtn.onClick.AddListener(StartGame);

        //Options Buttons
        menBtn.onClick.AddListener(ChangeSexMen);
        womenBtn.onClick.AddListener(ChangeSexWoman);
        leftSkinBtn.onClick.AddListener(Skinleft);
        rightSkinBtn.onClick.AddListener(SkinRight);
        leftHairBtn.onClick.AddListener(HairLeft);
        rightHairBtn.onClick.AddListener(HairRight);
        leftFacialBtn.onClick.AddListener(FacialLeft);
        rightFacialBtn.onClick.AddListener(FacialRight);
        leftEyesBtn.onClick.AddListener(EyesLeft);
        rightEyesBtn.onClick.AddListener(EyesRight);
        leftHairColorBtn.onClick.AddListener(HairColorLeft);
        rightHairColorBtn.onClick.AddListener(HairColorRight);
        leftFacialColorBtn.onClick.AddListener(FacialColorLeft);
        rightFacialColorBtn.onClick.AddListener(FacialColorRight);

        //Rotate PlayerView
        rotateLeft.onClick.AddListener(RotateLeft);
        rotateRight.onClick.AddListener(RotateRight);
    }

    private void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ShowConfirmPanel()
    {
        confirmPanel.SetActive(true);
    }

    private void BackToCharacterCreator()
    {
        confirmPanel.SetActive(false);
    }

    private void StartGame()
    {
        animList[0] = playerBody.GetComponent<Animator>().runtimeAnimatorController;
        animList[1] = playerEyes.GetComponent<Animator>().runtimeAnimatorController;
        animList[2] = playerHair.GetComponent<Animator>().runtimeAnimatorController;
        animList[3] = playerFacialHair.GetComponent<Animator>().runtimeAnimatorController;

        colorList[0] =playerHair.GetComponent<Image>().color;
        colorList[1] = playerFacialHair.GetComponent<Image>().color;

        saveCP = SaveCreatedPlayer.GetInstance();
        saveCP.SaveCreatedCharacter(animList, colorList);

        PlayerPrefs.SetString("NewGame", "True");

        SceneManager.LoadScene("Game");
    }

    // Changing body buttons
    private void ChangeSexMen()
    {
        playerBodyManager.UpdatePlayerVisuals(1, mBody[bodyPartId]);
        menCheched = true;
        StartCoroutine(ChangeSprites());
    }
    private void ChangeSexWoman()
    {
        playerBodyManager.UpdatePlayerVisuals(1, wBody[bodyPartId]);
        menCheched = false;
        StartCoroutine(ChangeSprites());
    }
    private void ChangeBody()
    {
        if (menCheched)
        {
            playerBodyManager.UpdatePlayerVisuals(1, mBody[bodyPartId]);
        }
        else
        {
            playerBodyManager.UpdatePlayerVisuals(1, wBody[bodyPartId]);
        }
        StartCoroutine(ChangeSprites());
    }
    private void Skinleft()
    {
        if (bodyPartId == 0)
        {
            bodyPartId = mBody.Length - 1;
        }
        else
        {
            bodyPartId -= 1;
        }
        ChangeBody();
    }
    private void SkinRight()
    {
        if (bodyPartId == mBody.Length - 1)
        {
            bodyPartId = 0;
        }
        else
        {
            bodyPartId += 1;
        }
        ChangeBody();
    }


    // Changing hair buttons
    private void HairLeft()
    {
        if (hairPartId == 0)
        {
            hairPartId = Hair.Length - 1;
        }
        else
        {
            hairPartId -= 1;
        }
        playerBodyManager.UpdatePlayerVisuals(3, Hair[hairPartId]);
        StartCoroutine(ChangeSprites());
    }
    private void HairRight()
    {
        if (hairPartId == Hair.Length - 1)
        {
            hairPartId = 0;
        }
        else
        {
            hairPartId += 1;
        }
        playerBodyManager.UpdatePlayerVisuals(3, Hair[hairPartId]);
        StartCoroutine(ChangeSprites());
    }
    private void HairColorLeft()
    {
        if(choosenHairColor == 0)
        {
            choosenHairColor = hairColors.Length-1;
        }
        else
        {
            choosenHairColor -= 1;
        }
        playerHair.GetComponent<Image>().color = hairColors[choosenHairColor];
        StartCoroutine(ChangeSprites());

    }
    private void HairColorRight()
    {
        if (choosenHairColor == hairColors.Length-1)
        {
            choosenHairColor = 0;
        }
        else
        {
            choosenHairColor += 1;
        }
        playerHair.GetComponent<Image>().color = hairColors[choosenHairColor];
        StartCoroutine(ChangeSprites());
    }


    // Changing facial hair buttons
    private void FacialLeft()
    {
        if (facialPartId == 0)
        {
            facialPartId = FacialHair.Length - 1;
        }
        else
        {
            facialPartId -= 1;
        }
        playerBodyManager.UpdatePlayerVisuals(4, FacialHair[facialPartId]);
        StartCoroutine(ChangeSprites());
    }
    private void FacialRight()
    {
        if (facialPartId == FacialHair.Length - 1)
        {
            facialPartId = 0;
        }
        else
        {
            facialPartId += 1;
        }
        playerBodyManager.UpdatePlayerVisuals(4, FacialHair[facialPartId]);
        StartCoroutine(ChangeSprites());
    }
    private void FacialColorLeft()
    {
        if (choosenFacialColor == 0)
        {
            choosenFacialColor = hairColors.Length - 1;
        }
        else
        {
            choosenFacialColor -= 1;
        }
        playerFacialHair.GetComponent<Image>().color = hairColors[choosenFacialColor];
        StartCoroutine(ChangeSprites());
    }
    private void FacialColorRight()
    {
        if (choosenFacialColor == hairColors.Length - 1)
        {
            choosenFacialColor = 0;
        }
        else
        {
            choosenFacialColor += 1;
        }
        playerFacialHair.GetComponent<Image>().color = hairColors[choosenFacialColor];
        StartCoroutine(ChangeSprites());
    }


    // Changing eyes buttons
    private void EyesLeft()
    {
        if (eyesPartId == 0)
        {
            eyesPartId = Eyes.Length - 1;
        }
        else
        {
            eyesPartId -= 1;
        }
        playerBodyManager.UpdatePlayerVisuals(2, Eyes[eyesPartId]);
        StartCoroutine(ChangeSprites());
    }
    private void EyesRight()
    {
        if(eyesPartId == Eyes.Length - 1)
        {
            eyesPartId = 0;
        }
        else
        {
            eyesPartId += 1;
        }
        playerBodyManager.UpdatePlayerVisuals(2, Eyes[eyesPartId]);
        StartCoroutine(ChangeSprites());
    }


    // Helping functions
    private IEnumerator ChangeSprites()
    {
        yield return new WaitForSeconds(0.2f);
        ChangeFromSpriteToImage(playerBody);
        ChangeFromSpriteToImage(playerEyes);
        ChangeFromSpriteToImage(playerHair);
        ChangeFromSpriteToImage(playerFacialHair);
    }
    private void ChangeFromSpriteToImage(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
    }


    // Rotate player visuals buttons
    private void RotateLeft()
    {
        if (rotatePos == 1)
            rotatePos = 4;
        else
            rotatePos -= 1;

        ChangeRotation();
    }
    private void RotateRight()
    {
        if (rotatePos == 4)
            rotatePos = 1;
        else
            rotatePos += 1;

        ChangeRotation();
    }
    private void ChangeRotation()
    {
        foreach(Animator anim in animator)
        {
            anim.SetFloat("lastMoveX", 0);
            anim.SetFloat("lastMoveY", 0);
            switch (rotatePos)
            {
                case 1:
                    anim.SetFloat("lastMoveY", -1);
                    break;
                case 2:
                    anim.SetFloat("lastMoveX", 1);
                    break;
                case 3:
                    anim.SetFloat("lastMoveY", 1);
                    break;
                case 4:
                    anim.SetFloat("lastMoveX", -1);
                    break;
            }
            StartCoroutine(ChangeSprites());
        }
    }
}
