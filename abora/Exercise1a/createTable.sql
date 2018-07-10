create Table Users(
	id int IDENTITY(1,1) PRIMARY KEY,
	username varchar(50),
	password varchar(200)
)

create Table Rainbow(
	id int IDENTITY(1,1) PRIMARY KEY,
	password varchar(200),
	md2hash varchar(200),
	md4hash varchar(200),
	md5hash varchar(200),
	shahash varchar(200),
	sha1hash varchar(200)
)