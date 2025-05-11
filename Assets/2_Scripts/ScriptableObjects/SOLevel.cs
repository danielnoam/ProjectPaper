using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Core.Attributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Scriptable Objects/SOLevel")]
public class SOLevel : ScriptableObject
{
    [Header("Level Settings")]
    [SerializeField] private string levelName;
    
    
    [SerializedDictionary ("Type","Visual")] public SerializedDictionary<SOSectionType, SOSectionVisual> levelSections = new SerializedDictionary<SOSectionType, SOSectionVisual>();

}
