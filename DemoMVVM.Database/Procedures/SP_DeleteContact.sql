CREATE PROCEDURE [dbo].[SP_DeleteContact]
	@Id int
AS
Begin
	DELETE From Contact Where Id = @Id;
End