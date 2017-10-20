SELECT
Id, Login, SaltedPassword, Token, TokenExpirationDate
FROM
[dbo].[Users]
WHERE 
Login = @Login