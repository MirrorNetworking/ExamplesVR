using UnityEngine;
using UnityEngine.SceneManagement;

public class VRCanvasMenu : MonoBehaviour
{
    private string mapName;

    public void ButtonMap(int _map)
    {
        if (_map == 1)
        {
            mapName = "SceneVR-Basic";
        }
        else if (_map == 2)
        {
            mapName = "SceneVR-Common";
        }
        else if (_map == 3)
        {
            mapName = "SceneVR-UnityDemo";
        }

        //Debug.Log(name + " loading map: " + mapName);

        SceneManager.LoadScene(mapName);

    }
}

