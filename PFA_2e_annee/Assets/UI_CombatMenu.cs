using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CombatMenu : MonoBehaviour
{
    public enum UICombatMenuState
    {
        Closed,
        WaitingInstruction,
        NavigateTabs,
        NavigateActions,
    }

    public List<UI_Tab> Tabs = new List<UI_Tab>();
}
