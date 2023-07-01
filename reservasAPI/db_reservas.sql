
create database db_reservas
use db_reservas

create table clientes(
	codigo int not null identity primary key,
	nombres varchar(30) not null,
	apellidos varchar(30) not null,
	telefono varchar(9) null,
	email varchar(70) null,
)

create table paquetes_turisticos(
	codigo int not null identity primary key,
	nom_paquete varchar(100) not null,
	descripcion varchar(300) null,
	precio decimal(12,2) not null,
	fecha_ini date null,
	fecha_fin date null
)

create table destinos(
	codigo int not null identity primary key ,
	nom_destino varchar(100) not null,
	ubicacion varchar(100) not null,
	descripcion varchar(300) null,
	codigo_paquete int not null references paquetes_turisticos(codigo)
)

create table reservas(
	codigo int not null identity primary key,
	codigo_referencia varchar(10),
	fecha_reserva date,
	codigo_cliente int not null references clientes(codigo),
	codigo_paquete int not null references paquetes_turisticos(codigo)
)

go
----------Crud Reservas
alter proc select_all_reservas
as
begin
	select re.codigo as codigo_reserva, re.codigo_referencia, re.fecha_reserva, re.codigo_cliente, re.codigo_paquete,
		clientes.nombres, clientes.apellidos, clientes.telefono, clientes.email,
		pt.nom_paquete, pt.descripcion, pt.precio, pt.fecha_ini, pt.fecha_fin
	from reservas as re
	left join clientes on clientes.codigo = re.codigo_cliente
	left join paquetes_turisticos as pt on pt.codigo = re.codigo_paquete
end

go
alter proc select_reservas_codigo
	@codigo int
as
begin
	select re.codigo as codigo_reserva, re.codigo_referencia, re.fecha_reserva, re.codigo_cliente, re.codigo_paquete,
		clientes.nombres, clientes.apellidos, clientes.telefono, clientes.email,
		pt.nom_paquete, pt.descripcion, pt.precio, pt.fecha_ini, pt.fecha_fin
	from reservas as re
	left join clientes on clientes.codigo = re.codigo_cliente
	left join paquetes_turisticos as pt on pt.codigo = re.codigo_paquete
	where re.codigo = @codigo
		or clientes.codigo = @codigo
		or pt.codigo = @codigo		
end

go
alter proc insert_reservas
	@fecha_reserva date, @codigo_cliente int, @codigo_paquete int
as
begin
	begin try
		declare @registros int

		select @registros = count(codigo) from paquetes_turisticos
		where @fecha_reserva < fecha_ini
			or @fecha_reserva > fecha_fin
		and codigo = @codigo_paquete

		if(@registros > 0)
		begin
			declare @codigo_referencia varchar(10), @codigo_reserva int
			select @codigo_reserva = isnull(max(codigo), 0) + 1 from reservas

			select @codigo_referencia = concat( SUBSTRING(nombres, 1, 1), SUBSTRING(apellidos, 1, 1), @codigo_reserva)
			from clientes
			where codigo = @codigo_cliente

			
			insert into reservas(codigo_referencia, fecha_reserva, codigo_cliente, codigo_paquete)
			values (@codigo_referencia, @fecha_reserva, @codigo_cliente, @codigo_paquete)

			select @@IDENTITY as codigo
		end
		else
		begin
			select -1 as codigo
		end
	end try
	begin catch
		rollback
		select 0 as codigo
	end catch
end


go
alter proc update_reservas
	@fecha_reserva date, @codigo_cliente int, @codigo_paquete int, @codigo int
as
begin
	begin try
		declare @registros int

		select @registros = count(codigo) from paquetes_turisticos
		where @fecha_reserva < fecha_ini
			or @fecha_reserva > fecha_fin
		and codigo = @codigo_paquete

		if(@registros > 0)
		begin
			declare @codigo_referencia varchar(10), @codigo_reserva int
			select @codigo_reserva = isnull(max(codigo), 0) + 1 from reservas

			select @codigo_referencia = concat( SUBSTRING(nombres, 1, 1), SUBSTRING(apellidos, 1, 1), @codigo_reserva)
			from clientes
			where codigo = @codigo_cliente

			
			update reservas
			set	codigo_referencia = @codigo_referencia,
				fecha_reserva = @fecha_reserva,
				codigo_cliente = @codigo_cliente,
				codigo_paquete = @codigo_paquete
			where codigo = @codigo

			select @codigo as codigo
		end
		else
		begin
			select -1 as codigo
		end
	end try
	begin catch
		select 0 as codigo
	end catch
end

go
alter proc delete_reservas
	@codigo int
as
begin
	begin try
		delete from reservas
		where codigo = @codigo

		select @codigo as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end
--- fin CRUD Reservas
----------Crud clientes



go
alter proc select_all_clientes
as
begin
	select codigo, nombres, apellidos, telefono, email
	from clientes
	order by apellidos
end

go
alter proc select_clientes_codigo
	@codigo int
as
begin
	select codigo, nombres, apellidos, telefono, email
	from clientes
	where codigo = @codigo
end

go
alter proc insert_clientes
	@nombres varchar(30), @apellidos varchar(30), @telefono varchar(9), @email varchar(30)
as
begin
	begin try
		insert into clientes(nombres, apellidos, telefono, email)
		values (@nombres, @apellidos, @telefono, @email)

		select @@IDENTITY as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end

go
alter proc update_clientes
	@codigo int, @nombres varchar(30), @apellidos varchar(30), @telefono varchar(9), @email varchar(30)
as
begin
	begin try
		update clientes
		set nombres = @nombres, 
			apellidos = @apellidos, 
			telefono = @telefono, 
			email = @email
		where codigo = @codigo

		select @codigo as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end

go
alter proc delete_clientes
	@codigo int
as
begin
	begin try
		delete from clientes
		where codigo = @codigo

		select @codigo as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end

go
--- fin CRUD clientes

----------Crud paquetes_turisticos

alter proc select_all_paquetes
as
begin
	select codigo, nom_paquete, descripcion, precio, fecha_ini, fecha_fin
	from paquetes_turisticos
	order by nom_paquete
end

go

alter proc select_paquete_codigo
	@codigo int
as
begin
	select codigo, nom_paquete, descripcion, precio, fecha_ini, fecha_fin
	from paquetes_turisticos
	where codigo = @codigo
end

go

alter proc insert_paquetes
	@nom_paquete varchar(100), @descripcion varchar(300), @precio decimal(12,2), @fecha_ini date, @fecha_fin date
as
begin
	begin try
		declare @resultado int = 1
		if(@fecha_ini > @fecha_fin)
		begin
			set @resultado = -1
		end

		if(@fecha_ini < GETDATE() or @fecha_fin < GETDATE())
		begin
			set @resultado = -1
		end

		if(@resultado = 1)
		begin
			insert into paquetes_turisticos(nom_paquete, descripcion, precio, fecha_ini, fecha_fin)
			values (@nom_paquete, @descripcion, @precio, @fecha_ini, @fecha_fin)

			set @resultado = @@IDENTITY
		end
			select @resultado as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end

go

alter proc update_paquetes
	@codigo int, @nom_paquete varchar(100), @descripcion varchar(300), @precio decimal(12,2), @fecha_ini date, @fecha_fin date
as
begin
	begin try
	declare @resultado int = 1
		if(@fecha_ini > @fecha_fin)
		begin
			set @resultado = -1
		end

		if(@fecha_ini < GETDATE() or @fecha_fin < GETDATE())
		begin
			set @resultado = -1
		end

		if(@resultado = 1)
		begin
			update paquetes_turisticos
			set nom_paquete = @nom_paquete, 
			descripcion = @descripcion, 
			precio = @precio, 
			fecha_ini = @fecha_ini, 
			fecha_fin = @fecha_fin
			where codigo = @codigo

			set @resultado = @@IDENTITY
		end
			select @resultado as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end

go

alter proc delete_paquetes
	@codigo int
as
begin
	begin try
		delete from paquetes_turisticos
		where codigo = @codigo

		select @codigo as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end

--- fin CRUD paquetes_turisticos

go


----------Crud Destinos
alter proc select_all_destinos
as
begin
	select codigo, nom_destino, descripcion, ubicacion, codigo_paquete		
	from destinos
	order by nom_destino
end

go

alter proc select_destino_codigo
	@codigo int
as
begin
	select codigo, nom_destino, descripcion, ubicacion, codigo_paquete		
	from destinos
	where codigo = @codigo
end

go

alter proc insert_destinos
	@nombre varchar(100), @descripcion varchar(300), @ubicacion varchar(100), @codigo_paquete int
as
begin
	begin try
		insert into destinos(nom_destino, descripcion, ubicacion, codigo_paquete)
		values (@nombre, @descripcion, @ubicacion, @codigo_paquete)

		select @@IDENTITY as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end

go

alter proc update_destinos
	@codigo int, @nombre varchar(100), @descripcion varchar(300), @ubicacion varchar(100), @codigo_paquete int
as
begin
	begin try
		set nom_destino = @nombre,
			descripcion = @descripcion,
			ubicacion = @ubicacion,
			codigo_paquete = @codigo_paquete
			where codigo = @codigo

		select @codigo as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end

go

alter proc delete_destinos
	@codigo int
as
begin
	begin try
		delete from destinos
		where codigo = @codigo

		select @codigo as codigo
	end try
	begin catch
		select 0 as codigo
	end catch
end
----------Crud Destinos


------ SP para cosultas
go
alter proc select_all_destinos_por_paquete
	@codigo_paquete int
as
begin
	select codigo, nom_destino, descripcion, ubicacion, codigo_paquete		
	from destinos
	where codigo_paquete = @codigo_paquete
	order by nom_destino
end