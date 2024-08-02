using UnityEngine;

public class Rotator : MonoBehaviour
{
    /// <summary>
    /// Degrees to rotate around the X axis.
    /// </summary>
    [SerializeField]
    private float rotationStepX = 2f;
    /// <summary>
    /// Degrees to rotate around the Y axis.
    /// </summary>
    [SerializeField]
    private float rotationStepY = 2f;
    /// <summary>
    /// Degrees to rotate around the Z axis.
    /// </summary>
    [SerializeField]
    private float rotationStepZ = 2f;

    /// <summary>
    /// Axis to apply rotation to.
    /// </summary>
    private Vector3 rotationStep = new Vector3();

    public void Awake()
    {
        rotationStep = new Vector3(Random.Range(-rotationStepX, rotationStepX), Random.Range(-rotationStepY, rotationStepY), Random.Range(-rotationStepZ, rotationStepZ));
    }

    public void FixedUpdate()
    {
        /// Rotate the object around its local axes at fixed framerate frame
        transform.Rotate(rotationStep);
    }
}