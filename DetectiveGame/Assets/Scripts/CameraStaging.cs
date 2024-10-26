using UnityEngine;

public class CameraStaging : MonoBehaviour
{
    /// <summary>
    /// Transforms within the scene used to determine the camera's stagings.
    /// </summary>
    [SerializeField] private Transform[] m_positionMarkers;

    /// <summary>
    /// The speed, in meters per second, at which the camera will move towards its target.
    /// </summary>
    [SerializeField] private float m_movementSpeed = 5.0f;
    
    /// <summary>
    /// Once the camera reaches this distance from its target, it will snap to its position and stop moving.
    /// </summary>
    [SerializeField] private float m_minDistance = 0.1f;

    private Vector3 m_targetPosition;
    
    private bool m_isMoving;

    private void Start()
    {
        MoveCamera(0);
    }

    private void Update()
    {
        if(!m_isMoving) { return; } // Break if the camera is not currently moving.

        var direction = Vector3.Normalize(m_targetPosition - transform.position);
        transform.position += direction * (m_movementSpeed * Time.deltaTime); // Move the camera towards the target, incrementally.
        
        if(Vector3.Distance(transform.position, m_targetPosition) > m_minDistance) { return; } // Break if the camera is still further from the target than the clamping distance.

        ClampToPosition(m_targetPosition); // Clamp to the target position, stop moving.
    }

    /// <summary>
    /// Moves the camera towards a staging position, laid out within the scene.
    /// </summary>
    /// <param name="destinationIndex">The index of the staging Transform to move toward.</param>
    /// <param name="instant">Whether the camera should instantly snap to its new position.</param>
    public void MoveCamera(int destinationIndex, bool instant = true)
    {
        if(destinationIndex >= m_positionMarkers.Length) { return; }
        MoveCamera(m_positionMarkers[destinationIndex].position);
    }

    /// <summary>
    /// Moves the camera towards an arbitrary position within the scene.
    /// </summary>
    /// <param name="destination">A world-space position to move toward.</param>
    /// <param name="instant">Whether the camera should instantly snap to its new position.</param>
    public void MoveCamera(Vector3 destination, bool instant = true)
    {
        if (instant)
        {
            ClampToPosition(destination);
            return;
        }

        m_targetPosition = destination;
        m_isMoving = true;
    }

    private void ClampToPosition(Vector3 position)
    {
        transform.position = position;
        m_isMoving = false;
    }
}
