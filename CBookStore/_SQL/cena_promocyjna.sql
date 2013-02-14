CREATE FUNCTION [dbo].cena_promocyjna(@isbn char(11)) RETURNS decimal(10,2)
AS
BEGIN
	DECLARE @cena decimal(10,2) = (SELECT cena FROM Ksi¹¿ki WHERE isbn = @isbn);
	DECLARE @sum decimal(2,0) = (SELECT sum(p.obnizka) FROM Promocje p
								INNER JOIN Ksiazki_promocje kp ON kp.id_promocja = p.id_promocja
								WHERE kp.isbn = @isbn);	
	RETURN @cena-(@cena*(@sum/100))

END