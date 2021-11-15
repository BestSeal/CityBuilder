using System;
using Sources.Views;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Sources
{
    public class Builder : MonoBehaviour
    {
        public Action Constructed;
        public Action Construct;
        private bool CanBuild { get; set; } = true;

        private Camera _mainCamera;
        private RecoursesRepository _recoursesRepository;

        [Inject]
        public void Init(Camera mainCamera, RecoursesRepository recoursesRepository)
        {
            _mainCamera = mainCamera;
            _recoursesRepository = recoursesRepository;
        }

        private void Awake()
        {
            Constructed += () => CanBuild = true;
        }

        public void CreateNewBuildingTemplate(GameObject building)
        {
            if (CanBuild && building != null)
            {
                var buildingView = building.GetComponent<BuildingView>();
                var (recourseType, cost) = buildingView.GetConstructionCost();

                if (_recoursesRepository.GetResourceCurrentValue(recourseType) - cost >= 0)
                {
                    _recoursesRepository.RecourseValueChange?.Invoke(recourseType, -cost);
                    Construct?.Invoke();
                    CanBuild = false;
                    var buildingPosition = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                    Instantiate(building, buildingPosition, Quaternion.identity);
                }
            }
        }
    }
}