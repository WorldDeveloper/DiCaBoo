IF (SELECT COUNT(*) FROM Calendar)<5
BEGIN
	SET IDENTITY_INSERT [dbo].[Calendar] ON
	INSERT INTO [dbo].[Calendar] ([EventId], [EventTypeId], [EventStart], [EventEnd], [EventTitle], [EventDescription], [EventVenue]) VALUES (1, 1, N'2015-09-08 17:00:00', N'2015-12-11 00:00:00', N'Event', N'Event Description', N'somwhere')
	INSERT INTO [dbo].[Calendar] ([EventId], [EventTypeId], [EventStart], [EventEnd], [EventTitle], [EventDescription], [EventVenue]) VALUES (3, 3, N'2015-11-11 08:00:00', N'2015-12-11 10:00:00', N'Event1', N'Event Description', N'somwhere')
	INSERT INTO [dbo].[Calendar] ([EventId], [EventTypeId], [EventStart], [EventEnd], [EventTitle], [EventDescription], [EventVenue]) VALUES (4, 1, N'2015-11-11 09:00:00', N'2015-12-11 09:00:00', N'Event2', N'', N'')
	INSERT INTO [dbo].[Calendar] ([EventId], [EventTypeId], [EventStart], [EventEnd], [EventTitle], [EventDescription], [EventVenue]) VALUES (5, 2, N'2015-11-12 00:00:00', N'2015-12-11 00:00:00', N'Event3', N'Event Description', N'somwhere')
	INSERT INTO [dbo].[Calendar] ([EventId], [EventTypeId], [EventStart], [EventEnd], [EventTitle], [EventDescription], [EventVenue]) VALUES (6, 3, N'2015-11-12 11:15:00', N'2015-12-11 00:00:00', N'Event4', N'Event Description', N'')
	INSERT INTO [dbo].[Calendar] ([EventId], [EventTypeId], [EventStart], [EventEnd], [EventTitle], [EventDescription], [EventVenue]) VALUES (9, 3, N'2015-11-11 15:09:00', N'2015-12-11 23:00:00', N'Event111', N'', N'somwhere')
	INSERT INTO [dbo].[Calendar] ([EventId], [EventTypeId], [EventStart], [EventEnd], [EventTitle], [EventDescription], [EventVenue]) VALUES (10, 1, N'2015-11-11 23:00:00', N'2015-12-11 00:00:00', N'Event11', N'Event Description', N'somwhere')
	SET IDENTITY_INSERT [dbo].[Calendar] OFF
END