create table effortRollouts(
	id int not null auto_increment primary key,
	effId int not null,
	rollId int not null,
	rolled bool not null,
	text tinytext not null,
	finalComment text not null,
	reasonForRollback tinytext not null,
	reasonForCodeFix tinytext not null,
	wasCodeFixed bool,
	departmentResponsibleForErrorId int not null
);
