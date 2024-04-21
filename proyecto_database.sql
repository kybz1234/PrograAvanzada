use master

create database PrograAvanzadaGrupo1

use PrograAvanzadaGrupo1

--------------------------------------------------------------------

CREATE TABLE Estado (
    estado_id INT PRIMARY KEY IDENTITY,
    estado_nombre VARCHAR(10) NOT NULL CHECK (estado_nombre IN ('Activo', 'Inactivo'))
);

INSERT INTO Estado (estado_nombre) VALUES ('Activo'), ('Inactivo');

select * from Estado
--------------------------------------------------------------------

CREATE TABLE Usuarios (
    user_id INT PRIMARY KEY IDENTITY,
    usuario VARCHAR(50) NOT NULL,
    contraseña_hash VARCHAR(255) NOT NULL,
    ultimo_login DATETIME,
    estado_id INT NOT NULL,
    FOREIGN KEY (estado_id) REFERENCES Estado(estado_id)
);

alter table usuarios add esAdmin bit;

update usuarios set esAdmin = 1 where user_id = 1;
update usuarios set esAdmin = 0 where user_id = 2;
update usuarios set esAdmin = 0 where user_id = 3;
update usuarios set esAdmin = 0 where user_id = 6;

select * from Usuarios

--------------------------------------------------------------------

CREATE TABLE Productos (
    producto_id INT PRIMARY KEY IDENTITY,
    nombre_producto VARCHAR(100) NOT NULL,
    precio DECIMAL(10, 2) NOT NULL,
    fecha_salida DATETIME,
    cantidad INT,
    video_url VARCHAR(255),
    estado_id INT NOT NULL,
    FOREIGN KEY (estado_id) REFERENCES Estado(estado_id)
);

alter table Productos add descripcion varchar(max);

alter table Productos add foto nvarchar(max);

alter table Productos add en_descuento bit not null;

alter table Productos add precio_descuento DECIMAL(10, 2);

select * from Productos


--------------------------------------------------------------------

CREATE TABLE Carrito_de_compras (
    carrito_id INT PRIMARY KEY IDENTITY,
    user_id INT,
    producto_id INT,
    cantidad INT,
    FOREIGN KEY (user_id) REFERENCES Usuarios(user_id),
    FOREIGN KEY (producto_id) REFERENCES Productos(producto_id)
);

--------------------------------------------------------------------

CREATE TABLE Pedidos (
    orden_id INT PRIMARY KEY IDENTITY,
    user_id INT,
    fecha_pedido DATETIME,
    total DECIMAL(10, 2),
    FOREIGN KEY (user_id) REFERENCES Usuarios(user_id)
);

--------------------------------------------------------------------

CREATE TABLE Detalles_de_pedidos (
    detalle_id INT PRIMARY KEY IDENTITY,
    orden_id INT,
    product_id INT,
    cantidad INT,
    precio DECIMAL(10, 2),
    FOREIGN KEY (orden_id) REFERENCES Pedidos(orden_id),
    FOREIGN KEY (product_id) REFERENCES Productos(producto_id)
);

--------------------------------------------------------------------
--Stored Procedures

--Registrar usuario

create proc sp_RegistrarUsuario(
@nombre_usuario nvarchar(50),
@contraseña_hash nvarchar(max),
@estado_nombre nvarchar(10)
)
AS
BEGIN
	if (not exists(select * from Usuarios where usuario = @nombre_usuario))
	begin

	DECLARE @estado_id INT
    
    
    SELECT @estado_id = estado_id FROM Estado WHERE estado_nombre = @estado_nombre
    
    
    INSERT INTO Usuarios (usuario, contraseña_hash, estado_id)
    VALUES (@nombre_usuario, @contraseña_hash, @estado_id)
	end
end
go

--Registrar usuario admin

create proc sp_RegistrarUsuarioAdmin(
@nombre_usuario nvarchar(50),
@contraseña_hash nvarchar(max),
@estado_id nvarchar(10)
)
AS
BEGIN
	if (not exists(select * from Usuarios where usuario = @nombre_usuario))
	begin    
    
    INSERT INTO Usuarios (usuario, contraseña_hash, estado_id)
    VALUES (@nombre_usuario, @contraseña_hash, @estado_id)
	end
end
go

--Validar usuario

create proc sp_ValidarUsuario(
@nombre_usuario  nvarchar(450),
@contraseña_hash nvarchar(max)
)
as
begin

	if (exists(select * from Usuarios where usuario = @nombre_usuario and contraseña_hash = @contraseña_hash))
		select usuario from Usuarios where usuario = @nombre_usuario and contraseña_hash = @contraseña_hash

	DECLARE @user_id INT

	if @nombre_usuario is not null
	begin
		update Usuarios set ultimo_login = GETDATE() where usuario = @nombre_usuario
	end

	Select @user_id AS user_id

end
go

drop proc sp_ValidarUsuario


------------------------------------------------------------------------


--registrar producto

create proc sp_RegistrarProducto(
@nombre_producto VARCHAR(100),
@precio DECIMAL(10, 2),
@fecha_salida DATETIME,
@cantidad INT,
@video_url VARCHAR(255),
@estado_id INT,
@descripcion varchar(max),
@foto nvarchar(max),
@precio_descuento DECIMAL(10, 2),
@en_descuento bit
)
as
begin
	if (not exists(select * from Productos where nombre_producto = @nombre_producto))
	begin
		insert into Productos(nombre_producto,precio,fecha_salida,cantidad,video_url,estado_id,descripcion,foto,precio_descuento,en_descuento) 
					  values (@nombre_producto,@precio,@fecha_salida,@cantidad,@video_url,@estado_id,@descripcion,@foto,@precio_descuento,@en_descuento)
	end
end


select * from Usuarios