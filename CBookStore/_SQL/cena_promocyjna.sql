CREATE FUNCTION cena_promocyjna(@isbn char(11)) RETURNS decimal(10,2)
AS
BEGIN
	DECLARE @cena_ostateczna decimal(10,2);
	DECLARE @promocja_id int;
	DECLARE @obnizka decimal(2,0);
	SET @cena_ostateczna = (SELECT cena FROM [Ksi¹¿ki] WHERE isbn =@isbn);
	DECLARE nCursor CURSOR FOR SELECT id_promocja FROM [Ksiazki_promocje] WHERE isbn =@isbn;
	
	OPEN nCursor;
	
	FETCH NEXT FROM nCursor INTO @promocja_id;
	WHILE @@FETCH_STATUS=0
	BEGIN
		SET @obnizka = @obnizka + (SELECT obnizka FROM [Promocje] WHERE id_promocja = @promocja_id AND GETDATE() BETWEEN data_poczatek AND data_koniec);
			FETCH NEXT FROM nCursor INTO @promocja_id;
	END 
	
	CLOSE nCursor;
	DEALLOCATE nCursor;
	SET @cena_ostateczna = @cena_ostateczna - (@obnizka * @cena_ostateczna);
	RETURN @cena_ostateczna
END