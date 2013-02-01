/**
* MD5
*/
CREATE FUNCTION [dbo].MD5(@data VARCHAR) RETURNS VARCHAR(32) as
BEGIN 
	RETURN LOWER(CONVERT(VARCHAR(32), HashBytes('MD5', @data), 2));
END
/**
* Cena po zniżkach
*/
CREATE FUNCTION [dbo].PriceAfterDiscount(@book_id INTEGER) RETURNS REAL
AS
BEGIN

END