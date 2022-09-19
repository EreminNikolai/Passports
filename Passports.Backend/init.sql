CREATE DATABASE testdb;

\c testdb

CREATE TABLE public.passports (
	id int4 NULL GENERATED ALWAYS AS IDENTITY,
	series int4 NOT NULL DEFAULT 0,
	"number" int4 NOT NULL DEFAULT 0,
	CONSTRAINT passports_un UNIQUE (id)
);

insert into public.passports(series,number) values (1,1);
insert into public.passports(series,number) values (1111,111111);
insert into public.passports(series,number) values (2222,222222);
