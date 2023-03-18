using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem : MonoBehaviour {

    public Transform detectionPoint;
    private const float detectionRadius = 1f;
    public LayerMask detectionLayer;
    public AudioClip pickUpSound;

    public List<GameObject> pickedItems = new List<GameObject>();

    // cached trigger object
    public GameObject detectedObject; // by default NULL

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(detectionPoint.position, detectionRadius);
    }

    // Update is called once per frame
    void Update() {
        if (DetectObject()) {
            detectedObject.GetComponent<Item>().Interact();
        }
    }


    //public bool InteractInput() {
    //    return Input.GetKeyDown(KeyCode.E);
    //}

    // interacting between 2 scripts
    public bool DetectObject() {
        Collider2D obj = Physics2D.OverlapCircle(detectionPoint.position, detectionRadius, detectionLayer);

        if (obj == null) {
            detectedObject = null;
            return false;
        } else {
            detectedObject = obj.gameObject;
            return true;
        }
    }

    public void PickUpItem(GameObject item) {
        pickedItems.Add(item);
    }
}
