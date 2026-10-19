using UnityEngine;

public class check : MonoBehaviour
{
    private SpriteRenderer s;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        s = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnon() {
        s.enabled = true;
    }
}
