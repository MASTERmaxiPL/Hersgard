using UnityEngine;

public class Interactible : MonoBehaviour
{
    public bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision !=  null && collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }

    public virtual void Interact()
    {
        
    }
    
}
