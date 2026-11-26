using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using UnityEngine.SceneManagement;
using System.Collections;

public class Walking : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    public float speed = 10.0f;
    public bool onground = false;
    public float jumpheight = 10;
    private Animator anim;
    public int maxjumps = 2;
    private int jumpCount = 0;
    public int maxhealth = 100;
    private int heath;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        heath = maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        walking();
        Jump();
        SetAnimation(Horizontal);

    }

    void walking()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        Vector2 direction = new Vector2(Horizontal, 0);
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);
    }
    void Jump()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && (onground || jumpCount < maxjumps))
        {
            rb.linearVelocity = new Vector2(Horizontal, jumpheight);
            onground = false;
            jumpCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onground = true;
            Debug.Log("Opdegrond");
            jumpCount = 0;
        }
        if (collision.gameObject.CompareTag("Damage"))
        {
            onground = true;
            heath -= 25;
            StartCoroutine(blinkedred());
            if (heath <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
            }

            Debug.Log("-10 health");
        }

    }

    private void SetAnimation(float Horizontal)
    {

        if (onground)
        {
            if (Horizontal == 0)
            {
                anim.Play("player_idle");
            }
            else
            {

                anim.Play("player_Run");
            }

        }
        else

            if (rb.linearVelocityY > 0)
            {
                anim.Play("Player_Jump");
            }
            else if (rb.linearVelocity.y < -0.1f)
            {
                anim.Play("player_fall");
            }
        if (Horizontal > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (Horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }

    }
    private IEnumerator blinkedred()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }


}


    
    

