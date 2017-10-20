UPDATE  [dbo].[Users] 
SET
Token = @Token, 
TokenExpirationDate = @TokenExpirationDate
WHERE
Login = @Login