using UnityEngine;

public class PlayerStudent : MonoBehaviour
{
    public Animator animator;              // Reference to the Animator component
    public SpriteRenderer spriteRenderer;  // Reference to the SpriteRenderer component
    public Sprite newSprite;               // Sprite to change to when space is pressed
    public Sprite originalSprite;          // Original sprite to revert back to when space is released

    void Start()
    {
        // Ensure the animator is initially disabled if you want
        animator.enabled = false;
    }

    void Update()
    {
        // Enable animator when 'W' is pressed
        if (Input.GetKeyDown(KeyCode.W))
        {
            animator.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            animator.enabled = false;
        }

        // Change sprite when spacebar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spriteRenderer.sprite = newSprite;
        }
        // Revert to original sprite when spacebar is released
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}