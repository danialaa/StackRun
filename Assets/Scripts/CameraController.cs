using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset;

    public void SetOffset(Vector3 runnerPosition)
    {
        offset = runnerPosition - transform.position;
    }

    public void UpdateCameraToRunner(Vector3 runnerPos)
    {
        transform.position = runnerPos - offset;
    }
}
