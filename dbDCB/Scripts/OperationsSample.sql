Delete FROM Operations;

SET IDENTITY_INSERT [dbo].[Operations] ON
	INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (1, N'2015-10-07 01:51:23', N'/2/1/', N'/1/1/', CAST(4000.0000 AS Decimal(19, 4)), N'')
	INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (2, N'2015-10-07 01:49:46', N'/1/3/', N'/3/1/', CAST(1000.0000 AS Decimal(19, 4)), N'')
	INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (3, N'2015-10-08 01:49:59', N'/2/2/', N'/1/1/', CAST(1000.0000 AS Decimal(19, 4)), N'')
	INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (4, N'2015-10-08 01:50:10', N'/1/1/', N'/3/1/', CAST(100.0000 AS Decimal(19, 4)), N'note')
	INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (5, N'2015-10-05 22:16:34', N'/2/', N'/3/', CAST(1.1000 AS Decimal(19, 4)), N'')
	INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (6, N'2015-10-08 01:50:46', N'/2/1/', N'/3/2/', CAST(800.0000 AS Decimal(19, 4)), N'')
	INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (7, N'2015-10-08 01:51:06', N'/1/1/', N'/3/3/', CAST(500.0000 AS Decimal(19, 4)), N'')
	INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (8, N'2015-10-08 22:35:11', N'/1/1/', N'/1/3/', CAST(500.0000 AS Decimal(19, 4)), N'')
SET IDENTITY_INSERT [dbo].[Operations] OFF
