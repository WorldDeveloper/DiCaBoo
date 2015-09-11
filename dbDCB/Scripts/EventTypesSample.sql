SET IDENTITY_INSERT [dbo].[EventTypes] ON
IF (SELECT COUNT(*) FROM EventTypes)<3
BEGIN
Insert into EventTypes(id, eventtype) Values(1,'Birthday');
Insert into EventTypes(id, eventtype) Values(2,'Holiday');
Insert into EventTypes(id, eventtype) Values(3,'Meeting');
END
SET IDENTITY_INSERT [dbo].[EventTypes] OFF