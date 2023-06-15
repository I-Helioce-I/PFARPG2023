using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CombatExperienceScreen : MonoBehaviour
{
    public Button ConfirmButton;

    private void OnEnable()
    {
        ConfirmButton.Select();
    }
}
