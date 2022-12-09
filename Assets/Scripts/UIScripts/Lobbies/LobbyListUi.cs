using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;
using System;
using Unity.VisualScripting;

public class LobbyListUi : MonoBehaviour
{
    public static LobbyListUi Instance { get; private set; }

    [SerializeField] private Transform lobbySingleTemp;
    [SerializeField] private Transform container;
    [SerializeField] private Button refresh;
    [SerializeField] private Button createLobby;

    private void Awake()
    {
        Instance = this;

        lobbySingleTemp.gameObject.SetActive(false);

        refresh.onClick.AddListener(RefreshButtonClick);
        createLobby.onClick.AddListener(CreateLobbyButtonClick);
    }

    private void Start()
    {
        LobbyNet.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        LobbyNet.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        LobbyNet.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyNet.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;
    }

    private void LobbyManager_OnKickedFromLobby(object sender, LobbyNet.LobbyEventArgs e)
    {
        Show();
    }
    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
        Show();
    }

    private void LobbyManager_OnJoinedLobby(object sender, LobbyNet.LobbyEventArgs e)
    {
        Hide();
    }

    private void LobbyManager_OnLobbyListChanged(object sender, LobbyNet.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbies);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach(Transform child in container)
        {
            if (child == lobbySingleTemp) continue;

            Destroy(child.gameObject);
        }
        foreach(Lobby lobby in lobbyList)
        {
            Transform lobbySingleTransfrom = Instantiate(lobbySingleTemp, container);
            lobbySingleTransfrom.gameObject.SetActive(true);
            LobbyListSingleUI lobbySingleUI = lobbySingleTransfrom.GetComponent<LobbyListSingleUI>();
            lobbySingleUI.UpdateLobby(lobby);
        }
    }
    
    private void RefreshButtonClick()
    {
        LobbyNet.Instance.RefreshLobbyList();
    }

    private void CreateLobbyButtonClick()
    {
        LobbyCreateUI.Instance.Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }


}
