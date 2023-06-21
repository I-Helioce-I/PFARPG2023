using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Env_Chest : MonoBehaviour
{
    public Animator Anim;

    public void Open()
    {
        Anim.SetTrigger("Open");
    }
}
