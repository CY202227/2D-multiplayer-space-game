using TMPro;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerManager : NetworkBehaviour
{
    public TMP_Text Captain;
    public TMP_Text Gunner;
    public TMP_Text Scanner;
    public Button play;
    public Button Weapon1;
    public Button Weapon2;
    public Button Weapon3;
    public bool Scanned;
    public TMP_Text Memo;
    public Button Scan;


    private Dictionary<ulong, TMP_Text> playerReadyTexts = new Dictionary<ulong, TMP_Text>();

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }
    void Update()
    {

        Scanned = GameManager.Instance.temp;
    }

    private new void OnDestroy()
    {
        if (NetworkManager.Singleton)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
        base.OnDestroy(); // Call the base class OnDestroy
    }


    private void OnClientConnected(ulong clientId)
    {
        Debug.Log(clientId);
        // Instantiate a new TMP_Text object for the connecting player
        if (clientId == 0) // Host
        {
            Captain.SetText("Captain Ready");
        }
        else if (clientId == 1) // Gunner (Client ID 1)
        {
            Captain.SetText("Captain Ready");
            Gunner.SetText("Gunner Ready");
        }
        else if (clientId == 2) // Scanner (Client ID 2)
        {
            Captain.SetText("Captain Ready");
            Gunner.SetText("Gunner Ready");
            Scanner.SetText("Scanner Ready");
            play.interactable = true;
        }
    }

    public void ChooseLazer()
    {
        if (Scanned)
        {
            GameManager.Instance.SelectWeapon(30, 10);
        }
        else
        {
            Memo.SetText("You didn't find the enemy");
        }
        // Select the laser weapon and update weapon information

    }

    public void ChooseBullet()
    {
        // Select the bullet weapon and update weapon information
        if (Scanned)
        {
            GameManager.Instance.SelectWeapon(10, 5);
        }
        else
        {
            Memo.SetText("You didn't find the enemy");
        }
    }

    public void ChooseMainBattery()
    {
        if (Scanned)
        {
            GameManager.Instance.SelectWeapon(100, 20);
        }
        else
        {
            Memo.SetText("You didn't find the enemy");
        }
        // Select the main battery weapon and update weapon information
    }
}
