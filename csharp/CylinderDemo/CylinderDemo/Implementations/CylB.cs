using CylinderDemo.Interfaces;
using CylinderDemo.Static;

namespace CylinderDemo.Implementations;

/// <summary>
/// CylB = electric cylinder implementation.
/// PLC analogy: FB_CylB.
///
/// Internal model uses an integer position (0..100),
/// but the PUBLIC interface is still all-or-nothing:
///   Extended  <-> position == 100
///   Retracted <-> position == 0
///
/// This mirrors your didactic TIA version:
/// - Constructor sets initial state
/// - SanitizeConfig clamps SpeedPerScan
/// - ExecuteCommands sets a target (intent) when enabled
/// - UpdateMotion moves position towards target scan-by-scan
/// - PublishStatus derives status from internal position
/// </summary>
public sealed class CylB : IActStatus
{
    // Private fields (PLC: VAR)
    private int _position; // 0..100
    private int _target;   // 0 or 100
    private int _step;     // sanitized speed

    public CylB()
    {
        // PLC analogy: firstScan initialization
        _position = 0;
        _target = 0;
        _step = 1;
    }

    /// <summary>
    /// One scan/update of the object.
    /// </summary>
    public void Update(IActCmd cmd, int speedPerScan)
    {
        SanitizeConfig(speedPerScan);
        ExecuteCommands(cmd);
        UpdateMotion(cmd);
        PublishStatus();
    }

    // --- "Methods" (PLC: REGIONS)

    private void SanitizeConfig(int speedPerScan)
    {
        // Equivalent to calling FC_ClampInt in TIA
        _step = FC_ClampInt.ClampInt(speedPerScan, 1, 100);
    }

    private void ExecuteCommands(IActCmd cmd)
    {
        // If not enabled, ignore commands (selection)
        if (!cmd.Enable) return;

        // Commands set target (intent), not position directly
        if (cmd.Extend)
            _target = 100;
        else if (cmd.Retract)
            _target = 0;
    }

    private void UpdateMotion(IActCmd cmd)
    {
        // Didactic policy used in your TIA code:
        // If disabled -> stop motion (hold position)
        if (!cmd.Enable)
            _target = _position;

        if (_position < _target)
        {
            _position = Math.Min(_position + _step, _target);
        }
        else if (_position > _target)
        {
            _position = Math.Max(_position - _step, _target);
        }
    }

    private void PublishStatus()
    {
        // Public properties are derived; nothing to do here.
    }

    // Public interface (PLC: Status)
    public bool Extended => _position == 100;
    public bool Retracted => _position == 0;

    // Debug helpers (watch table)
    public int DebugPosition => _position;
    public int DebugTarget => _target;
    public int DebugStep => _step;
}
