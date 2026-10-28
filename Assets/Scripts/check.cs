using UnityEngine;

public class check : MonoBehaviour
{
    private SpriteRenderer renderer;
    public Sprite sprite_on;
    public Sprite sprite_off;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        renderer.sprite = sprite_off;
    }

    public void turnon() {
        renderer.sprite = sprite_on;
    }
}
