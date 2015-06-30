create table users(
	id int not null auto_increment primary key,
	email tinytext not null,
	fname tinytext not null,
	lname tinytext not null,
	passwd tinytext not null,
	qa bool not null,
	manager bool not null,
	admin bool not null
);
