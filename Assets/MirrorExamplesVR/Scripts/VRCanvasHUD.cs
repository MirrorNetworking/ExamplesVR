using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using Mirror.Discovery;
using TMPro;
using UnityEngine.SceneManagement;

public class VRCanvasHUD : MonoBehaviour
{
    // this will check for games to join, if non, start host.
    public bool alwaysAutoStart = false;
    public VRNetworkDiscovery networkDiscovery;
    readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
    private TouchScreenKeyboard keyboard;
    private int keyboardStatus = 0;

    // UI
    public GameObject PanelStart, PanelStop;
    public Button buttonHost, buttonServer, buttonClient, buttonStop, buttonAuto;
    public Text infoText;
    // legacy inputfield interaction does not auto bring up a keyboard on headset builds, use tmp.
    public TMP_InputField inputFieldAddress, inputFieldPlayerName;

    private void Start()
    {
        //Make sure to attach these Buttons in the Inspector
        buttonHost.onClick.AddListener(ButtonHost);
        buttonServer.onClick.AddListener(ButtonServer);
        buttonClient.onClick.AddListener(ButtonClient);
        buttonStop.onClick.AddListener(ButtonStop);
        buttonAuto.onClick.AddListener(ButtonAuto);

        //Update the canvas text if you have manually changed network managers address from the game object before starting the game scene
        inputFieldAddress.text = NetworkManager.singleton.networkAddress;

        //Adds a listener to the input field and invokes a method when the value changes.
        inputFieldAddress.onValueChanged.AddListener(delegate { OnValueChangedAddress(); });
        inputFieldPlayerName.onValueChanged.AddListener(delegate { OnValueChangedName(); });

        if (networkDiscovery == null)
        { networkDiscovery = GameObject.FindObjectOfType<VRNetworkDiscovery>(); }

        if (networkDiscovery == null)
        { networkDiscovery = GameObject.FindObjectOfType<VRNetworkDiscovery>(); }

        // skips waiting for users to press ui button
        if (alwaysAutoStart)
        {
            StartCoroutine(Waiter());
        }
    }

    public IEnumerator Waiter()
    {
        infoText.text = "Discovering servers..";
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
        // we have set this as 3.1 seconds, default discovery scan is 3 seconds, allows some time if host and client are started at same time
        yield return new WaitForSeconds(3.1f);
        if (discoveredServers == null || discoveredServers.Count <= 0)
        {
            infoText.text = "No Servers found, starting as Host.";
            yield return new WaitForSeconds(1.0f);
            discoveredServers.Clear();
           // NetworkManager.singleton.onlineScene = SceneManager.GetActiveScene().name;
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }
    }

    void Connect(ServerResponse info)
    {
        infoText.text = "Connecting to: " + info.serverId;
        networkDiscovery.StopDiscovery();
        NetworkManager.singleton.StartClient(info.uri);
    }

    public void OnDiscoveredServer(ServerResponse info)
    {
        discoveredServers[info.serverId] = info;
        Connect(info);
    }

    public void ButtonHost()
    {
        SetupInfoText("Starting as host");
        discoveredServers.Clear();
        //NetworkManager.singleton.onlineScene = SceneManager.GetActiveScene().name;
        NetworkManager.singleton.StartHost();
        networkDiscovery.AdvertiseServer();

    }

    public void ButtonServer()
    {
        SetupInfoText("Starting as server.");
        discoveredServers.Clear();
       // NetworkManager.singleton.onlineScene = SceneManager.GetActiveScene().name;
        NetworkManager.singleton.StartServer();
        networkDiscovery.AdvertiseServer();

    }

    public void ButtonClient()
    {
        SetupInfoText("Starting as client.");
        discoveredServers.Clear();
        networkDiscovery.StartDiscovery();
    }

    public void ButtonStop()
    {
        SetupInfoText("Stopping.");
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }
        networkDiscovery.StopDiscovery();
        // we need to call setup canvas a second time in this function for it to update the abovee changes
        SetupCanvas();
    }

    public void ButtonAuto()
    {
        SetupInfoText("Auto Starting.");
        StartCoroutine(Waiter());
    }

    // manually call canvas changes for performance, can lazily be done via Update()
    public void SetupCanvas()
    {
        // Here we will dump majority of the canvas UI

        if (NetworkManager.singleton == null)
        {
            SetupInfoText("NetworkManager null");
            return;
        }

        // check network status, and show required UI
        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (NetworkClient.active)
            {
                PanelStart.SetActive(false);
                PanelStop.SetActive(true);
            }
            else
            {
                PanelStart.SetActive(true);
                PanelStop.SetActive(false);
            }
        }
        else
        {
            PanelStart.SetActive(false);
            PanelStop.SetActive(true);
        }
    }

    // useful status info to display on screen
    public void SetupInfoText(string _info)
    {
        infoText.text = _info;
        SetupCanvas();
    }

    // Invoked when the value of the text field changes.
    public void OnValueChangedAddress()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }

    // touchscreen keyboard can be weird, so we have an option to open it manually
    public void ButtonKeyboard(int _status)
    {
        if (TouchScreenKeyboard.isSupported)
        {
            keyboardStatus = _status;
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, "", 15);
            // Open(string text, TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default, bool autocorrection = true, bool multiline = false, bool secure = false, bool alert = false, string textPlaceholder = "", int characterLimit = 0);
        }
        else
        {
            Debug.Log("Touchscreen keyboard not supported.");
        }
    }

    private void Update()
    {
        if (TouchScreenKeyboard.isSupported && keyboard != null && keyboard.active && keyboard.text != "")
        {
            if (keyboardStatus == 1)
            {
                inputFieldAddress.text = keyboard.text;
            }
            else if (keyboardStatus == 2)
            {
                inputFieldPlayerName.text = keyboard.text;
                VRStaticVariables.playerName = inputFieldPlayerName.text;
            }
        }
    }

    // Invoked when the value of the text field changes.
    public void OnValueChangedName()
    {
        VRStaticVariables.playerName = inputFieldPlayerName.text;
    }
}

