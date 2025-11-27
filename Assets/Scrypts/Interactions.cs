using UnityEngine;
using UnityEngine.InputSystem;

public class Interactions : MonoBehaviour
{
    public Camera playerCamera;
    
    // Nouveau Input System
    public InputActionAsset actionsAsset;
    private InputAction interactAction;
    private bool interactPressed = false;

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
        /*else if (hit.transform.gameObject.CompareTag("Pickup") && !itemPickup.GetCurrentItem())    
            itemPickup.PickupItem(hit);
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

}
