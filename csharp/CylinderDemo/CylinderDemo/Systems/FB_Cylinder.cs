using CylinderDemo.Implementations;
using CylinderDemo.Interfaces;
using CylinderDemo.Static;

namespace CylinderDemo.Systems;

/// <summary>
/// FB_Cylinder = container / orchestrator.
/// PLC analogy: FB_Cylinder that owns CylA and CylB as multi-instances,
/// and is called cyclically from OB1.
///
/// This object:
/// - builds command objects for each cylinder (like BuildCommands region in TIA)
/// - supplies external conditions / configs (PressureOk, SpeedPerScan)
/// - executes each cylinder once per scan (like calling FBs every cycle)
/// - exposes published status for a "watch table" style output
/// </summary>
public sealed class FB_Cylinder
{
    // "Member objects" (PLC: multi-instances)
    private readonly CylA _cylA = new();
    private readonly CylB _cylB = new();

    // External conditions / parameters (PLC: inputs to FB_Cylinder)
    public bool PressureOk { get; set; } = true;
    public int SpeedPerScan { get; set; } = 10;

    // High-level command inputs (PLC: ExtendCylA/RetractCylA etc.)
    public bool ExtendCylA { get; set; }
    public bool RetractCylA { get; set; }
    public bool ExtendCylB { get; set; }
    public bool RetractCylB { get; set; }

    /// <summary>
    /// One scan of the container object.
    /// PLC analogy: one call of FB_Cylinder from OB1.
    /// </summary>
    public void Update()
    {
        // Build per-cylinder commands (PLC: FC_BuildActCmd)
        var cmdA = FC_BuildActCmd.BuildActCmd(
            enable: ExtendCylA || RetractCylA,
            extend: ExtendCylA,
            retract: RetractCylA);

        var cmdB = FC_BuildActCmd.BuildActCmd(
            enable: ExtendCylB || RetractCylB,
            extend: ExtendCylB,
            retract: RetractCylB);

        // Execute objects (PLC: call CylA(...) / CylB(...) each scan)
        _cylA.Update(cmdA, pressureOk: PressureOk);
        _cylB.Update(cmdB, speedPerScan: SpeedPerScan);
    }

    // Public status outputs (PLC: outputs of FB_Cylinder)
    public IActStatus CylA_Status => _cylA;
    public IActStatus CylB_Status => _cylB;

    // Debug-only for console watch table (PLC: monitoring internal vars)
    public (bool ValveExtend, bool ValveRetract) CylA_DebugValves => _cylA.DebugValves;
    public (int Position, int Target, int Step) CylB_Debug => (_cylB.DebugPosition, _cylB.DebugTarget, _cylB.DebugStep);
}
