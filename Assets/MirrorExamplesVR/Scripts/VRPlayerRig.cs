using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerRig : MonoBehaviour
{
    public Transform rHandTransform;
    public Transform lHandTransform;
    public Transform headTransform;

    public Transform canvasUIPosition;

    public GameObject damageTriggerR;
    public GameObject damageTriggerL;

    public VRNetworkPlayerScript localVRNetworkPlayerScript;
    public AudioSource audioCue;

    // switch to Late/Fixed Update if weirdness happens
    private void Update()
    {
        if (localVRNetworkPlayerScript)
        {
            // presuming you want a head object to sync, optional, same as hands.
            localVRNetworkPlayerScript.headTransform.position = this.headTransform.position;
            localVRNetworkPlayerScript.headTransform.rotation = this.headTransform.rotation;
            localVRNetworkPlayerScript.rHandTransform.position = this.rHandTransform.position;
            localVRNetworkPlayerScript.rHandTransform.rotation = this.rHandTransform.rotation;
            localVRNetworkPlayerScript.lHandTransform.position = this.lHandTransform.position;
            localVRNetworkPlayerScript.lHandTransform.rotation = this.lHandTransform.rotation;
        }
    }

    // Simple movement for testing on PC/Editor/Controller joystick
    // helps if you cannot use headset directly in Unity Editor  (W A S D)
    private void FixedUpdate()
    {
        HandleMovement();
        HandleInput();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 100.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0, moveX, 0);
        transform.Translate(0, 0, moveZ);
    }

    private void HandleInput()
    {
        // take input from focused window only
        if (!Application.isFocused)
            return;

        // input for local player
        if (localVRNetworkPlayerScript && localVRNetworkPlayerScript.isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioCue.Play();
                localVRNetworkPlayerScript.CmdFire(0);
            }
        }

        //bool pressed;
        //rightHand.inputDevice.IsPressed(button, out pressed);

        //if (pressed)
        //{
        //    Debug.Log("Hello - " + button);
        //    audioCue.Play();
        //    localVRNetworkPlayerScript.CmdFire();
        //}

        if (localVRNetworkPlayerScript && localVRNetworkPlayerScript.isLocalPlayer)
        {

            if (rightHand.GetComponent<ActionBasedController>().activateAction.action.ReadValue<float>() > 0.5f)
            {
                //Debug.Log("Mirror R trigger pressed");
                audioCue.Play();
                localVRNetworkPlayerScript.CmdFire(1);
            }
            if (leftHand.GetComponent<ActionBasedController>().activateAction.action.ReadValue<float>() > 0.5f)
            {
                //Debug.Log("Mirror L trigger pressed");
                audioCue.Play();
                localVRNetworkPlayerScript.CmdFire(2);
            }
        }
    }
    public XRBaseController rightHand;
    public XRBaseController leftHand;
    public InputHelpers.Button buttonR;
    public InputHelpers.Button buttonL;

    public InputActionReference shootButtonRightHand;
    public InputActionReference shootButtonLeftHand;

    //private void OnEnable()
    //{
    //    shootButtonRightHand.asset.Enable();
    //    shootButtonLeftHand.asset.Enable();
    //    testReference.asset.Enable();
    //}
    //private void OnDisable()
    //{
    //    shootButtonRightHand.asset.Disable();
    //    shootButtonLeftHand.asset.Disable();
    //    testReference.asset.Disable();
    //}

    private void InputActionShootButton(InputAction.CallbackContext context)
    {
        //Debug.Log("Mirror InputActionCombatActivated: " + context);
        audioCue.Play();
        localVRNetworkPlayerScript.CmdFire(0);
    }

    public InputActionReference testReference = null;

    private void Start()
    {
        //shootButtonLeftHand.action.performed += InputActionShootButton;
        //shootButtonRightHand.action.started += InputActionShootButton;
        //testReference.action.performed += DoChangeThing;
        //var inputDevices = new List<UnityEngine.XR.InputDevice>();
        //UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        //foreach (var device in inputDevices)
        //{
        //    Debug.Log(string.Format("Device found with name '{0}' and role '{1}'", device.name, device.role.ToString()));
        //}
    }

    private void DoChangeThing(InputAction.CallbackContext context)
    {
        audioCue.Play();
        localVRNetworkPlayerScript.CmdFire(0);
    }

    public void SetHandStatus(int _status)
    {
        //Debug.Log("Mirror SetHandStatus: " + _status);
        VRStaticVariables.handValue = _status;
    }
}