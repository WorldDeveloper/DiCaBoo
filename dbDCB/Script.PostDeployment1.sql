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

<<<<<<< HEAD

Print 'Populating table Diary.sql';
:r .\Scripts\DiarySample.sql

=======
<<<<<<< HEAD
Print 'Populating table Diary.sql';
:r .\Scripts\DiarySample.sql
=======
Print 'Populating table Diary';
:r .\Scripts\DiarySample.sql

>>>>>>> origin/master
>>>>>>> ce59f70112c692373b8aaba462652795c8583136
