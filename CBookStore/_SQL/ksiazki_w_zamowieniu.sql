CREATE PROCEDURE ksiazki_w_zamowieniu(@id int)
AS
BEGIN
	SELECT k.isbn, k.tytul FROM [Książki] k
	INNER JOIN Zamowienie_ksiazki zk ON k.isbn = zk.isbn
	WHERE zk.id_zamowienie = @id; 
END