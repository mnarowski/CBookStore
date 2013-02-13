CREATE PROCEDURE ksiazki_w_promocji(@id_promocja int)
AS
BEGIN
	SELECT k.isbn, k.tytul FROM [Ksi¹¿ki] k
	INNER JOIN Ksiazki_promocje kp ON kp.isbn=k.isbn 
	WHERE kp.id_promocja = @id_promocja
END