using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New equipment", menuName ="Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentType equipSlot;

    public int armorModifier;
    public int damageModifier;
    public AnimatorOverrideController animController;

    public override void Use()
    {
        base.Use();
        EquipmentManager.instance.Equip(this);
    }
}

public enum EquipmentType
{
    Helmet,
    Chestplate,
    Leggins,
    Boots,

    LeftHandWeapon,
    RightHandWeapon,
    BothHandWeapon,

    Potion,
    Eatable,
}

public enum SpecialEffects
{
    Bleed,
    Poison,
    Fire
}
