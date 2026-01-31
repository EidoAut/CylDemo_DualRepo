# CylinderDemo  
### Object-Oriented Programming concepts applied to PLC logic (TIA Portal ↔ C#)

This project demonstrates how **object-oriented design principles** can be applied
to PLC programming (Siemens TIA Portal) and how the **same architecture maps 1:1 to real OOP in C#**.

The goal is not to simulate real physics, but to **teach structure, responsibility,
and clean design** for automation engineers.

---

## 🎯 Core idea

> **Same interface, different behavior.**  
> Implemented with PLC-native constructs in TIA Portal  
> and with real OOP constructs in C#.

---

## 🧠 Conceptual mapping (PLC ↔ C#)

| PLC / TIA Portal                | C# / Visual Studio              |
|--------------------------------|---------------------------------|
| OB1 (cyclic scan)              | `Program.cs` (main loop)        |
| FB (Function Block)            | `class`                         |
| Multi-instance FB              | Object composition              |
| UDT (data contract)            | `interface`                     |
| FC (stateless logic)           | `static` methods                |
| `firstScan`                    | Constructor                     |
| `REGION` as methods            | Class methods                   |
| DB / Watch table               | Console watch output             |

---

## 🏗️ Project structure

```
CylinderDemo
│
├─ Program.cs              // OB1 analogy (scan cycle)
│
├─ Interfaces              // PLC: Cyl_Interfaces
│   ├─ IActCmd.cs          // Command interface
│   └─ IActStatus.cs       // Status interface
│
├─ Implementations         // PLC: FB_CylA / FB_CylB
│   ├─ CylA.cs             // Pneumatic cylinder
│   └─ CylB.cs             // Electric cylinder
│
├─ Static                  // PLC: FCs (stateless helpers)
│   ├─ FC_ClampInt.cs
│   └─ FC_BuildActCmd.cs
│
├─ Models                  // Concrete data objects
│   └─ ActCmd.cs
│
└─ Systems
    └─ FB_Cylinder.cs      // Container / orchestrator
```

---

## 🔧 Cylinders implemented

### 🔹 CylA – Pneumatic cylinder
- Discrete behavior (extend / retract)
- Uses a permissive (`PressureOk`)
- Internal state is boolean (`isExtended / isRetracted`)
- Matches typical pneumatic PLC logic

**PLC analogy:** `FB_CylA`

---

### 🔹 CylB – Electric cylinder
- Internal continuous position (`0..100`)
- Moves gradually scan-by-scan
- Public status is still binary (Extended / Retracted)
- Demonstrates *different internal logic behind the same interface*

**PLC analogy:** `FB_CylB`

---

## 🧩 FB_Cylinder (container object)

`FB_Cylinder` is **not a base class**.

It is a **composition root / orchestrator** that:
- Owns multiple actuator objects
- Builds command objects
- Supplies external conditions (pressure, speed)
- Calls each actuator once per scan

This mirrors **multi-instance FB usage** in TIA Portal.

---

## ❗ About inheritance

This project **does NOT use inheritance in PLC code**, because:
- TIA Portal does not support class inheritance
- Pretending otherwise would be misleading

However, the design **can be understood as _conceptual inheritance_**:

- Same interfaces
- Same external behavior
- Different internal implementations

In C#, this concept is implemented using real interfaces and classes.

> **This is inheritance in design, not in PLC syntax.**

---

## ▶️ How to run

1. Open `CylinderDemo.sln` in Visual Studio
2. Run the project
3. Select which cylinder to control:
   - `1` → CylA (pneumatic)
   - `2` → CylB (electric)

### Runtime controls
- `E` → Extend selected cylinder
- `R` → Retract selected cylinder
- `P` → Toggle pressure (affects CylA)
- `Q` → Quit

The console output acts as a **watch table**, similar to TIA Portal.

---

## 📚 What this project teaches

- Encapsulation (private state vs public interface)
- Composition over inheritance
- Interface-based design
- Separation of concerns
- Clean architecture in automation software
- How PLC logic maps naturally to OOP concepts

---

## 🧠 Final takeaway

> PLCs do not prevent good software design.  
> They only force you to be explicit about it.

This project shows how to write PLC-style logic that:
- is readable
- is scalable
- is easy to teach
- and maps cleanly to real object-oriented programming.

---

## 👷 Target audience

- Automation engineers learning OOP
- PLC programmers transitioning to high-level languages
- Teachers and trainers
- Software developers curious about PLC architecture
