
CREATE TABLE IF NOT EXISTS User (
	Id						VARCHAR(40) NOT NULL	-- User/Client Id
	,ConcurrencyStamp		VARCHAR(40) NOT NULL	-- Concurrency Stamp
	,CreationTime			DATETIME NOT NULL		-- Creation Time
	,CreatorId				CHAR(36) NULL			-- Creator Id
	,LastModificationTime	DATETIME NULL			-- Last Modification Time
	,LastModifierId			VARCHAR(20) NULL		-- Last Modifier Id
	,ExtraProperties		LONGTEXT NULL			-- Store extra properties as JSON
	,PRIMARY KEY (Id)
);
