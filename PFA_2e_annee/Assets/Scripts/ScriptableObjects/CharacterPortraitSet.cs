using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JRPG/Game/Dialogue/PortraitSet")]
public class CharacterPortraitSet : ScriptableObject
{
    public Sprite Neutral;
    public Sprite Angry;
    public Sprite Sad;
    public Sprite Flustered;
    public Sprite Edgy;
}
