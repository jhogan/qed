create table rollLogs(
	id int not null auto_increment primary key,
	text text not null,
	userEmail tinytext not null,
	rollClass tinytext not null,
	rollType tinytext not null,
	ts TimeStamp
);
