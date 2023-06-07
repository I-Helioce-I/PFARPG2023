using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JRPG/Game/Dialogue/DialogueLinesSet")]
public class DialogueLinesSet : ScriptableObject
{
    public List<DialogueLine> DialogueLines = new List<DialogueLine>();
}
