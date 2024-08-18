using UnityEngine;

public class SpinObject : MonoBehaviour
{
    // Speed of rotation
    public float SpinMax=200f;
    public float SpinMin=-200f;

    private float rotationSpeed;
    
    void Start()
    {
        rotationSpeed = Random.Range(SpinMin, SpinMax);
    }
    void Update()
    {
        // Rotate the object slightly around the Z-axis over time
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}