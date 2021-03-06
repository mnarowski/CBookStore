﻿/**
* MD5
*/
CREATE FUNCTION [dbo].MD5(@data VARCHAR(50)) RETURNS VARCHAR(32) as
BEGIN 
	RETURN LOWER(CONVERT(VARCHAR(32), HashBytes('MD5', @data), 2));
END
