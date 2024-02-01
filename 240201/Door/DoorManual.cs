using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManual : DoorBase, IInteracable
{
    bool isUsing = false;

    public void Use()
    {
        if (isUsing)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
}
