using UnityEngine;

public class ParkourSurface : MonoBehaviour, IInteractable
{
    Collider objectCollider;
    Bounds bounds;
    Vector3 parkourPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string ReturnName()
    {
        return gameObject.name;
    }

    public Vector3 ObjectTop()
    {
        bounds = objectCollider.bounds;

        parkourPosition = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
        return parkourPosition;
    }
}
