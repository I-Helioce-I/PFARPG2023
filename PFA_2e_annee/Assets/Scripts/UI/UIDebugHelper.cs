using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDebugHelper : MonoBehaviour
{
    public CharacterStateHandler PlayerStateHandler;

    public TextMeshProUGUI StateText;

    private void OnEnable()
    {
        PlayerStateHandler.TransitionedFromTo -= OnStateChanged;
        PlayerStateHandler.TransitionedFromTo += OnStateChanged;
    }

    private void OnDisable()
    {
        PlayerStateHandler.TransitionedFromTo -= OnStateChanged;
    }

    private void Start()
    {
        StateText.text = PlayerStateHandler.CharacterTypeState.ToString();
    }

    private void OnStateChanged(CharacterTypeState fromState, CharacterTypeState toState)
    {
        switch (toState)
        {
            case CharacterTypeState.None:
                break;
            case CharacterTypeState.Solid:
                StateText.text = "Solid";
                break;
            case CharacterTypeState.Liquid:
                StateText.text = "Liquid";
                break;
            case CharacterTypeState.Gas:
                StateText.text = "Gas";
                break;
            case CharacterTypeState.TriplePoint:
                break;
            default:
                break;
        }
    }
}
