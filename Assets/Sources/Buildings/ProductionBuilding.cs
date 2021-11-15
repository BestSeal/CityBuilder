using UnityEngine;

namespace Sources.Buildings
{
    [CreateAssetMenu(menuName = "Buildings/" + nameof(ProductionBuilding), fileName = nameof(ProductionBuilding))]
    public class ProductionBuilding : Building
    {
        [SerializeField] private float productionCycleTime;
        [SerializeField] private float getProducedRecourseAmount;
        [SerializeField] private RecourseType producedRecourseType;

        public RecourseType ProducedResource => producedRecourseType;
        public float GetProducedAmount => getProducedRecourseAmount;
        public float GetProductionCycleTime => productionCycleTime;
    }
}