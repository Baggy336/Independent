using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

// Active while in a lobby with other players
public class RoomScreen : MonoBehaviour
{
    // Prefabs to be set within the editor
    [SerializeField] private LobbyPlayerPanel playerPanel;
    [SerializeField] private Transform panelParent;
    [SerializeField] private TMP_Text waitPlaceholder;
    [SerializeField] private GameObject startGame, playerReady;

    private readonly List<LobbyPlayerPanel> playerPanels = new();
    private bool arePlayersReady;
    private bool localReady;

    // Event for starting the game
    public static event Action StartPressed;

    // clear any lobbies that persist, set up the event systems
    private void OnEnable()
    {
        foreach (Transform child in panelParent) Destroy(child.gameObject);
        playerPanels.Clear();

        LobbyManager.LobbyPlayersUpdated += NetworkLobbyPlayersUpdated;
        MatchmakingService.CurrentLobbyRefreshed += OnCurrentLobbyRefreshed;
        startGame.SetActive(false);
        playerReady.SetActive(false);

        localReady = false;
    }

    private void OnDisable()
    {
        LobbyManager.LobbyPlayersUpdated -= NetworkLobbyPlayersUpdated;
        MatchmakingService.CurrentLobbyRefreshed -= OnCurrentLobbyRefreshed;
    }

    public static event Action LobbyLeft;

    public void OnLeaveLobby()
    {
        LobbyLeft?.Invoke();
    }

    // Get the player data, and update profiles
    private void NetworkLobbyPlayersUpdated(Dictionary<ulong, bool> players)
    {
        var allActivePlayerIds = players.Keys;

        // Remove all inactive panels
        var toDestroy = playerPanels.Where(p => !allActivePlayerIds.Contains(p.PlayerId)).ToList();
        foreach (var panel in toDestroy)
        {
            playerPanels.Remove(panel);
            Destroy(panel.gameObject);
        }

        foreach (var player in players)
        {
            var currentPanel = playerPanels.FirstOrDefault(p => p.PlayerId == player.Key);
            if (currentPanel != null)
            {
                if (player.Value) currentPanel.SetReady();
            }
            else
            {
                var panel = Instantiate(playerPanel, panelParent);
                panel.Init(player.Key);
                playerPanels.Add(panel);
            }
        }

        startGame.SetActive(NetworkManager.Singleton.IsHost && players.All(p => p.Value));
        playerReady.SetActive(!localReady);
    }


    // Events set in the on click

    private void OnCurrentLobbyRefreshed(Lobby lobby)
    {
        waitPlaceholder.text = $"Waiting on players... {lobby.Players.Count}/{lobby.MaxPlayers}";
    }

    public void OnReadyClicked()
    {
        playerReady.SetActive(false);
        localReady = true;
    }

    public void OnStartClicked()
    {
        StartPressed?.Invoke();
    }
}
