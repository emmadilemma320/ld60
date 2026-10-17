using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Follow : MonoBehaviour
{
    public Transform follow;
    public float easing;
    public float snap;
    private Vector2 velocity;
    private Vector2 btsp;
    public void Simulate()
    {
        velocity = new Vector2(follow.position.x - btsp.x, follow.position.y - btsp.y) / easing;
        velocity = new Vector2(Mathf.Sign(velocity.x) * Mathf.Max(Mathf.Abs(velocity.x), snap), Mathf.Sign(velocity.y) * Mathf.Max(Mathf.Abs(velocity.y), snap));
        btsp += velocity * Time.fixedDeltaTime;
        transform.position = new Vector3(1f / 16f * Mathf.Round(btsp.x * 16f), 1f / 16f * Mathf.Round(btsp.y * 16f), transform.position.z);
    }
}
