using System;
using UnityEngine;
using Unity.Netcode;

// Referenced by the game manager to make tanks behave a certain way
[Serializable]
public class TankManager : TankSingleton<TankManager>
{
    private NetworkVariable<int> playersInGame = new NetworkVariable<int>();

    public int PlayersInGame
    {
        get
        {
            return playersInGame.Value;
        }
    }

    public Color playerColor;
    public Transform spawnPoint;
    [HideInInspector] public int player;
    [HideInInspector] public GameObject instance;
    [HideInInspector] public int wins;

    private TankMovement move;
    private FireProjectile shoot;

    // Add a player to the game when this script starts
    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (IsServer)
            {
                playersInGame.Value++;
            }
        };

        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (IsServer)
            {
                playersInGame.Value--;
            }
        };
    }

    // Setup necessary tank scripts and render a color
    public void Setup()
    {
        move = instance.GetComponent<TankMovement>();
        shoot = instance.GetComponent<FireProjectile>();
        move.playerNumber = player;

        MeshRenderer[] rendering = instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < rendering.Length; i++)
        {
            rendering[i].material.color = playerColor;
        }
    }

    public void DisableControl()
    {
        move.enabled = false;
        shoot.enabled = false;
    }

    public void EnableControl()
    {
        move.enabled = true;
        shoot.enabled = true;
    }

    // Set a tank's location back to it's spawnpoint
    public void ResetTank()
    {
        instance.transform.position = spawnPoint.position;
        instance.transform.rotation = spawnPoint.rotation;

        instance.SetActive(false);
        instance.SetActive(true);
    }
}
