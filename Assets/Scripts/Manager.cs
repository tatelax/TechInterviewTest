using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool cullEveryFrame;
    [SerializeField] private float offsetMultiplier = 20F;
    [SerializeField] private int objAmount = 10;
    [SerializeField] private float spawnRadius = 2F;
    [SerializeField] private float viewCutoff = 45f;
    [SerializeField] private string objectName = "Object ";

    [Header("References")] 
    [SerializeField] private GameObject prefab;
    [SerializeField] private Camera mainCamera;

    private List<Transform> instantiatedObjects;
    private bool isDragging;
    private Vector3 mousePosOnStartDrag;

    private const float ZMouseOffset = 1F;
    
    private void Start()
    {
        instantiatedObjects = new List<Transform>();
        InstantiateObjects();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosOnStartDrag = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ZMouseOffset));
            isDragging = true;
        }
        
        if (Input.GetMouseButtonUp(0))
            isDragging = false;

        if (isDragging)
        {
            Vector3 currMousePosInWorld = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, ZMouseOffset));
            Vector3 offset = currMousePosInWorld - mousePosOnStartDrag;

            if (offset.x == 0 && offset.y == 0 && offset.z == 0) return; // We are only interested in moving objects if there was a change in drag
            
            UpdateObjectPositions(offset);
            mousePosOnStartDrag = currMousePosInWorld;
        }
        
        if(cullEveryFrame)
            CullObjects();
    }

    private void InstantiateObjects()
    {
        for (int i = 0; i < objAmount; i++)
        {
            GameObject newObj = Instantiate(prefab, Random.insideUnitSphere * spawnRadius, Quaternion.identity);
            newObj.name = objectName + i;
            
            instantiatedObjects.Add(newObj.transform);
        }
    }

    private void UpdateObjectPositions(Vector3 offset)
    {
        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            instantiatedObjects[i].transform.position += offset * offsetMultiplier;
            CullObjects();
        }
    }

    private void CullObjects()
    {
        for (int i = 0; i < instantiatedObjects.Count; i++)
        {
            Transform cameraTransform = mainCamera.transform;

            if (Helpers.TestCone(cameraTransform.position,
                instantiatedObjects[i].transform.position,
                cameraTransform.forward,
                viewCutoff))
            {
                instantiatedObjects[i].transform.position = Random.insideUnitSphere * spawnRadius;
            }
        }
    }
}