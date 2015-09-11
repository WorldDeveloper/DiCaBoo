CREATE PROCEDURE [dbo].[SelectEventsByType]
	@from DateTime2(0),
	@to DateTime2(0),
	@eventTypeId int
AS
	SELECT * FROM CalendarView WHERE EventStart BETWEEN @from AND @to AND EventTypeId=@eventTypeId
