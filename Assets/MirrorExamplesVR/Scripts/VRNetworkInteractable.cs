using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit;

public class VRNetworkInteractable : NetworkBehaviour
{
    private Rigidbody rb;
    //private XRGrabInteractable xRGrabInteractable;
    public VRWeapon vrWeapon;

    private void Start()
    {
        // we should technically make them public and set in inspector for less startup calls, but im feeling lazy.
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        //if (xRGrabInteractable == null)
        //{
        //    xRGrabInteractable = GetComponent<XRGrabInteractable>();
        //}
    }

    public void EventPickup()
    {
        //if (isOwned == false)
        //{
            ResetInteractableVelocity();
            CmdPickup(VRStaticVariables.handValue);
        //}
    }


    public void EventDrop()
    {
        // technically dont need to pass auth when dropping, only remove auth when another player grabs, or current grabber disconnects
        // doing it this way stops jitter when passing auth of moving objects (due to ping difference of positions)
        ///*
        if (isOwned == true)
        {
            //ResetInteractableVelocity();
            if (vrWeapon)
            {
                CmdDrop(VRStaticVariables.handValue);
            }
            //rb.velocity,rb.angularVelocity
        }
        //*/
    }

    [Command(requiresAuthority = false)]
    public void CmdPickup(int _hand, NetworkConnectionToClient sender = null)
    {
        //Debug.Log("Mirror CmdPickup owner set to: " + sender.identity);

        ResetInteractableVelocity();

        if (sender != netIdentity.connectionToClient)
        {
            netIdentity.RemoveClientAuthority();
            netIdentity.AssignClientAuthority(sender);
        }

        if (vrWeapon)
        {
            vrWeapon.vrNetworkPlayerScript = sender.identity.GetComponent<VRNetworkPlayerScript>();
            //vrWeapon.SetTextAmmo();
            if (_hand == 2)
            {
                vrWeapon.vrNetworkPlayerScript.leftHandObject = this.netIdentity;
            }
            else
            {
                vrWeapon.vrNetworkPlayerScript.rightHandObject = this.netIdentity;
            }
            
        }
    }

    ///*
    [Command(requiresAuthority = false)]
    public void CmdDrop(int _hand, NetworkConnectionToClient sender = null) //Vector3 _velocity, Vector3 _angualarVelocity,
    {
        //Debug.Log("Mirror CmdDrop owner removed from: " + sender.identity);

        //ResetInteractableVelocity();

        //if (netIdentity.connectionToClient != null)
        //{
        //    netIdentity.RemoveClientAuthority();
        //}
        //netIdentity.AssignClientAuthority(NetworkServer.connections[0]);

        //rb.velocity = _velocity;
        //rb.angularVelocity = _angualarVelocity;
        //Debug.Log("CmdDrop assigned to host : " + NetworkServer.connections[0].identity);

        if (vrWeapon && vrWeapon.vrNetworkPlayerScript)
        {
            if (_hand == 2)
            {
                vrWeapon.vrNetworkPlayerScript.leftHandObject = null;
            }
            else
            {
                vrWeapon.vrNetworkPlayerScript.rightHandObject = null;
            }
            // vrWeapon.vrNetworkPlayerScript = null;
        }
    }
    //*/

    private void ResetInteractableVelocity()
    {
        // Unitys interactable types need some adjustments to stop them behaving weird over network
        // Without this you may notice some pickups rapidly fall through the floor
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // we can use this check apply different behaviour depending on interactable type
        // if (xRGrabInteractable.movementType == XRBaseInteractable.MovementType.VelocityTracking) { }
    }

    public VRNetworkPlayerScript vrNetworkPlayerScript;

    private void Update()
    {
        if (vrNetworkPlayerScript == null)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            EventPickup();
            //vrNetworkPlayerScript = vrNetworkPlayerScript.GetComponent<VRNetworkPlayerScript>();
           // vrNetworkPlayerScript.rightHandObject = this.netIdentity;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            EventDrop();
        }
    }
}
