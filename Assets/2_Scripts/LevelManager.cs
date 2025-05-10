
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }
    
    
    [Header("Game Settings")]
    [SerializeField] private int difficulty = 1;
    [SerializeField] private int players = 1;
    [SerializeField] private Vector2 worldBoundsSize = new Vector2(10, 10);

    [Header("Current Level")]
    [SerializeField] private SOLevel level;
    
    
    public SOLevel Level => level;


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
        
            if (minDist == distToLeft) return new Vector2(bounds.xMin, position.y);
            if (minDist == distToRight) return new Vector2(bounds.xMax, position.y);
            if (minDist == distToBottom) return new Vector2(position.x, bounds.yMin);
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
