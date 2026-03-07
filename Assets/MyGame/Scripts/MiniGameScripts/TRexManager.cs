using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class TRexManager : MonoBehaviour
{
    public GameObject player;

    [SerializeField] int jumpforce;
    [SerializeField] bool isGrounded;

    public Animator dinoAnimator;
    public AudioSource jumpSound;

    public Rigidbody2D rb;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        if (player != null)
            rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!enabled) return;
        if (rb == null) Initialize();

        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpforce));
            jumpSound.Play();
        }
    }

    public void ResetTRex()
    {
        Initialize();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        isGrounded = true;

        if (dinoAnimator != null && dinoAnimator.gameObject.activeInHierarchy)
        {
            dinoAnimator.Rebind();
            dinoAnimator.Update(0f);
            dinoAnimator.speed = 4f;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GroundCollider"))
        {
            isGrounded = true;
            if (dinoAnimator != null)
                dinoAnimator.speed = 4f;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GroundCollider"))
        {
            isGrounded = false;
            if (dinoAnimator != null)
                dinoAnimator.speed = 0f;
        }
    }
}