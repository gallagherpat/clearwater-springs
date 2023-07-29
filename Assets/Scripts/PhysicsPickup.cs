using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPickup : MonoBehaviour
{
    [SerializeField] private LayerMask pickupMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform pickupTarget;
    [Space]
    [SerializeField] private float pickupRange;
    public Rigidbody currentObject;
    public GameObject currentObjectTransform;
    public Vector2 mouse;

    [Space]
    public float sensitivityX;
    public float sensitivityY;

    public bool holdingMouse = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && currentObject != null)
        {
            holdingMouse = true;
            Debug.Log("Mouse down");

            mouse.x += Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
            mouse.y += Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

            currentObject.transform.localRotation = Quaternion.Euler(-mouse.y, mouse.x, 0);
        }
        if(Input.GetMouseButtonUp(0))
        {
            holdingMouse = false;
        } 
        if(Input.GetKeyDown(KeyCode.E))
        {
            if(currentObject)
            {
                currentObject.useGravity = true;
                currentObject = null;
                return;
            }

            Ray cameraRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            if(Physics.Raycast(cameraRay, out RaycastHit HitInfo, pickupRange, pickupMask))
            {
                currentObject = HitInfo.rigidbody;
                currentObject.useGravity = false;
            } 
        }
    }

    void FixedUpdate()
    {
        if(currentObject)
        {
            Vector3 directionToPoint = pickupTarget.position - currentObject.position;
            float distanceToPoint = directionToPoint.magnitude;

            currentObject.velocity = directionToPoint * 12f * distanceToPoint;
        }
    }
}
