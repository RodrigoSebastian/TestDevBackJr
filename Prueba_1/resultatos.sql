-- Depurar solo los ID diferentes de 6,7,9 y 10 de la tabla usuarios (5 puntos)
DELETE FROM usuarios WHERE usuarios.userId NOT IN (6,7,9,10);
SELECT * FROM usuarios;

-- Actualizar el dato Sueldo en un 10 porciento a los empleados que tienen fechas entre el año 2000 y 2001 (5 puntos)
UPDATE empleados
SET empleados.sueldo = empleados.sueldo * 1.1
WHERE YEAR(empleados.fechaIngreso) BETWEEN 2000 AND 2001;
SELECT * FROM empleados;

-- Realiza una consulta para traer el nombre de usuario y fecha de ingreso de los usuarios que gananen mas de 10000 y su apellido comience con T ordernado del mas reciente al mas antiguo (10 puntos)
SELECT us.nombre, em.fechaIngreso FROM empleados AS em INNER JOIN usuarios AS us ON us.userId = em.userId
WHERE em.sueldo > 10000 AND us.paterno LIKE 'T%'
ORDER BY em.fechaIngreso DESC;

-- Realiza una consulta donde agrupes a los empleados por sueldo, un grupo con los que ganan menos de 1200 y uno mayor o igual a 1200, cuantos hay en cada grupo? (10 puntos)
