using CylinderDemo.Interfaces;

namespace CylinderDemo.Models;

/// <summary>
/// Concrete command object (PLC analogy: a UDT instance stored in a DB).
///
/// In TIA you did:
///   CmdA := FC_BuildActCmd(...)
///   CmdB := FC_BuildActCmd(...)
///
/// Here the object is a simple POCO implementing IActCmd.
/// </summary>
public sealed class ActCmd : IActCmd
{
    public bool Enable { get; set; }
    public bool Extend { get; set; }
    public bool Retract { get; set; }
}
