using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class Key : MonoBehaviour
{
    public AudioSource keyAudio;
    public GameObject interactionText;
    public GameObject confirmationText;

    private bool CanInteract = false;

    // Update is called once per frame
    void Update()
    {
        //Check if the E key is being pressed
        if (Input.GetKeyDown(KeyCode.E))
        {
            //And if the player can interact
            if (CanInteract == true)
            {
                //Turn off interaction text
                interactionText.SetActive(false);

                //Run key collected function, set key collected to true
                FindAnyObjectByType<Chest>().KeyCollected();

                //Switch off object
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;

                //Play Key Audio
                keyAudio.Play();

                //Start ConfirmText coroutine
                StartCoroutine(ConfirmText());
            }            
        }
    }

    //Check if the collider has been entered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the object colliding has the Player tag
        if (collision.tag == "Player")
        {            
            //Can interact with object
            CanInteract = true;

            //Display interaction text
            interactionText.SetActive(true);
        }
    }

    //Check if the collider has been exited
    private void OnTriggerExit2D(Collider2D collision)
    {
        //If the object is no longer being collided with by the Player
        if (collision.tag == "Player")
        {
            //Can no longer interact
            CanInteract = false;

            //Can no longer display interaction text
            interactionText.SetActive(false);
        }
    }

    //Function to display text once key is collected
    private IEnumerator ConfirmText()
    {
        //Turn the key collected text on
        confirmationText.SetActive(true);

        //Wait for x seconds
        yield return new WaitForSeconds(2f);

        //Turn the key collected text off
        confirmationText.SetActive(false);
    }

}     
