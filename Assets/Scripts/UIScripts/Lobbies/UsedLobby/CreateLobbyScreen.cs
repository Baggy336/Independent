using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CreateLobbyScreen : MonoBehaviour
{
    // Prefabs to be referenced in the editor
    [SerializeField] private TMP_InputField nameText, maxPlayers;
    [SerializeField] private TMP_Dropdown gameType;

    private void Start()
    {
        // The lobby host sets the game type and options for the lobby
        SetOptions(gameType, Constants.GameTypes);

        void SetOptions(TMP_Dropdown dropdown, IEnumerable<string> values)
        {
            dropdown.options = values.Select(type => new TMP_Dropdown.OptionData { text = type }).ToList();
        }
    }

    public static event Action<LobbyData> LobbyCreated;

    // Set the lobby data to be passed to the prefabs of the lobby
    public void OnCreateClicked()
    {
        var lobbyData = new LobbyData
        {
            Name = nameText.text,
            MaxPlayers = int.Parse(maxPlayers.text),
            Type = gameType.value
        };

        LobbyCreated?.Invoke(lobbyData);
    }
}

public struct LobbyData
{
    public string Name;
    public int MaxPlayers;
    public int Type;
}
