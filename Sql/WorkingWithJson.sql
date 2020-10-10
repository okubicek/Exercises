create table JsonTesting(
	id int identity(1,1) not null,
	propertiesJson nvarchar(max) not null CONSTRAINT Chk_JsonTesting_propertiesJson CHECK (ISJSON(propertiesJson) > 0)
)

insert into JsonTesting values('{"name":"Ondrej", "dateOfBirth":"2012-04-23T18:25:43.511Z","detail":{"age":28, "nicknames":["blee","bli"]}}')
insert into JsonTesting values('{"name":"Somebody else", "dateOfBirth":null,"detail":{"age":34,"nicknames":[]}}')

select t.* from JsonTesting as jt 
	cross apply openjson(jt.propertiesjson) 
with( 
	name nvarchar(64) '$.name',
	age int '$.detail.age',
	dateOfBirth dateTime '$.dateOfBirth',	
	nicknames nvarchar(max) '$.detail.nicknames' as json
	) as t

select (select
	d.name as name,
	d.dateOfBirth as dateOfBirth,
	d.age as [detail.age],
	d.nicknames as [detail.nicknames]
	for json path, WITHOUT_ARRAY_WRAPPER )
from (
	select t.* from JsonTesting as jt 
		cross apply openjson(jt.propertiesjson) 
	with( 
		name nvarchar(64) '$.name',
		age int '$.detail.age',
		dateOfBirth dateTime '$.dateOfBirth',	
		nicknames nvarchar(max) '$.detail.nicknames' as json
		) as t) as d

select 
	JSON_VALUE(jt.propertiesJson, '$.name') as name,
	JSON_VALUE(jt.propertiesJson, '$.detail.age') as age,
	JSON_VALUE(jt.propertiesJson, '$.dateOfBirth') as dateOfBirth,
	json_query(jt.propertiesJson, '$.detail.nicknames') as nicknames
from JsonTesting as jt 
