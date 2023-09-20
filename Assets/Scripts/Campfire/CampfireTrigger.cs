using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireTrigger : Interactible
{
    public override void Interact()
    {
        CampfireManager.GetInstance().ShowCampfireOptions();
    }
}
