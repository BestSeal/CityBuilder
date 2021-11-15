using UnityEngine;

namespace Sources.Buildings
{
    [CreateAssetMenu(menuName = "Buildings/" + nameof(StorageBuilding), fileName = nameof(StorageBuilding))]
    public class StorageBuilding : Building
    {
        [SerializeField] private float storageCapacity;
        [SerializeField] private RecourseType storableRecourse;

        public float StorageCapacity => storageCapacity;
        public RecourseType StorableRecourse => storableRecourse;
    }
}