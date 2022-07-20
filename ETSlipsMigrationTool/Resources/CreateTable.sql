DROP table runs;
DROP table pairs;
DROP table prefixes;
DROP table categories;
DROP table events;

--
-- Table structure for table "categories"
--

CREATE TABLE "categories" (
  "id" int NOT NULL IDENTITY PRIMARY KEY,
  "name" varchar(32) NOT NULL,
  CONSTRAINT u_cat_name UNIQUE(name),
);

-- --------------------------------------------------------

--
-- Table structure for table "events"
--

CREATE TABLE "events" (
  "id" int NOT NULL IDENTITY PRIMARY KEY ,
  "name" varchar(60) NOT NULL,
  CONSTRAINT u_event_name UNIQUE(name),
);

-- --------------------------------------------------------

--
-- Table structure for table "pairs"
--

CREATE TABLE "pairs" (
  "timestamp" datetime NOT NULL PRIMARY KEY,
  "event" int NOT NULL,
  "category" int NOT NULL,
  "round" varchar(3) NOT NULL,
  "finish" int NOT NULL,
  CONSTRAINT fk_pairs_event FOREIGN KEY (event) REFERENCES events(id),
  CONSTRAINT fk_pairs_categories FOREIGN KEY (category) REFERENCES categories(id)
); 

-- --------------------------------------------------------

--
-- Table structure for table "prefixes"
--

CREATE TABLE "prefixes" (
  "id" int NOT NULL IDENTITY PRIMARY KEY,
  "category_id" int NOT NULL,
  "name" varchar(12) NOT NULL,
  CONSTRAINT u_prefix_name UNIQUE(name, category_id),
  CONSTRAINT fk_prefixes_categories FOREIGN KEY (category_id) REFERENCES categories(id)
);

-- --------------------------------------------------------

--
-- Table structure for table "runs"
--

CREATE TABLE "runs" (
  "timestamp" datetime NOT NULL,
  "racenumber" varchar(8) NOT NULL,
  "prefix" int NOT NULL,
  "drivername" text,
  "lane" varchar(1) NOT NULL,
  "index" decimal(4,2) NOT NULL,
  "reaction" decimal(6,4) NOT NULL,
  "et60" decimal(6,4) NOT NULL,
  "et330" decimal(6,4) NOT NULL,
  "et594" decimal(6,4) NOT NULL,
  "et660" decimal(6,4) NOT NULL,
  "sp660" decimal(5,2) NOT NULL,
  "et934" decimal(6,4) NOT NULL,
  "et1000" decimal(6,4) NOT NULL,
  "sp1000" decimal(5,2) NOT NULL,
  "et1254" decimal(6,4) NOT NULL,
  "et1320" decimal(6,4) NOT NULL,
  "sp1320" decimal(5,2) NOT NULL,
  "result" varchar(255) NOT NULL,
  "remarks" varchar(255) NOT NULL,
  CONSTRAINT fk_runs_prefix FOREIGN KEY (prefix) REFERENCES prefixes(id),
  CONSTRAINT pk_timestamp_runs PRIMARY KEY (timestamp, lane)
);