using UnityEngine;

public class VRLookAtCamera : MonoBehaviour
{
    private Transform mainCameraTransform;
    public int lookAtMethod = 1;

    void LateUpdate()
    {
        if (mainCameraTransform == null)
        {
            mainCameraTransform = Camera.main.transform;
        }
        else
        {
            // transform.LookAt(mainCameraTransform, Vector3.down);
            if (lookAtMethod == 1)
            {
                this.transform.rotation = Quaternion.LookRotation(this.transform.position - mainCameraTransform.position);
            }
            else if(lookAtMethod == 2)
            {
                transform.forward = mainCameraTransform.forward;
            }
            else if (lookAtMethod == 3)
            {
                transform.LookAt(mainCameraTransform);
            }
        }
    }
}
