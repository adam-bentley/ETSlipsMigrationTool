CREATE LOGIN MigrationTool 
WITH PASSWORD = '';

CREATE USER MigrationTool
FROM LOGIN MigrationTool
WITH DEFAULT_SCHEMA=dbo;

GRANT INSERT ON OBJECT::dbo.events to MigrationTool;
GRANT INSERT ON OBJECT::dbo.categories to MigrationTool;
GRANT INSERT ON OBJECT::dbo.pairs to MigrationTool;
GRANT INSERT ON OBJECT::dbo.prefixs to MigrationTool;
GRANT INSERT ON OBJECT::dbo.runs to MigrationTool;

GRANT delete ON OBJECT::dbo.events to MigrationTool;
GRANT delete ON OBJECT::dbo.categories to MigrationTool;
GRANT delete ON OBJECT::dbo.pairs to MigrationTool;
GRANT delete ON OBJECT::dbo.prefixs to MigrationTool;
GRANT delete ON OBJECT::dbo.runs to MigrationTool;