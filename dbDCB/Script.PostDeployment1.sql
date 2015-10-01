/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

Print 'Inserting Settings string';

IF NOT EXISTS(SELECT * FROM Settings)
	INSERT INTO Settings VALUES(1,NULL,NULL);


Print 'Populating table AccountsSample.sql';
:r .\Scripts\AccountsSample.sql

Print 'Populating table EventTypesSample.sql';
:r .\Scripts\EventTypesSample.sql


Print 'Populating table Calendar.sql';
:r .\Scripts\CalendarSample.sql

Print 'Populating table Diary.sql';
:r .\Scripts\DiarySample.sql
