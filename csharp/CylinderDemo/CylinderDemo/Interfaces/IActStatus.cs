namespace CylinderDemo.Interfaces;

/// <summary>
/// Status data contract (PLC analogy: UDT_IActStatus).
///
/// In TIA:
///   Status.Extended / Status.Retracted is the "public interface"
///   published by each FB.
///
/// In C#, we model it as an interface.
/// </summary>
public interface IActStatus
{
    bool Extended { get; }
    bool Retracted { get; }
}
