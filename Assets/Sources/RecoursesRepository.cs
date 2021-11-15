using System;
using Sources.Buildings;
using UnityEngine;

namespace Sources
{
    public class RecoursesRepository : MonoBehaviour
    {
        [SerializeField] private float startMineralsValue;
        [SerializeField] private float startGasValue;

        private float _currentMineralsValue;
        private float _maxMineralsValue;
        private float _currentGasValue;
        private float _maxGasValue;

        /// <summary>
        /// Should be invoked when resource value needs update 
        /// </summary>
        public Action<RecourseType, float> RecourseValueChange;
        
        /// <summary>
        /// Invoked after resource value changed
        /// </summary>
        public event Action<RecourseType, float> RecourseValueChanged;
        
        /// <summary>
        /// Should be invoked when resource max value needs update 
        /// </summary>
        public Action<RecourseType, float> RecourseMaxValueChange;
        
        private void Awake()
        {
            _currentMineralsValue = startMineralsValue;
            _currentGasValue = startGasValue;
            _maxMineralsValue = startMineralsValue;
            _maxGasValue = startGasValue;
            RecourseValueChange += ChangeResourceValue;
            RecourseMaxValueChange += ChangeResourceMaxValue;
        }
        
        public float GetResourceMaxValue(RecourseType recourseType)
        {
            switch (recourseType)
            {
                case RecourseType.Mineral:
                    return _maxMineralsValue;
                case RecourseType.Gas:
                    return _maxGasValue;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unsupported value {recourseType} for {nameof(RecourseType)}");
            }
        }
        
        public float GetResourceCurrentValue(RecourseType recourseType)
        {
            switch (recourseType)
            {
                case RecourseType.Mineral:
                    return _currentMineralsValue;
                case RecourseType.Gas:
                    return _currentGasValue;
                default:
                    throw new ArgumentOutOfRangeException(
                        $"Unsupported value {recourseType} for {nameof(RecourseType)}");
            }
        }

        private void ChangeResourceValue(RecourseType recourseType, float value)
        {
            switch (recourseType)
            {
                case RecourseType.Mineral:
                    _currentMineralsValue = Mathf.Clamp(_currentMineralsValue + value, 0, _maxMineralsValue);
                    RecourseValueChanged?.Invoke(RecourseType.Mineral, _currentMineralsValue);
                    break;
                case RecourseType.Gas:
                    _currentGasValue = Mathf.Clamp(_currentGasValue + value, 0, _maxGasValue);
                    RecourseValueChanged?.Invoke(RecourseType.Gas, _currentGasValue);
                    break;
            }
        }
        
        private void ChangeResourceMaxValue(RecourseType recourseType, float value)
        {
            switch (recourseType)
            {
                case RecourseType.Mineral:
                    _maxMineralsValue = Mathf.Clamp(_currentMineralsValue + value, 0, float.MaxValue);
                    break;
                case RecourseType.Gas:
                    _maxGasValue = Mathf.Clamp(_maxGasValue + value, 0, float.MaxValue);
                    break;
            }

            ChangeResourceValue(recourseType, 0);
        }
    }
}
