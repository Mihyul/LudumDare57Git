using CoverSystem;
using FlashlightSystem;
using PerceptionSystem;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibility : APercievedObject, ICoverable
{
    [Space]
    [SerializeField] private Flashlight flashlight;
    [SerializeField] private float[] flashlightConfigurationVisibilities = new float[0];
    [SerializeField] private float flashlightSwitchedOffVisibility;

    private readonly HashSet<Cover> _covers = new();
    private bool IsInCover { get { return _covers.Count > 0; } }
    private bool IsHidden { get { return IsInCover && flashlight.CurrentConfigurationIndex == -1; } }
    private float FlashlightVisibility
    {
        get
        {
            if (flashlight.CurrentConfigurationIndex >= 0 && flashlight.CurrentConfigurationIndex < flashlightConfigurationVisibilities.Length)
                return flashlightConfigurationVisibilities[flashlight.CurrentConfigurationIndex];

            return flashlightSwitchedOffVisibility;
        }
    }

    protected override float VisibilityDistance { get { return IsHidden ? 0 : FlashlightVisibility; } }

    public void TakeCover(Cover cover)
    {
        _covers.Add(cover);
    }

    public void LeaveCover(Cover cover)
    {
        _covers.Remove(cover);
    }
}
