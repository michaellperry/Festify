SELECT TOP(2) [s].[ShowGuid], [t0].[Title], [t0].[ModifiedDate]
FROM [Show] AS [s]
LEFT JOIN (
    SELECT [t].[ShowId], [t].[Title], [t].[ModifiedDate]
    FROM (
        SELECT [s0].[ModifiedDate], [s0].[ShowId], [s0].[Title],
		    ROW_NUMBER() OVER(
			    PARTITION BY [s0].[ShowId]
				ORDER BY [s0].[ModifiedDate] DESC)
			AS [row]
        FROM [ShowDescription] AS [s0]
    ) AS [t]
    WHERE [t].[row] <= 1
) AS [t0] ON [s].[ShowId] = [t0].[ShowId]
WHERE [s].[ShowGuid] = '471E3620-D24C-449D-8547-A84EF8EA40ED'
