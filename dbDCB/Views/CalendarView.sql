CREATE VIEW [dbo].[CalendarView]
	AS SELECT *
	FROM Calendar JOIN EventTypes ON Calendar.EventTypeId=EventTypes.Id;
