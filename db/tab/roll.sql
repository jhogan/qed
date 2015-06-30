create table roll(
	id int not null auto_increment primary key,
	clientId int not null,
	scheduledDate DateTime not null,
	rolledDate  DateTime not null,
	rolled bool not null,
	rolledBack bool not null,
	finalComments text not null,
	rolledBy tinytext not null
);
