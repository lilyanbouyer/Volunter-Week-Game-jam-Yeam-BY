using UnityEngine;

// Component to attach to individual doors for custom settings
public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    // Use Vector3 to allow full rotation on all axes (x, y, z)
    public Vector3 OpenRotation = new Vector3(0f, 90.0f, 0f);
    public Vector3 CloseRotation = Vector3.zero;
    public float smoothSpeed = 2f;
    
    [Header("Invert Axes")]
    [Tooltip("Invert the opening direction on X axis relative to CloseRotation")]
    public bool invertX = false;
    [Tooltip("Invert the opening direction on Y axis relative to CloseRotation")]
    public bool invertY = false;
    [Tooltip("Invert the opening direction on Z axis relative to CloseRotation")]
    public bool invertZ = false;
    
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
        Vector3 targetEuler = isOpening ? GetEffectiveOpenRotation() : CloseRotation;
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

    // Compute OpenRotation with optional per-axis inversion around CloseRotation
    private Vector3 GetEffectiveOpenRotation()
    {
        Vector3 effective = OpenRotation;
        // If invert on axis, mirror OpenRotation around CloseRotation on that axis
        if (invertX)
            effective.x = CloseRotation.x - (OpenRotation.x - CloseRotation.x);
        if (invertY)
            effective.y = CloseRotation.y - (OpenRotation.y - CloseRotation.y);
        if (invertZ)
            effective.z = CloseRotation.z - (OpenRotation.z - CloseRotation.z);
        return effective;
    }
}
