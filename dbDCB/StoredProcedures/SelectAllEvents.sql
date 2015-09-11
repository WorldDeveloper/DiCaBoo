CREATE PROCEDURE [dbo].[SelectAllEvents]
	@from DateTime2(0),
	@to DateTime2(0)
AS
	SELECT * FROM CalendarView WHERE EventStart BETWEEN @from AND @to

