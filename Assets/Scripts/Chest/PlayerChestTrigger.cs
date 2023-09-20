using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChestTrigger : Interactible
{
    public override void Interact()
    {
        OpenPlayerChest();
    }

    private void OpenPlayerChest()
    {
        ChestInvManager.GetInstance().OpenChest();
    }
}

