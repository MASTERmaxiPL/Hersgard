using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckInteractible : MonoBehaviour
{
    public static CheckInteractible instance;
    [SerializeField]
    private GameObject interactibleArea;
    [SerializeField]
    private List<GameObject> interactibleObjects;

    public GameObject closestObject;

    public static CheckInteractible GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        interactibleObjects = new List<GameObject>();
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interactible" && collision.gameObject.name != "Trigger" && collision.gameObject.name != "Collider")
        {
            interactibleObjects.Add(collision.gameObject);
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Interactible" && collision.gameObject.name != "Trigger" && collision.gameObject.name != "Collider")
        {
            interactibleObjects.Remove(collision.gameObject);
        }
    }



    private void Update()
    {
        closestObject = GetClosestInteractibleObject(interactibleObjects, this.transform);
    }

    GameObject GetClosestInteractibleObject(List<GameObject> objects, Transform fromThis)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (GameObject potentialTarget in objects)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return bestTarget;
    }
}
