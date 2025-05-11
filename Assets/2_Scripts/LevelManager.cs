
using Core.Attributes;
using UnityEngine;

using VInspector;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }
    
    
    [Header("Game Settings")]
    [SerializeField] private Vector2 worldBoundsSize = new Vector2(10, 10);
    [CreateEditableAsset][SerializeField] private SOLevel defaultLevel;
    
    
    [Header("Debug")]
    [SerializeField, ReadOnly] private int difficulty = 1;
    [SerializeField, ReadOnly] private int players = 1;
    [SerializeField, ReadOnly] private SOLevel currentLevel;
    [SerializeField, ReadOnly] private SOSectionVisual currentSectionVisual;
    [SerializeField, ReadOnly] private SOSectionType currentSectionType;
    
    public SOLevel CurrentLevel => currentLevel;
    public SOSectionVisual CurrentSectionVisual => currentSectionVisual;
    public SOSectionType CurrentSectionType => currentSectionType;
    public int Difficulty => difficulty;
    public int Players => players;


    private void Awake()
    {
        if (!Instance || Instance == this)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // Set the default level
        if (!currentLevel && defaultLevel) currentLevel = defaultLevel;
    }


    #region World Bounds -------------------------------------------------------------

    private Rect WorldBoundsRect
    {
        get
        {
            Vector2 center = transform.position;
            return new Rect(
                center.x - worldBoundsSize.x / 2,
                center.y - worldBoundsSize.y / 2,
                worldBoundsSize.x,
                worldBoundsSize.y
            );
        }
    }
    
    public bool IsWithinBounds(Vector2 position)
    {
        Rect bounds = WorldBoundsRect;
        return bounds.Contains(position);
    }
    
    public Vector2 ClampToBounds(Vector2 position)
    {
        Rect bounds = WorldBoundsRect;
        position.x = Mathf.Clamp(position.x, bounds.xMin, bounds.xMax);
        position.y = Mathf.Clamp(position.y, bounds.yMin, bounds.yMax);
        return position;
    }

    public Vector2 GetClosestPointOnBounds(Vector2 position)
    {
        Rect bounds = WorldBoundsRect;
    
        // If already within bounds, return the position clamped to the edge
        if (bounds.Contains(position))
        {
            // Find the closest edge and return a point on it
            float distToLeft = position.x - bounds.xMin;
            float distToRight = bounds.xMax - position.x;
            float distToBottom = position.y - bounds.yMin;
            float distToTop = bounds.yMax - position.y;
        
            float minDist = Mathf.Min(distToLeft, distToRight, distToBottom, distToTop);
        
            if (Mathf.Approximately(minDist, distToLeft)) return new Vector2(bounds.xMin, position.y);
            if (Mathf.Approximately(minDist, distToRight)) return new Vector2(bounds.xMax, position.y);
            if (Mathf.Approximately(minDist, distToBottom)) return new Vector2(position.x, bounds.yMin);
            return new Vector2(position.x, bounds.yMax);
        }
    
        // If outside bounds, clamp to the nearest edge
        return new Vector2(
            Mathf.Clamp(position.x, bounds.xMin, bounds.xMax),
            Mathf.Clamp(position.y, bounds.yMin, bounds.yMax)
        );
    }

    #endregion World Bounds -------------------------------------------------------------
    
    
    
    private void OnDrawGizmos()
    {
        // Draw the world bounds for a 2D game
        Gizmos.color = Color.red;
    
        Vector3 center = transform.position;
    
        // Draw a 2D rectangle
        Vector3 topLeft = center + new Vector3(-worldBoundsSize.x/2, worldBoundsSize.y/2, 0);
        Vector3 topRight = center + new Vector3(worldBoundsSize.x/2, worldBoundsSize.y/2, 0);
        Vector3 bottomLeft = center + new Vector3(-worldBoundsSize.x/2, -worldBoundsSize.y/2, 0);
        Vector3 bottomRight = center + new Vector3(worldBoundsSize.x/2, -worldBoundsSize.y/2, 0);
    
        // Draw the rectangle
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
    
    
}
