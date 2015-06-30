create table efforts(
	id int not null auto_increment primary key,
	extId int not null,
	type char(1) not null,
	testedBy tinytext not null,
	approved bool not null,
	pmResource tinytext not null,
	webResource tinytext not null,
	dbResource tinytext not null,
	uatApproved bool not null,
	desc_ tinytext not null,
	maxResource tinytext not null,
	uatApprovedBy tinytext not null,
	branchFileHierarchy tinytext not null,
	environment tinytext not null,
	resolver tinytext not null
);
