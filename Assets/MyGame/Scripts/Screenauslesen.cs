using UnityEngine;

public class Screenauslesen : MonoBehaviour
{
    // Das Script ist nur für das auslesen der DPI da.
    void Start()
    {
        Debug.Log(Screen.dpi);
    }

}
