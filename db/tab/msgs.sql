create table msgs(
	id int not null auto_increment primary key,
	text text not null,
	rollId int not null,
	effId int not null,
	ts timestamp not null,
	createdBy tinytext not null
);
