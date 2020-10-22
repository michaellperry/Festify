SELECT s.ShowId, s.ShowGuid, t0.ModifiedDate, t0.Title
FROM Show AS s
LEFT JOIN (
    SELECT t.ShowDescriptionId, t.ModifiedDate, t.ShowId, t.Title
    FROM (
        SELECT s0.ShowDescriptionId, s0.ModifiedDate, s0.ShowId, s0.Title,
            ROW_NUMBER() OVER(PARTITION BY s0.ShowId ORDER BY s0.ModifiedDate DESC) AS row
        FROM ShowDescription AS s0
    ) AS t
    WHERE t.row <= 1
) AS t0 ON s.ShowId = t0.ShowId
WHERE NOT (EXISTS (
    SELECT 1
    FROM ShowRemoved AS s1
    WHERE s.ShowId = s1.ShowId))