using UnityEngine;
using UnityEngine.UI;

public class BildID : MonoBehaviour
{
    public int id;
    public GameManager gameManager;
    [SerializeField] private Image img;

    private void Start()
    {
        img = GetComponent<Image>();
        gameManager = FindFirstObjectByType<GameManager>();
        this.img.sprite = gameManager.bild[id];
    }
}
