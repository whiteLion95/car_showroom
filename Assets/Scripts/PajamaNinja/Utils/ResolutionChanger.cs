using UnityEngine;

public class ResolutionChanger : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;

    void Start()
    {
        Screen.SetResolution(width, height, true);
    }
}
