using CylinderDemo.Interfaces;

namespace CylinderDemo.Implementations;

/// <summary>
/// CylA = pneumatic cylinder implementation.
/// PLC analogy: FB_CylA.
///
/// Design matches your class-like TIA structure:
/// - Constructor (firstScan) sets initial state
/// - ExecuteCommands decides internal outputs from Cmd + permissives
/// - UpdateInternalState updates private state ("physics")
/// - PublishStatus exposes public status
///
/// Here we implement the same concept as real C# OOP.
/// </summary>
public sealed class CylA : IActStatus
{
    // --------------------------
    // "Private fields" (PLC: VAR)
    // --------------------------
    private bool _isExtended;
    private bool _isRetracted;

    // Internal "valves" (kept private; in PLC they were local BOOLs)
    private bool _valveExtend;
    private bool _valveRetract;

    public CylA()
    {
        // PLC analogy: firstScan constructor initialization
        // Didactic assumption: starts retracted
        _isExtended = false;
        _isRetracted = true;
    }

    /// <summary>
    /// One scan/update of the object.
    /// PLC analogy: cyclic call of FB_CylA from FB_Cylinder / OB1.
    /// </summary>
    public void Update(IActCmd cmd, bool pressureOk)
    {
        ExecuteCommands(cmd, pressureOk);
        UpdateInternalState();
        PublishStatus();
    }

    // --------------------------
    // "Methods" (PLC: REGIONS)
    // --------------------------

    private void ExecuteCommands(IActCmd cmd, bool pressureOk)
    {
        // Default safe outputs (valves OFF)
        _valveExtend = false;
        _valveRetract = false;

        // Object enable (selection)
        if (!cmd.Enable) return;

        // Pneumatic permissive
        if (!pressureOk) return;

        // Direct command handling (no priority function)
        // Extend wins over Retract because of IF/ELSIF order
        if (cmd.Extend)
            _valveExtend = true;
        else if (cmd.Retract)
            _valveRetract = true;
    }

    private void UpdateInternalState()
    {
        // Simplified "physics": valves define state
        if (_valveExtend)
        {
            _isExtended = true;
            _isRetracted = false;
        }
        else if (_valveRetract)
        {
            _isExtended = false;
            _isRetracted = true;
        }
    }

    private void PublishStatus()
    {
        // In C#, the public properties read from private state.
        // (In PLC you assigned Status := isExtended/isRetracted)
    }

    // --------------------------
    // "Public interface"
    // --------------------------

    public bool Extended => _isExtended;
    public bool Retracted => _isRetracted;

    // Helpful for the console "watch table"
    public (bool ValveExtend, bool ValveRetract) DebugValves => (_valveExtend, _valveRetract);
}
