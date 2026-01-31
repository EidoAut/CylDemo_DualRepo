namespace CylinderDemo.Interfaces;

/// <summary>
/// Command data contract (PLC analogy: UDT_IActCmd).
///
/// In the TIA project, commands are a UDT passed into each FB:
///   - Enable
///   - Extend
///   - Retract
///
/// In C#, this is a true interface contract.
/// </summary>
public interface IActCmd
{
    bool Enable { get; }
    bool Extend { get; }
    bool Retract { get; }
}
