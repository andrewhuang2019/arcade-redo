using UnityEngine;

// On the GameObject with the Animator
public class AnimationEventBridge : MonoBehaviour
{
    public PlayerShoot playerScript;

    public void Shoot()
    {
        if (playerScript != null)
        {
            playerScript.Shoot();
        }
    }
}