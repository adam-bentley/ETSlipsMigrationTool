# ETSlipsMigrationTool
The ETSlips migration tool takes data from the old GoDaddy MySQL database and transfers it to an AzureSQL server in the cloud. It: 
1. Fetches all the data from MySQL server.
2. Deletes all the data from the AzureSQL server.
3. Uploads the new data into the AzureSQL server.

##  Table Structure
Data is migrated from the following tables:
``` 
CREATE TABLE "categories" (
  "id" int NOT NULL IDENTITY PRIMARY KEY,
  "name" varchar(32) NOT NULL,
  CONSTRAINT u_cat_name UNIQUE(name),
);

CREATE TABLE "events" (
  "id" int NOT NULL IDENTITY PRIMARY KEY ,
  "name" varchar(60) NOT NULL,
  CONSTRAINT u_event_name UNIQUE(name),
);

CREATE TABLE "pairs" (
  "timestamp" datetime NOT NULL PRIMARY KEY,
  "event" int NOT NULL,
  "category" int NOT NULL,
  "round" varchar(3) NOT NULL,
  "finish" int NOT NULL,
  CONSTRAINT fk_pairs_event FOREIGN KEY (event) REFERENCES events(id),
  CONSTRAINT fk_pairs_categories FOREIGN KEY (category) REFERENCES categories(id)
); 

CREATE TABLE "prefixs" (
  "id" int NOT NULL IDENTITY PRIMARY KEY,
  "category_id" int NOT NULL,
  "name" varchar(12) NOT NULL,
  CONSTRAINT u_prefix_name UNIQUE(name, category_id),
  CONSTRAINT fk_prefixs_categories FOREIGN KEY (category_id) REFERENCES categories(id)
);


CREATE TABLE "runs" (
  "timestamp" datetime NOT NULL,
  "racenumber" varchar(8) NOT NULL,
  "prefix" int DEFAULT NULL,
  "drivername" text,
  "lane" varchar(1) NOT NULL,
  "index" decimal(4,2) DEFAULT NULL,
  "reaction" decimal(6,4) NOT NULL,
  "et60" decimal(6,4) DEFAULT NULL,
  "et330" decimal(6,4) DEFAULT NULL,
  "et594" decimal(6,4) DEFAULT NULL,
  "et660" decimal(6,4) DEFAULT NULL,
  "sp660" decimal(5,2) DEFAULT NULL,
  "et934" decimal(6,4) DEFAULT NULL,
  "et1000" decimal(6,4) DEFAULT NULL,
  "sp1000" decimal(5,2) DEFAULT NULL,
  "et1254" decimal(6,4) DEFAULT NULL,
  "et1320" decimal(6,4) DEFAULT NULL,
  "sp1320" decimal(5,2) DEFAULT NULL,
  "result" varchar(255) DEFAULT NULL,
  "remarks" varchar(255) DEFAULT NULL,
  CONSTRAINT fk_runs_prefix FOREIGN KEY (prefix) REFERENCES prefixs(id),
  CONSTRAINT pk_timestamp_runs PRIMARY KEY (timestamp, lane)
); 
```

**The permit column (categories), and the flagged columns (categories, events, prefixs) are not migrated since these columns are no longer used.**

**`ET936` is renamed to `ET934` as its a typo in the original system.**

## Foreign Keys
The primary keys for the events, categories and prefixs table are assigned by the MySQL Database via `auto incrementing`. However, to keep that behaviour in the AzureSQL database, it's required to assign new primary keys. This presents a problem that foreign key constraints are no longer met during the migration. To counter this, when inserted, the `new ID` is added to a dictionary with the `old ID` as the key. Then when the foreign keys are inserted, the lookup is done, and the new key is inserted.