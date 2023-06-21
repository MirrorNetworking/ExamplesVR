using UnityEngine;
using Mirror;

public class VRNetworkPlayerScript : NetworkBehaviour
{
    public Transform rHandTransform;
    public Transform lHandTransform;
    public Transform headTransform;

    public VRPlayerRig vrPlayerRig;

    public override void OnStartLocalPlayer()
    {
        // create a link to local vr rig, so that rig can sync to our local network players transforms
        vrPlayerRig = GameObject.FindObjectOfType<VRPlayerRig>();
        vrPlayerRig.localVRNetworkPlayerScript = this;

        // we dont need to see our network representation of hands, or our own headset, it also covers camera without using layers or some repositioning
        rHandTransform.gameObject.SetActive(false);
        lHandTransform.gameObject.SetActive(false);
        headTransform.gameObject.SetActive(false);
    }
}
