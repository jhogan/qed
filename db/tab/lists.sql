create table lists(
	id int not null auto_increment primary key,
	listName text not null,
	key_ text not null,
	value text not null,
	leaf bool not null,
	parId int not null
);
