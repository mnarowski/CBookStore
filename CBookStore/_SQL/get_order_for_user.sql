CREATE FUNCTION get_order_for_user(@id_user int) RETURNS TABLE
AS
BEGIN
	DECLARE @result TABLE(id_zamowienia int)
	DECLARE @tmpnum int;
	DECLARE @user_role int = (SELECT rola FROM [dbo].[U¿ytkownicy] WHERE id_user = @id_user);
	
	
	IF @user_role < 1 
	BEGIN
		SET @tmpnum = (SELECT count(*) FROM [dbo].[Zamówienia] WHERE id_user = @id_user AND status=0);
		
		IF @tmpnum = 0
		BEGIN
				INSERT INTO [dbo].[Zamowienia] VALUES(@id_user,0,1,0);
				SET @result = (SELECT id_zamowienia FROM [dbo].[Zamowienia] WHERE id_user = @id_user AND status=0);
		END
		
		IF @tmpnum > 0
		BEGIN
				SET @result = (SELECT id_zamowienia FROM [dbo].[Zamowienia] WHERE id_user = @id_user AND status=0);
		END
	END
	IF @user_role > 0
	BEGIN
		SET @result = (SELECT id_zamowienia FROM [dbo].[Zamowienia] WHERE id_user = @id_user AND status=0 );
	END
	RETURN @result

	END
