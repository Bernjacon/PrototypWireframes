using UnityEngine;

public class CactusMove : MonoBehaviour
{
    public float speed = 300f;

    void Update()
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition += Vector2.left * speed * Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player" && FindAnyObjectByType<CactusSpawn>().hasntWonYet)
        {
            FindAnyObjectByType<CactusSpawn>().Death();
            Debug.Log("Hit");
        }
    }
}

