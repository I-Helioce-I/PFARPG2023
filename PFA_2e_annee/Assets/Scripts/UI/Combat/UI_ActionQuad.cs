using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ActionQuad : MonoBehaviour
{
    [Header("Actions")]
    public UI_ActionSlot[] Actions = new UI_ActionSlot[4];

    public void CloseAllActionSlots()
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            Actions[i].ForceFold();
        }
    }
}
