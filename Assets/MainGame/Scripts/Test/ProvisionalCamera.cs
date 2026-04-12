using UnityEngine;

public class ProvisionalCamera : MonoBehaviour
{
    // Reference to the player GameObject
    GameObject player;
    // Reference to the camera GameObject
    GameObject camera;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null ) Debug.LogError("Player not found in the scene. Please make sure there is a GameObject with the tag 'Player'.");
        camera = gameObject;
    }
    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        camera.transform.position = new Vector3(0, Mathf.Max(player.transform.position.y, 0), -20);
    }
}
