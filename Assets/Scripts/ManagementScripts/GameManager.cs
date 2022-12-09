using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

// Takes care of spawning and managing tanks
public class GameManager : MonoBehaviour
{
    // Public variables to be referenced in editor
    public int roundsToWin = 5;
    public float startDelay = 3f;
    public float endDelay = 3f;
    public GameObject tank;
    public TankManager[] tanks;

    // Private variables that are constant in this script
    private int roundNumber;
    private WaitForSeconds startWait;
    private WaitForSeconds endWait;
    private TankManager roundWinner;
    private TankManager gameWinner;

    private void Start()
    {
        startWait = new WaitForSeconds(startDelay);
        endWait = new WaitForSeconds(endDelay);

        SpawnTanks();

        StartCoroutine(GameLoop());
    }

    // Spawn a tank for each tank in the manager
    private void SpawnTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].instance = Instantiate(tank, tanks[i].spawnPoint.position, tanks[i].spawnPoint.rotation) as GameObject;
            tanks[i].player = i + 1;
            tanks[i].Setup();
        }
    }

    // Coroutine to handle each round
    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(RoundStarting());
        yield return StartCoroutine(RoundPlaying());
        yield return StartCoroutine(RoundEnding());

        if (gameWinner != null)
        {
            SceneManager.LoadScene("Sandbox");
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }

    // Each tank is set to it's spawnpoint
    // Controls are disabled for the duration of the start timer
    private IEnumerator RoundStarting()
    {
        ResetTanks();
        DisableControl();

        roundNumber++;
        yield return startWait;
    }

    // Controls are enabled for the time it takes for only 1 player to be alive
    private IEnumerator RoundPlaying()
    {
        EnableControl();

        while (!OneTankLeft())
        {
            yield return null;
        }

        // If more than one player is alive, return nothing yet
        yield return null;
    }

    // Declare the round winner after taking away controls. If the round winner has the amount of rounds to win, end the game
    private IEnumerator RoundEnding()
    {
        DisableControl();

        roundWinner = null;

        roundWinner = GetRoundWinner();
        if (roundWinner != null) roundWinner.wins++;

        gameWinner = GetGameWinner();

        yield return endWait;
    }

    // Get the only active tank game object, add a round win
    private TankManager GetRoundWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf)
                return tanks[i];
        }

        return null;
    }

    private TankManager GetGameWinner()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].wins == roundsToWin) return tanks[i];
        }
        return null;
    }

    private bool OneTankLeft()
    {
        int numOfTanks = 0;

        for (int i = 0; i < tanks.Length; i++)
        {
            if (tanks[i].instance.activeSelf) numOfTanks++;
        }

        return numOfTanks <= 1;
    }

    private void ResetTanks()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].ResetTank();
        }
    }

    private void DisableControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].DisableControl();
        }
    }

    private void EnableControl()
    {
        for (int i = 0; i < tanks.Length; i++)
        {
            tanks[i].EnableControl();
        }
    }
}
