# ArchsimLibraryData

The Archsim Library Data Forman is now open source in order to facilitate data exchange with other energy modeling software.
While some parts of the format will change significantly, others are stable. The below description provides and overview.

#H2 Version 1:
Low level classes such as 
``` LibraryComponent, BaseMaterial, OpaqueMaterial, OpaqueConstruction, GlazingConstructionSimple, DaySchedule, WeekSchedule, YearSchedule, ScheduleArray ```
 will not change in the near future. An exception are the GlazingMaterial and GlazingConstruction classes.
High level classes such as 
``` ZoneDefinition ```
are subject to change in the near future.
