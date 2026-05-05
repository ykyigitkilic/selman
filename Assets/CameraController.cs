using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] public List<GameObject> cameras;
    private int currentCameraIndex = 0;

    void Start()
    {
        // Initial setup: turn everything off except the first camera
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] != null)
                cameras[i].SetActive(i == 0);
        }
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // "wasPressedThisFrame" ensures it only triggers once per tap
        if (keyboard.tKey.wasPressedThisFrame)
        {
            CycleCamera();
        }
    }

    private void CycleCamera()
    {
        if (cameras.Count == 0) return;

        // Disable current
        cameras[currentCameraIndex].SetActive(false);

        // Increment and wrap around
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;

        // Enable new
        cameras[currentCameraIndex].SetActive(true);
    }
}