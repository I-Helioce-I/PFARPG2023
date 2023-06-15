using UnityEngine;

public class UI_GoBack : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            OptionManager.instance.CloseParty();
            Destroy(this.gameObject);
        }
    }
}
