using UnityEngine;

public class PlayerMovementBoundary: MonoBehaviour
{
    [SerializeField] 
    float xMin = -4.5f;
    [SerializeField] 
    float xMax = 4.5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(
            Mathf.Clamp(gameObject.transform.position.x, xMin, xMax),
            gameObject.transform.position.y,
            gameObject.transform.position.z
        );
    }
}
