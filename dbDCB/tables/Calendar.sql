CREATE TABLE [dbo].[Calendar]
(
	[EventId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[EventTypeId] INT NOT NULL,
	[EventDateTime] DATETIME2(0) NOT NULL,
	[EventTitle] NVARCHAR(250) NOT NULL,
	[EventDescription] NVARCHAR(MAX) NULL,
	[EventVenue] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_Calendar_ToEventTypes] FOREIGN KEY (EventTypeId) REFERENCES EventTypes(Id)
)
