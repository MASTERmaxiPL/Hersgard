using UnityEngine;

[CreateAssetMenu(fileName = "New Character Body", menuName = "Character Body")]
public class SO_CharacterBody : ScriptableObject
{
    public RuntimeAnimatorController[] characterBodyParts;
    public Color32[] colorList;
}

