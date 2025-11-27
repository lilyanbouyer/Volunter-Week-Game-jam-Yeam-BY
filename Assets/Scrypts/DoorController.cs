using UnityEngine;

// Component to attach to individual doors for custom settings
public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    // Use Vector3 to allow full rotation on all axes (x, y, z)
    public Vector3 OpenRotation = new Vector3(0f, 90.0f, 0f);
    public Vector3 CloseRotation = Vector3.zero;
    public float smoothSpeed = 2f;
    
    private bool isOpening = false;
    private Quaternion targetRotation;

    void Start()
    {
    }

    public bool TryOpen()
    {
        isOpening = !isOpening;
        return true;
    }

    public void Update()
    {
        Vector3 targetEuler = isOpening ? OpenRotation : CloseRotation;
        targetRotation = Quaternion.Euler(targetEuler);
        
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            smoothSpeed * Time.deltaTime
        );
    }

    public bool IsOpen()
    {
        return isOpening;
    }
}
