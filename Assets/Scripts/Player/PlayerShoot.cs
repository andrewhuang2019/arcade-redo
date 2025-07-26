using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireCooldown = 0.5f;

    private SpriteRenderer spriteRenderer;
    private PlayerInputHandler inputHandler;
    public Animator animator;
    private float lastFireTime = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetBool("IsJumping") && inputHandler.ShootPressed && Time.time >= lastFireTime + fireCooldown)
        {
            animator.SetTrigger("Shoot");
            lastFireTime = Time.time;
            inputHandler.ShootPressed = false;
        }
    }
    
    public void Shoot()
    {
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        bullet.GetComponent<PlayerArrow>().SetDirection(direction);
    }
}
