using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// makes sure that whenever we add a specific object adds up that component
[RequireComponent(typeof(CircleCollider2D))]

public class Item : MonoBehaviour {

    public AudioClip pickUpSound;

    private void Reset() {
        GetComponent<Collider2D>().isTrigger = true;
        gameObject.layer = 10;
    }

    public enum InteractionType {
        NONE,
        PickUp,
        Examine
    }

    public InteractionType type;

    public void Interact() {
        switch(type) {
            case InteractionType.PickUp:
                AudioSource.PlayClipAtPoint(pickUpSound, transform.position);

                FindObjectOfType<InteractionSystem>().PickUpItem(gameObject);
                gameObject.SetActive(false);
                Debug.Log("PICK UP");
                break;
            case InteractionType.Examine:
                Debug.Log("EXAMINE");
                break;
            default:
                Debug.Log("NULL");
                break;
        }
    }
}
