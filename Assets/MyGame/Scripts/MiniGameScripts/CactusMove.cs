using UnityEngine;

public class CactusMove : MonoBehaviour
{
    public float speed = 300f;

    private RectTransform rt;
    private CactusSpawn gameManager;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Initialize(CactusSpawn manager, float moveSpeed)
    {
        gameManager = manager;
        speed = moveSpeed;
    }

    void Update()
    {
        if (gameManager == null || !gameManager.GameActive)
            return;

        rt.anchoredPosition += Vector2.left * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameManager.GameActive)
        {
            gameManager.Death();
        }
    }
}