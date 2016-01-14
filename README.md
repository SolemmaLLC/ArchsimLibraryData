# Archsim Library Data

The Archsim Library Data class definitions are now open source in order to facilitate data exchange with other energy modeling software.
While some parts of the format may change significantly, others are more stable. The below description provides and overview.

### Expected changes:
Low level classes such as 
``` LibraryComponent, BaseMaterial, OpaqueMaterial, OpaqueConstruction, GlazingConstructionSimple, DaySchedule, WeekSchedule, YearSchedule, ScheduleArray ```
 will not change in the near future. An exception are the GlazingMaterial and GlazingConstruction classes.
High level classes such as 
``` ZoneDefinition ```
are subject to change in the near future.
