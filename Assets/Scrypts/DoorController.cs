using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Camera playerHead; // Caméra du joueur
    public float doorOpenAngle = 90.0f;
    public float doorCloseAngle = 0.0f;
    public float smooth = 2.0f; // Augmenté pour une transition plus fluide

    private bool isOpening = false; // État de la porte
    private GameObject currentDoor = null;

    public void OpenCloseDoor(RaycastHit hit)
    {
        currentDoor = hit.transform.gameObject;
        isOpening = !isOpening; // Inverser l'état de la porte

    }

    public GameObject GetCurrentDoor()
    {
        return currentDoor;
    }

    public void HandleDoor()
    {
        if (currentDoor)
        {
            // Définir l'angle cible
            float targetAngle = isOpening ? doorOpenAngle : doorCloseAngle;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

            // Appliquer la rotation progressivement
            currentDoor.transform.localRotation = Quaternion.Slerp(
                currentDoor.transform.localRotation,
                targetRotation,
                smooth * Time.deltaTime
            );
        }
    }
}
