using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Optional: add collision logic here (e.g., destroy enemy)
        Destroy(gameObject);
    }
}
