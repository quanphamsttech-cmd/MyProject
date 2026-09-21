SELECT MigrationId
FROM dbo.__EFMigrationsHistory
WHERE MigrationId LIKE '%AdmissionPeriod%'
ORDER BY MigrationId;
