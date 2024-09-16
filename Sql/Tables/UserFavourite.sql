
CREATE TABLE IF NOT EXISTS UserFavourite (
	UserId					VARCHAR(40) NOT NULL	-- User/Client id
	,MovieId				INTEGER NOT NULL		-- Movie id
	,Adult					BOOLEAN NOT NULL		-- Is Adult genre?
	,BackdropPath			VARCHAR(255) NOT NULL	-- Backdrop image path
	,OriginalLanguage		VARCHAR(100) NOT NULL	-- Original language
	,OriginalTitle			VARCHAR(150) NOT NULL	-- Original title
	,Overview				VARCHAR(500) NOT NULL	-- Overview
	,Popularity				DECIMAL(12,3) NOT NULL	-- Popularity
	,PosterPath				VARCHAR(255) NOT NULL	-- Poster image path
	,ReleaseDate			VARCHAR(10) NOT NULL	-- Release date
	,Title					VARCHAR(150) NOT NULL	-- Title
	,Video					BOOLEAN NOT NULL		-- Has video?
	,VoteAverage			DECIMAL(5,3) NOT NULL	-- Vote average
	,VoteCount				INTEGER NOT NULL		-- Vote count
	,CreationTime			DATETIME NOT NULL		-- Creation Time
	,CreatorId				CHAR(36) NULL			-- Creator Id
	,PRIMARY KEY (UserId, MovieId)
	,CONSTRAINT FK1_UserFavourite FOREIGN KEY (UserId) REFERENCES User(Id)
);
