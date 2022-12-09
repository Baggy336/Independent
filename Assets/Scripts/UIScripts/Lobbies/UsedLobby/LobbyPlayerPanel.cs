using TMPro;
using UnityEngine;

// Some convenience funtions for the player panel prefab
public class LobbyPlayerPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text playerName, playerState;

    public ulong PlayerId { get; private set; }

    public void Init(ulong playerId)
    {
        PlayerId = playerId;
        playerName.text = $"Player {playerId}";
    }

    public void SetReady()
    {
        playerState.text = "Ready";
        playerState.color = Color.blue;
    }
}
