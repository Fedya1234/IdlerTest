Test task

There are three buildings that produce three types of resources.
Each building has two storage units: one for input (consumed) resources and one for output (produced) resources. Both storages have limited capacity.
Resources are represented as rectangular blocks in three different colors.

Each building produces a resource over a fixed amount of time.
Production scheme:

The first building produces resource N1.

The second building consumes resource N1 and produces resource N2.

The third building consumes resources N1 and N2 and produces resource N3.

Buildings stop production in two cases:

There are no required resources in the input storage.

The output storage is full.

The player must be informed via UI text which production has stopped and for what reason.

The player controls a character (capsule) using a virtual joystick.
The character has an inventory with limited capacity for carrying resources.
Resources in the inventory are visually represented as a stack behind the character.

Resources can be collected from the output storage and delivered to the input storage when the character enters the storage trigger zone.
The transfer of each individual unit of a resource takes a certain amount of time.

All resource movements must be visualized using linear interpolation:

building → storage
storage → character
character → storage
storage → building
