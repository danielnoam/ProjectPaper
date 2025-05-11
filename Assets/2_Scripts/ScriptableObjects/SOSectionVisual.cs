using UnityEngine;

[CreateAssetMenu(fileName = "New Section Visual", menuName = "Scriptable Objects/SOSectionVisual")]
public class SOSectionVisual : ScriptableObject
{

    [Header("Section Visuals")]
    [SerializeField] private Color backgroundColor = Color.aquamarine;
    [SerializeField] private GameObject[] backgroundObjects;
    
    
    [Header("Section Settings")]
    [SerializeField] private float backgroundObjectsScarcity = 1f;
    [SerializeField] private float backgroundObjectsSpeed = 1f;
    [SerializeField] private float backgroundObjectsScale = 1f;


}
