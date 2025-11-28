using UnityEngine;
using UnityEngine.InputSystem;

public class Interactions : MonoBehaviour
{
    public Camera playerCamera;
    
    // Nouveau Input System
    public InputActionAsset actionsAsset;
    private InputAction interactAction;
    private bool interactPressed = false;
    public NewMonoBehaviourScript movementsScript;
    public Transform playerHand;
    private GameObject currentItem = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialisation des actions
        if (actionsAsset == null)
        {
            Debug.LogError("Assigne InputActionAsset dans l'inspecteur !");
            return;
        }
        interactAction = actionsAsset.FindAction("Interact");
        if (interactAction != null)
        {
            interactAction.Enable();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (interactAction != null && interactAction.IsPressed()) {
            if (!interactPressed) {
                interactPressed = true;
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 4f)) {
                    ControllerColliderHit(hit);
                }
            }
        }
        else {
            interactPressed = false;
        }
        // else if (Input.GetKeyDown(KeyCode.F12)) {
        //    SceneManager.LoadScene("Test_Scene");
        // }
    }

    private void ControllerColliderHit(RaycastHit hit)
    {
        if (hit.transform.gameObject.CompareTag("Door"))
            OpenCloseDoor(hit);
        if (hit.transform.gameObject.CompareTag("NPC"))
        {
            conversation_starter convo = hit.transform.GetComponent<conversation_starter>();
            if (convo != null)
            {
                movementsScript.isdialogueActive = true;
                convo.StartConversation();
            }
        }
        else if (hit.transform.gameObject.CompareTag("Pickup"))    
            PickupItem(hit);
        /*
        else if (hit.transform.gameObject.CompareTag("Placement") && itemPickup.GetCurrentItem()) {
            if (hit.transform.childCount == 0) {
                hit.transform.GetComponent<ItemPlacement>().SetCurrentItem(itemPickup.GetCurrentItem());
                hit.transform.GetComponent<ItemPlacement>().PlaceItem(hit);
                itemPickup.SetCurrentItem(null);
            }
        } else if (itemPickup.GetCurrentItem())
            itemPickup.DropItem();
        else if (hit.transform.gameObject.CompareTag("Switch"))
            lightController.ToggleLightWithend(hit.transform.gameObject.name, hit.transform.gameObject);*/
    }

    public void OpenCloseDoor(RaycastHit hit)
    {
        Door door = hit.transform.GetComponent<Door>();
        
        if (door == null)
        {
            Debug.LogWarning("Clicked object doesn't have a Door component!");
            return;
        }

        door.TryOpen();
    }

    public void PickupItem(RaycastHit hit)
    {
        GameObject targetItem = hit.transform.gameObject;
        /*if(targetItem.transform.parent != null && targetItem.transform.parent.CompareTag("Placement"))
        {
            ItemPlacement placement = targetItem.transform.parent.GetComponent<ItemPlacement>();
            if(placement.enigmeChecker != null && placement.IsGoodItem(targetItem.transform.parent.name, targetItem.name))
            {
                placement.enigmeChecker.DecrementCurrent();
            }
            targetItem.transform.SetParent(null);
        }*/
        currentItem = targetItem;
        currentItem.transform.SetParent(playerHand);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localRotation = Quaternion.Euler(0, 180, 0);
        currentItem.GetComponent<Rigidbody>().isKinematic = true;
        currentItem.GetComponent<Rigidbody>().useGravity = true;
        if (currentItem.GetComponent<Collider>() != null)
            currentItem.GetComponent<Collider>().enabled = false;
        //if (hit.transform.parent.GetComponent<ItemPlacement>())
        //    hit.transform.parent.GetComponent<ItemPlacement>().SetCurrentItem(currentItem);
}

    public void DropItem()
    {
        if (false) {
        currentItem.transform.SetParent(null);
        currentItem.GetComponent<Rigidbody>().isKinematic = false;
        currentItem.GetComponent<Rigidbody>().useGravity = true;
        if (currentItem.GetComponent<Collider>() != null)
            currentItem.GetComponent<Collider>().enabled = true;
        currentItem = null;
        }
    }

}
