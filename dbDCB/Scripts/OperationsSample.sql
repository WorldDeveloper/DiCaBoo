Delete FROM Operations;

SET IDENTITY_INSERT [dbo].[Operations] ON
INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (1, N'2015-10-07 17:45:03', N'/2/', N'/1/1/', CAST(500.0000 AS Decimal(19, 4)), N'')
INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (2, N'2015-10-07 17:45:22', N'/1/3/', N'/3/', CAST(1000.0000 AS Decimal(19, 4)), N'')
INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (3, N'2015-10-08 14:14:39', N'/2/', N'/1/1/', CAST(1000.0000 AS Decimal(19, 4)), N'')
INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (4, N'2015-10-08 22:22:19', N'/1/1/', N'/3/', CAST(100.0000 AS Decimal(19, 4)), N'note')
INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (5, N'2015-10-05 22:16:34', N'/2/', N'/3/', CAST(1.1000 AS Decimal(19, 4)), N'')
INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (6, N'2015-10-08 17:38:17', N'/2/', N'/1/3/', CAST(3.0000 AS Decimal(19, 4)), N'')
INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (7, N'2015-10-08 17:39:01', N'/1/1/', N'/3/', CAST(1.0000 AS Decimal(19, 4)), N'')
INSERT INTO [dbo].[Operations] ([OperationId], [OperationDate], [CreditId], [DebitId], [Summ], [Note]) VALUES (8, N'2015-10-08 22:35:11', N'/1/1/', N'/1/3/', CAST(500.0000 AS Decimal(19, 4)), N'')
SET IDENTITY_INSERT [dbo].[Operations] OFF
