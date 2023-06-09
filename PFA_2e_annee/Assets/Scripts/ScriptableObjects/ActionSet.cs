using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JRPG/Game/Combat/ActionSet")]
public class ActionSet : ScriptableObject
{
    public ActionDescription SolidAction;
    public ActionDescription LiquidAction;
    public ActionDescription GasAction;
}
