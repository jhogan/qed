create table times(
	id int not null auto_increment primary key,
	effId int not null,
	rollId int not null,
	minutes int not null,
	date datetime not null,
	text tinytext not null,
	user tinytext not null
);
