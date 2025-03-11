using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float swipeSensitivity = 0.1f;
    
    [Header("Crowd Settings")]
    public GameObject stickmanPrefab;
    public float crowdSpacing = 0.5f;
    public int initialCrowdSize = 10;
    
    private List<GameObject> crowd = new List<GameObject>();
    private Vector2 touchStartPos;
    private bool isDragging = false;
    private Transform crowdParent;
    
    void Start()
    {
        crowdParent = new GameObject("CrowdParent").transform;
        InitializeCrowd();
    }
    
    void Update()
    {
        HandleMovement();
        HandleSwipeInput();
    }
    
    void InitializeCrowd()
    {
        for (int i = 0; i < initialCrowdSize; i++)
        {
            SpawnStickman(GetFormationPosition(i));
        }
    }
    
    void SpawnStickman(Vector3 position)
    {
        GameObject stickman = Instantiate(stickmanPrefab, position, Quaternion.identity, crowdParent);
        crowd.Add(stickman);
    }
    
    Vector3 GetFormationPosition(int index)
    {
        int row = index / 5;
        int col = index % 5;
        return transform.position + new Vector3(col * crowdSpacing, 0, row * crowdSpacing);
    }
    
    void HandleMovement()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        
        // Update crowd positions
        for (int i = 0; i < crowd.Count; i++)
        {
            Vector3 targetPos = GetFormationPosition(i);
            crowd[i].transform.position = Vector3.Lerp(crowd[i].transform.position, targetPos, Time.deltaTime * 10f);
        }
    }
    
    void HandleSwipeInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isDragging = true;
                    break;
                    
                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Vector2 delta = touch.position - touchStartPos;
                        float moveX = delta.x * swipeSensitivity;
                        Vector3 newPos = transform.position + new Vector3(moveX, 0, 0);
                        transform.position = newPos;
                        touchStartPos = touch.position;
                    }
                    break;
                    
                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
    }
    
    public void AddToCrowd(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnStickman(GetFormationPosition(crowd.Count));
        }
    }
    
    public void RemoveFromCrowd(int count)
    {
        count = Mathf.Min(count, crowd.Count);
        for (int i = 0; i < count; i++)
        {
            GameObject stickman = crowd[crowd.Count - 1];
            crowd.RemoveAt(crowd.Count - 1);
            Destroy(stickman);
        }
    }
    
    public int GetCrowdSize()
    {
        return crowd.Count;
    }
}
