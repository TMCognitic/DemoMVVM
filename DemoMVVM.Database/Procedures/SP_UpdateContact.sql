CREATE PROCEDURE [dbo].[SP_UpdateContact]
    @Id int,
	@LastName NVARCHAR(50), 
    @FirstName NVARCHAR(50), 
    @Email NVARCHAR(320)
AS
Begin
    Update Contact 
    Set LastName = @LastName, FirstName = @FirstName, Email = @Email
    WHERE Id = @Id;
End
