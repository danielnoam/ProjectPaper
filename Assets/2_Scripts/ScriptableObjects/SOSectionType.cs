using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

public enum SectionType
{
    Checkpoint,
    EnemyWave,
    BossWave,
}
[CreateAssetMenu(fileName = "New Level Type", menuName = "Scriptable Objects/SOSectionType")]
public class SOSectionType : ScriptableObject
{

    [Header("Section Type")]
    [SerializeField] private SectionType sectionType;
    
    
    [EnableIf("sectionType", SectionType.Checkpoint)]
    [Header("Checkpoint")]
    [SerializeField] private float duration;
    [SerializeField] private bool healPlayer;
    [EndIf]
    
    [EnableIf("sectionType", SectionType.EnemyWave)]
    [Header("Enemy Wave")]
    [SerializedDictionary ("Enemy","Amount")] public AYellowpaper.SerializedCollections.SerializedDictionary<SOSectionType, int> enemies = new AYellowpaper.SerializedCollections.SerializedDictionary<SOSectionType, int>();
    [EndIf]
    
    [EnableIf("sectionType", SectionType.BossWave)]
    [Header("Boss Wave")]
    [SerializeField] private GameObject boss;
    
    
    
    public SectionType SectionType => sectionType;
    public float Duration => duration;
    public bool HealPlayer => healPlayer;
    public AYellowpaper.SerializedCollections.SerializedDictionary<SOSectionType, int> Enemies => enemies;
    public GameObject Boss => boss;


    
}
