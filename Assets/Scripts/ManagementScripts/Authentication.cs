using UnityEngine;
using UnityEngine.SceneManagement;

// Responsible for loading the initial scene
public class AuthenticationManager : MonoBehaviour
{
    // Syncronize the client to the server
    public async void LogIn()
    {
        using (new Load("Logging you in..."))
        {
            // Run the authentication process
            await Authenticator.Login();
            SceneManager.LoadSceneAsync("Lobby");
        }
    }
}
