create table connections(
	id int not null auto_increment primary key,
	user tinytext not null,
	passwd tinytext not null,
	serverId int not null,
	port int not null,
	transport tinytext not null,
	protocol tinytext not null,
	sspi bool not null,
	catalog tinytext not null,
	systemName tinytext not null
);
