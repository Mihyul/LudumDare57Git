using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FlashlightSystem
{
    public class Flashlight : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Light2D radiusLight;
        [SerializeField] private Light2D coneLight;
        [SerializeField] private Transform aimTarget;

        [Space]
        [SerializeField] private FlashlightConfigurationSetter.FlashlightConfiguration[] flashlightConfigurations = new FlashlightConfigurationSetter.FlashlightConfiguration[0];

        [Space]
        [SerializeField] private CinemachineVirtualCamera switchedOffVirtualCamera;
        [SerializeField] private float switchedOffAimDistance;

        public int CurrentConfigurationIndex { get; private set; }

        private FlashlightConfigurationSetter _flashlightConfigurationSetter;

        private void Start()
        {
            _flashlightConfigurationSetter = new(radiusLight, coneLight, aimTarget, switchedOffVirtualCamera, switchedOffAimDistance);
        }

        public void SwitchToNextConfiguration()
        {
            if (CurrentConfigurationIndex == flashlightConfigurations.Length - 1)
                return;

            SwitchToConfiguration(CurrentConfigurationIndex + 1);
        }

        public void SwitchToPreviousConfiguration()
        {
            if (CurrentConfigurationIndex == -1)
                return;

            SwitchToConfiguration(CurrentConfigurationIndex - 1);
        }

        public void SwitchToConfiguration(int index)
        {
            if (index < -1 || index >= flashlightConfigurations.Length)
                return;
            else if (index == -1)
                _flashlightConfigurationSetter.SetSwitchOffConfiguration();
            else
                _flashlightConfigurationSetter.SetFlashlightConfiguration(flashlightConfigurations[index]);

            CurrentConfigurationIndex = index;
        }

        private class FlashlightConfigurationSetter
        {
            private readonly LightConfiurationSetter _lightConfigurationSetter;
            private readonly AimConfiurationSetter _aimConfigurationSetter;

            public FlashlightConfigurationSetter(Light2D radiusLight,
                                                 Light2D coneLight,
                                                 Transform aimTarget,
                                                 CinemachineVirtualCamera switchedOffVirtualCamera,
                                                 float switchedOffAimDistance)
            {
                _lightConfigurationSetter = new(radiusLight, coneLight);
                _aimConfigurationSetter = new(aimTarget, switchedOffVirtualCamera, switchedOffAimDistance);
            }

            public void SetFlashlightConfiguration(FlashlightConfiguration configuration)
            {
                _lightConfigurationSetter.SetLightConfiguration(configuration.LightConfiguration);
                _aimConfigurationSetter.SetAimSettings(configuration.AimConfiguration);
            }

            public void SetSwitchOffConfiguration()
            {
                _lightConfigurationSetter.SetSwitchOffConfiguration();
                _aimConfigurationSetter.SetSwitchOffConfiguration();
            }

            [Serializable]
            public struct FlashlightConfiguration
            {
                [field: SerializeField] public LightConfiurationSetter.LightConfiguration LightConfiguration { get; private set; }

                [field: Space]
                [field: SerializeField] public AimConfiurationSetter.AimConfiguration AimConfiguration { get; private set; }
            }


            public class LightConfiurationSetter
            {
                private readonly Light2D _radiusLight;
                private readonly Light2D _coneLight;

                public LightConfiurationSetter(Light2D radiusLight, Light2D coneLight)
                {
                    _radiusLight = radiusLight != null ? radiusLight : throw new ArgumentNullException(nameof(radiusLight));
                    _coneLight = coneLight != null ? coneLight : throw new ArgumentNullException(nameof(coneLight));

                    _radiusLight.lightType = Light2D.LightType.Point;
                    _radiusLight.pointLightInnerAngle = 360.0f;
                    _radiusLight.pointLightOuterAngle = 360.0f;

                    _coneLight.lightType = Light2D.LightType.Point;
                }

                public void SetLightConfiguration(LightConfiguration configuration)
                {
                    _radiusLight.enabled = true;
                    _coneLight.enabled = true;

                    _radiusLight.pointLightOuterRadius = configuration.RadiusLightOuterRadius;
                    _radiusLight.pointLightInnerRadius = configuration.RadiusLightInnerRadius;
                    _radiusLight.falloffIntensity = configuration.RadiusLightFalloffStrength;

                    _coneLight.pointLightOuterRadius = configuration.ConeLightOuterRadius;
                    _coneLight.pointLightInnerRadius = configuration.ConeLightInnerRadius;
                    _coneLight.pointLightInnerAngle = configuration.ConeLightInnerAngle;
                    _coneLight.pointLightOuterAngle = configuration.ConeLightOuterAngle;
                    _coneLight.falloffIntensity = configuration.ConeLightFalloffStrength;
                }

                public void SetSwitchOffConfiguration()
                {
                    _radiusLight.enabled = false;
                    _coneLight.enabled = false;
                }

                [Serializable]
                public struct LightConfiguration
                {
                    [field: Header("Radius light")]
                    [field: SerializeField] public float RadiusLightInnerRadius { get; private set; }
                    [field: SerializeField] public float RadiusLightOuterRadius { get; private set; }
                    [field: SerializeField, Range(0f, 1.0f)] public float RadiusLightFalloffStrength { get; private set; }

                    [field: Header("Cone Light")]
                    [field: SerializeField] public float ConeLightInnerRadius { get; private set; }
                    [field: SerializeField] public float ConeLightOuterRadius { get; private set; }
                    [field: SerializeField, Range(0f, 360.0f)] public float ConeLightInnerAngle { get; private set; }
                    [field: SerializeField, Range(0f, 360.0f)] public float ConeLightOuterAngle { get; private set; }
                    [field: SerializeField, Range(0f, 1.0f)] public float ConeLightFalloffStrength { get; private set; }
                }
            }

            public class AimConfiurationSetter
            {
                private readonly Transform _aimTarget;
                private readonly CinemachineVirtualCamera _switchedOffVirtualCamera;
                private readonly float _switchedOffAimDistance;

                private CinemachineVirtualCamera _currentVirtualCamera;

                public AimConfiurationSetter(Transform aimTarget, CinemachineVirtualCamera switchedOffVirtualCamera, float switchedOffAimDistance)
                {
                    _aimTarget = aimTarget != null ? aimTarget : throw new ArgumentNullException(nameof(aimTarget));
                    _switchedOffVirtualCamera = switchedOffVirtualCamera != null ? switchedOffVirtualCamera : throw new ArgumentNullException(nameof(switchedOffVirtualCamera));
                    _switchedOffAimDistance = switchedOffAimDistance;
                }

                public void SetAimSettings(AimConfiguration configuration)
                {
                    SetVirtualCamera(configuration.VirtualCamera);
                    _aimTarget.localPosition = new(0.0f, configuration.AimDistance, 0.0f);
                }

                public void SetSwitchOffConfiguration()
                {
                    SetVirtualCamera(_switchedOffVirtualCamera);
                    _aimTarget.localPosition = new(0.0f, _switchedOffAimDistance, 0.0f);
                }

                private void SetVirtualCamera(CinemachineVirtualCamera camera)
                {
                    if (_currentVirtualCamera != null)
                        _currentVirtualCamera.enabled = false;
                    _currentVirtualCamera = camera;
                    _currentVirtualCamera.enabled = true;
                }

                [Serializable]
                public struct AimConfiguration
                {
                    [field: SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
                    [field: SerializeField] public float AimDistance { get; private set; }
                }
            }
        }
    }
}
