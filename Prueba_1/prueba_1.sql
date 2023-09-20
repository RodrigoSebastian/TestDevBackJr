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
    FOREIGN KEY (userId) REFERENCES usuarios(userId) ON UPDATE CASCADE ON DELETE CASCADE
) ENGINE=InnoDB;

-- Ya que no se menciona la forma en la que se deben cargar los datos, opte por facilitar la carga de datos
-- Y modifique el archivo de informacion a .csv para poder subirse de forma mas sencilla
-- Esto solo es una solucion temporal para esta prueba en especifico
-- Se puede ya que en tengo la opcion de cargar archivos locales en el servidor de mysql
LOAD DATA LOCAL INFILE 'C:/Users/rodri/Documents/Personal/Nuxiba/testdevbackjr/Prueba_1/usuarios.csv' INTO TABLE usuarios FIELDS TERMINATED BY ',' LINES TERMINATED BY '\n' IGNORE 1 ROWS;
LOAD DATA LOCAL INFILE 'C:/Users/rodri/Documents/Personal/Nuxiba/testdevbackjr/Prueba_1/empleados.csv' INTO TABLE empleados FIELDS TERMINATED BY ',' LINES TERMINATED BY '\n' IGNORE 1 ROWS;