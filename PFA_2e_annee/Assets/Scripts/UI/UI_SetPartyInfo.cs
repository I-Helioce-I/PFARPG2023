using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SetPartyInfo : MonoBehaviour, ISelectHandler
{
    [SerializeField] private int index;

    public void OnSelect(BaseEventData eventData)
    {
        PartyPanelSpawn.instance.SetStats(index);
    }
}
