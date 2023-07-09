using UnityEngine;

public class VRLookAtCamera : MonoBehaviour
{
    // There may be a few different ways/axis we want objects to look at cameera
    // Stick them all in one script with a number to trigger which type
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
