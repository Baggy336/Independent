using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] private Vector2 dir = new(0, 0.01f);
    private RawImage img;

    private void Awake()
    {
        img = GetComponent<RawImage>();
    }

    private void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + dir * Time.fixedDeltaTime, img.uvRect.size);
    }
}
