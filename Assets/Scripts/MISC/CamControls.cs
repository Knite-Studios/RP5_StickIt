using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControls : MonoBehaviour
{
    [SerializeField]
    private Bounds bounds;

    [Header("Zooming"), SerializeField]
    private float zoomSensitivity = 0.1f;
    [SerializeField]
    private Transform minZoomTransform;
    [SerializeField]
    private Transform maxZoomTransform;
    [Range(0, 1), SerializeField]
    private float smoothing = 0.1f;
    [SerializeField]
    private bool zoomToMouse = true;

    [Header("Panning"), SerializeField]
    private float panSpeed = 0.5f;
    private float zoomValue = 1.0f;

    [Header("Rotation"), SerializeField]
    private float rotationSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Pan();
        Zoom();
        Rotate();
    }
    void Pan()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        this.transform.Translate(horizontal * panSpeed, 0, vertical * panSpeed);
        transform.position = bounds.ClosestPoint(transform.position);
    }
    void Zoom()
    {
        Vector3 mousePosBeforeZoom = GetMouseHitPoint();
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel"); //Make sure this is spelled correctly
        Debug.Log(scrollDelta);
        zoomValue -= scrollDelta * zoomSensitivity;
        zoomValue = Mathf.Clamp(zoomValue, 0, 1);

        //We now need to change two things when we zoom: the rotation along the x axis and our vertical position
        //we need to do a linear interpolation between a couple values: for position, where our camera is and where a new position it should be is
        //for our rotation the same
        Vector3 newPosition = Vector3.Lerp(minZoomTransform.position, maxZoomTransform.position, zoomValue);
        Quaternion newRotation = Quaternion.Slerp(minZoomTransform.rotation, maxZoomTransform.rotation, zoomValue);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition, smoothing);
        Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, newRotation, smoothing);
        if (zoomToMouse)
        {
            Vector3 mousePosAfterZoom = GetMouseHitPoint();
            Vector3 offset = mousePosBeforeZoom - mousePosAfterZoom;
            transform.Translate(offset, Space.World);
        }
    }
    private Vector3 GetMouseHitPoint() //this is going to determine where we are hitting in our level
    {
        //we're going to cast a ray into our scene and see where it hits
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;

        //now let's shoot a ray into the scene and make sure to use a layermask to only hit terrain
        if (Physics.Raycast(mouseRay, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
    void Rotate()
    {
        float rotationInput = Input.GetAxis("Rotation");
        transform.Rotate(0, rotationInput * rotationSpeed, 0);
    }
}
