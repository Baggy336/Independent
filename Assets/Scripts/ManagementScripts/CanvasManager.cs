using System;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Make sure there is only one Canvas Manager in the scene
    public static CanvasManager Instance;

    // Variables to be set in the inspector with Prefabs
    [SerializeField] private CanvasGroup _loader;
    [SerializeField] private float fadeDur;
    [SerializeField] private TMP_Text loadText, errorTxt;

    private void Awake()
    {
        // Set up singleton instance of this script
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Toggle(false, instant: true);
    }

    // Toggle visibility of the canvas depending on the scene
    public void Toggle(bool on, string text = null, bool instant = false)
    {
        loadText.text = text;
        _loader.gameObject.SetActive(on);
    }

    public void ShowError(string error)
    {
        errorTxt.text = error;
    }

    private const CursorMode CursorMode = UnityEngine.CursorMode.Auto;
    private readonly Vector2 hotSpot = Vector2.zero;
    private void SetCursor(Texture2D tex) => Cursor.SetCursor(tex, hotSpot, CursorMode);
}

// Load the scene initally, dispose of the scene on exit
public class Load : IDisposable
{
    public Load(string text)
    {
        CanvasManager.Instance.Toggle(true, text);
    }

    public void Dispose()
    {
        CanvasManager.Instance.Toggle(false);
    }
}
