-- usuarios con las siguientes columnas
-- userId int
-- Login varchar(100)
-- Nombre varchar(100)
-- Paterno varchar(100)
-- Materno varchar(100)

-- empleados esta debe tener relacion la tabla usuarios, con las siguientes columnas
-- userId int
-- Sueldo double
-- FechaIngreso date

CREATE SCHEMA IF NOT EXISTS nuxiba;
USE nuxiba;

DROP TABLE IF EXISTS empleados;
DROP TABLE IF EXISTS usuarios;

CREATE TABLE usuarios (
    userId int NOT NULL AUTO_INCREMENT,
    login VARCHAR(100) NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    paterno VARCHAR(100) NOT NULL,
    materno VARCHAR(100) NOT NULL,
    PRIMARY KEY (userId)
) ENGINE=InnoDB;

CREATE TABLE empleados (
    userId int NOT NULL,
    sueldo double NOT NULL,
    fechaIngreso date NOT NULL,
    FOREIGN KEY (userId) REFERENCES usuarios(userId) ON UPDATE CASCADE
) ENGINE=InnoDB;
