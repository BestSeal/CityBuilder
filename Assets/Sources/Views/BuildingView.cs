using System;
using System.Threading.Tasks;
using Sources.Buildings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace Sources.Views
{
    public class BuildingView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private LayerMask layerToPutOn;
        [SerializeField] private GridView buildingGrid;
        [SerializeField] private Building buildingType;
        [SerializeField] private GameObject readyPopup;
        private Camera _mainCamera;
        private GridView _floorGrid;
        private Ground _ground;
        private Builder _builder;
        private RecoursesRepository _recoursesRepository;
        private bool _isProductionReady;

        private Action ProductionDone;
        
        [Inject]
        public void Init(Camera mainCamera, GridView floorGrid, Ground ground, Builder builder,
            RecoursesRepository recoursesRepository)
        {
            _mainCamera = mainCamera;
            _floorGrid = floorGrid;
            _ground = ground;
            _builder = builder;
            _recoursesRepository = recoursesRepository;
        }
        
        private bool InTemplateMode { get; set; }

        public (RecourseType recourseType, float cost) GetConstructionCost()
            => buildingType.GetConstructionCost();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (buildingType is ProductionBuilding productionBuilding && _isProductionReady)
            {
                _isProductionReady = false;
                if (readyPopup != null)
                {
                    readyPopup.gameObject.SetActive(false);
                }
                ProduceResource(ProductionDone);
                _recoursesRepository.RecourseValueChange(productionBuilding.ProducedResource,
                    productionBuilding.GetProducedAmount);
            }
        }

        private void ResetProduction()
        {
            _isProductionReady = true;
            if (readyPopup != null)
            {
                readyPopup.gameObject.SetActive(true);
            }
        }
        
        private void Awake()
        {
            InTemplateMode = true;
            if (readyPopup != null)
            {
                readyPopup.gameObject.SetActive(false);
            }
            ProductionDone += ResetProduction;
        }

        private void Update()
        {
            if (InTemplateMode && Mouse.current.delta.ReadValue().magnitude > 0)
            {
                ChangeTemplatePosition();
            }

            if (InTemplateMode && Mouse.current.leftButton.isPressed)
            {
                InstantiateTemplate();
            }
        }

        private void ChangeTemplatePosition()
        {
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hitInfo, 100, layerToPutOn) &&
                hitInfo.transform.gameObject == _ground.gameObject)
            {
                var hitPoint = hitInfo.point;
                var (rows, columns) = buildingGrid.GetRowsAndColumnsCount;
                if (_floorGrid.IsCellsRegionEmpty(hitPoint, rows, columns))
                {
                    transform.position = _floorGrid.GetClosestCellPosition(hitPoint);
                }
            }
        }

        private void InstantiateTemplate()
        {
            var ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hitInfo, 100, layerToPutOn) &&
                hitInfo.transform.gameObject == _ground.gameObject)
            {
                var hitPoint = hitInfo.point;
                var (rows, columns) = buildingGrid.GetRowsAndColumnsCount;
                if (_floorGrid.TryPlaceBuilding(hitPoint, rows, columns))
                {
                    ConstructBuilding();
                    InTemplateMode = false;
                }
            }
        }
        
        private async void ConstructBuilding()
        {
            await Task.Delay(TimeSpan.FromSeconds(buildingType.GetConstructionTime));
            _builder.Constructed?.Invoke();
            if (buildingType is ProductionBuilding productionBuilding)
            {
                ProduceResource(ProductionDone);
            }
            else if (buildingType is StorageBuilding storageBuilding)
            {
                _recoursesRepository.RecourseMaxValueChange?.Invoke(storageBuilding.StorableRecourse,
                    storageBuilding.StorageCapacity);
            }
        }
        
        private async void ProduceResource(Action productionCallback)
        {
            if (buildingType is ProductionBuilding productionBuilding)
            {
                await Task.Delay(TimeSpan.FromSeconds(productionBuilding.GetProductionCycleTime));
                productionCallback?.Invoke();
            }
        }
    }
}