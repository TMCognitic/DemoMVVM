CREATE PROCEDURE [dbo].[SP_AddContact]
	@LastName NVARCHAR(50), 
    @FirstName NVARCHAR(50), 
    @Email NVARCHAR(320)
AS
Begin
    insert into Contact (LastName, FirstName, Email)
    output inserted.Id
    values (@LastName, @FirstName, @Email);
End
