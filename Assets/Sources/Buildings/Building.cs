using UnityEngine;

namespace Sources.Buildings
{
    [CreateAssetMenu(menuName = "Buildings/" + nameof(Building), fileName = nameof(Building))]
    public class Building : ScriptableObject
    {
        [SerializeField] private float buildingCost;
        [SerializeField] private RecourseType resourceTypeCost;
        [Tooltip("In seconds")]
        [SerializeField] private float constructionTime;

        public (RecourseType recourseType, float cost) GetConstructionCost()
            => (resourceTypeCost, buildingCost);

        public float GetConstructionTime => constructionTime;
    }
}
