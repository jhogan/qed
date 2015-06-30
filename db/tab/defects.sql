create table defects(
	id int not null auto_increment primary key,
	desc_ text not null,
	resolver tinytext not null,
	createdBy tinytext not null,
	effId int not null,
	rollId int not null,
	forRoll bool not null
);
