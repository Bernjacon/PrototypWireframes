using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class TRexManager : MonoBehaviour
{
    public GameObject player;
    [SerializeField] int jumpforce;
    [SerializeField] bool isGrounded;
    public void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpforce));
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "GroundCollider")
        {
            isGrounded = true;
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "GroundCollider")
        {
            isGrounded = false;
        }
    }
}
