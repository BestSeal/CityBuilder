using Sources.Buildings;
using TMPro;
using UnityEngine;
using Zenject;

namespace Sources
{
    public class UiActionsHandler : MonoBehaviour
    {
        [SerializeField] private RectTransform buildingsPanel;
        [SerializeField] private TextMeshProUGUI mineralValueIndicator;
        [SerializeField] private TextMeshProUGUI gasValueIndicator;

        private Builder _builder;
        private RecoursesRepository _recoursesRepository;

        [Inject]
        public void Init(Builder builder, RecoursesRepository recoursesRepository)
        {
            _builder = builder;
            _recoursesRepository = recoursesRepository;
        }

        private void Awake()
        {
            _builder.Construct += HideBuildingsPanel;
            _recoursesRepository.RecourseValueChanged += UpdateResourceIndicator;
        }

        private void Start()
        {
            UpdateResourceIndicator(RecourseType.Gas, _recoursesRepository.GetResourceCurrentValue(RecourseType.Gas));
            UpdateResourceIndicator(RecourseType.Mineral, _recoursesRepository.GetResourceCurrentValue(RecourseType.Mineral));
        }

        public void ShowBuildingsPanel()
        {
            buildingsPanel.gameObject.SetActive(!buildingsPanel.gameObject.activeSelf);
        }
    
        private void HideBuildingsPanel()
        {
            buildingsPanel.gameObject.SetActive(false);
        }

        private string GetResourceIndicatorValue(float currentValue, float maxValue)
            => $"{currentValue} / {maxValue}";
    
        private void UpdateResourceIndicator(RecourseType recourseType, float value)
        {
            var maxResourceValue = _recoursesRepository.GetResourceMaxValue(recourseType);
        
            switch (recourseType)
            {
                case RecourseType.Mineral:
                    mineralValueIndicator.text = GetResourceIndicatorValue(value, maxResourceValue);
                    break;
                case RecourseType.Gas:
                    gasValueIndicator.text = GetResourceIndicatorValue(value, maxResourceValue);
                    break;
            }
        }
    }
}
