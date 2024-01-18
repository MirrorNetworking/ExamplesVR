using UnityEngine;

public class VRStaticVariables : MonoBehaviour
{
    // here we will store player and game information, that needs to be saved and passed between scenes

    public static string playerName = "";
    public static int handValue = 0; // 1 right hand, 2 left hand, used as a shortcut to tell interactable which controller was used
}
