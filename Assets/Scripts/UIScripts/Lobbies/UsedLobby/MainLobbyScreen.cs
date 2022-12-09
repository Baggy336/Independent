using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainLobbyScreen : MonoBehaviour
{
    // Prefabs to be referenced in editor
    [SerializeField] private LobbyRoomPanel lobbyPrefab;
    [SerializeField] private Transform lobbyParent;
    [SerializeField] private GameObject noLobbiesReady;
    [SerializeField] private float refreshTimer = 2;

    private readonly List<LobbyRoomPanel> currentReadyLobbies = new();
    private float refreshCooldown;

    private void Update()
    {
        if (Time.time >= refreshCooldown) FetchLobbies();
    }

    private void OnEnable()
    {
        foreach (Transform child in lobbyParent) Destroy(child.gameObject);
        currentReadyLobbies.Clear();
    }

    // Refresh the lobby list from the matchmaking services
    private async void FetchLobbies()
    {
        try
        {
            refreshCooldown = Time.time + refreshTimer;
            var allLobbies = await MatchmakingService.GatherLobbies();

            var lobbyIds = allLobbies.Where(l => l.HostId != Authenticator.PlayerId).Select(l => l.Id);
            var notActive = currentReadyLobbies.Where(l => !lobbyIds.Contains(l.Lobby.Id)).ToList();

            foreach (var panel in notActive)
            {
                Destroy(panel.gameObject);
                currentReadyLobbies.Remove(panel);
            }

            foreach (var lobby in allLobbies)
            {
                var current = currentReadyLobbies.FirstOrDefault(p => p.Lobby.Id == lobby.Id);
                if (current != null)
                {
                    current.UpdateDetails(lobby);
                }
                else
                {
                    var panel = Instantiate(lobbyPrefab, lobbyParent);
                    panel.Init(lobby);
                    currentReadyLobbies.Add(panel);
                }
            }

            noLobbiesReady.SetActive(!currentReadyLobbies.Any());
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
