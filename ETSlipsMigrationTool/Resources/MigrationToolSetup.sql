/* On master database */
CREATE LOGIN MigrationTool 
WITH PASSWORD = '';

/* On ETSlips database */
CREATE USER MigrationTool
FROM LOGIN MigrationTool
WITH DEFAULT_SCHEMA=dbo;

/* Insert permsions on all tables */
GRANT INSERT ON OBJECT::dbo.events to MigrationTool;
GRANT INSERT ON OBJECT::dbo.categories to MigrationTool;
GRANT INSERT ON OBJECT::dbo.pairs to MigrationTool;
GRANT INSERT ON OBJECT::dbo.prefixes to MigrationTool;
GRANT INSERT ON OBJECT::dbo.runs to MigrationTool;

/* Delete permisions on all tables */
GRANT delete ON OBJECT::dbo.events to MigrationTool;
GRANT delete ON OBJECT::dbo.categories to MigrationTool;
GRANT delete ON OBJECT::dbo.pairs to MigrationTool;
GRANT delete ON OBJECT::dbo.prefixes to MigrationTool;
GRANT delete ON OBJECT::dbo.runs to MigrationTool;

/* Clean up */
/* ETSlips Table */
DROP USER MigrationTool

/* Master table */
DROP login MigrationTool
