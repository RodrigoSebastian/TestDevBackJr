using System.Text;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace Prueba2 {
   class Program {

      static void Main(string[] args) {
         DatabaseConnectionPool pool = new DatabaseConnectionPool("SERVER=localhost;DATABASE=nuxiba;USERNAME=root;PASSWORD=;", 5);

         int opcion = 0;

         while (opcion != 5) {
            Console.WriteLine("Prueba Tecnica Nuxiba - Parte 2");
            Console.WriteLine("Rodrigo Sebastian de la Rosa Andres");
            Console.WriteLine();
            Console.WriteLine("Comandos disponibles: ");
            Console.WriteLine("1. Listar usuarios (Primeros 10)");
            Console.WriteLine("2. Generar archivo CSV");
            Console.WriteLine("3. Actualizar salario");
            Console.WriteLine("4. Agregar nuevo usuario");
            Console.WriteLine("5. SALIR");

            Console.WriteLine();
            Console.Write("Ingrese una opcion: ");
            opcion = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();

            switch (opcion)
            {
               case 1: ListUsers(pool); break;
               case 2: GenerateCSV(pool); break;
               case 3: UpdateSalary(pool); break;
               case 4: AddNewUser(pool); break;
               case 5:
                  Console.WriteLine("Adios");
                  Environment.Exit(0); break;
               default:            
                  Console.WriteLine("Funcion no disponible");
                  break;
            }

            Console.WriteLine("\nPresione ENTER para continuar");
            Console.ReadLine();
            Console.Clear();
         }
      }

      // Listar top 10 usuarios de la base antes creada (10 puntos)
      private static void ListUsers(DatabaseConnectionPool pool) {
         MySqlConnection connection = pool.GetConnection();
         try {
            string query = "SELECT * FROM usuarios LIMIT 10";
            MySqlCommand command = new MySqlCommand(query, connection);
            
            MySqlDataReader reader = command.ExecuteReader();

            Console.WriteLine("Lista de usuarios");
            while (reader.Read()) {
               Console.WriteLine($"ID: {reader.GetInt32("userID")} | Nombre: {reader.GetString("nombre")} | Paterno: {reader.GetString("paterno")} | Materno: {reader.GetString("materno")}");
            }

            reader.Close();
         } catch (Exception ex) {
            Console.WriteLine(ex.Message);
         } finally {
            pool.ReleaseConnecction(connection);
         }
      }

      // Generar un archivo csv con las siguienets campos con su información(Login, Nombre completo, sueldo, fecha Ingreso) (25 puntos)
      private static void GenerateCSV(DatabaseConnectionPool pool)
      {
         MySqlConnection connection = pool.GetConnection();
         try {
            Console.WriteLine("Gerando CSV");

            string query = "SELECT * FROM usuarios AS us INNER JOIN empleados AS em ON us.userID = em.userID;";
            MySqlCommand command = new MySqlCommand(query, connection);
            
            MySqlDataReader reader = command.ExecuteReader();
            var csv = new StringBuilder();
            csv.AppendLine("Login,Nombre Completo,Sueldo,Fecha Ingreso");

            while (reader.Read()) {
               string login = reader.GetString("login");
               string nombre = reader.GetString("nombre");
               string paterno = reader.GetString("paterno");
               string materno = reader.GetString("materno");
               string sueldo = reader.GetString("sueldo");
               string fechaIngreso = reader.GetString("fechaIngreso");

               csv.Append($"{login},{nombre} {paterno} {materno.Trim()},{sueldo},{fechaIngreso}\n");
            }
            File.WriteAllText("C:/Users/rodri/Documents/Personal/Nuxiba/testdevbackjr/Prueba_2/NET APP/info.csv", csv.ToString());
            reader.Close();
            Console.WriteLine("CSV generado");
         } catch (Exception ex) {
            Console.WriteLine(ex.Message);
         } finally {
            pool.ReleaseConnecction(connection);
         }
      }

      // Poder actualizar el salario del algun usuario especifico (10 puntos)
      private static void UpdateSalary(DatabaseConnectionPool pool)
      {
         MySqlConnection connection = pool.GetConnection();
         try {
            Console.Write("Ingresa el ID del usuario: ");

            int userID = Convert.ToInt32(Console.ReadLine());
            Console.Write("Ingresa el nuevo salario: ");
            int newSalary = Convert.ToInt32(Console.ReadLine());

            string query = "UPDATE empleados SET sueldo = @sueldo WHERE userID = @userID";

            MySqlCommand command = new MySqlCommand(query, connection);
            // Evitar SQL Injection
            command.Parameters.AddWithValue("@sueldo", newSalary);
            command.Parameters.AddWithValue("@userID", userID);
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0) {
               Console.WriteLine($"Usuario {userID}, actualizado correctamente");
            } else {
               Console.WriteLine($"Usuario {userID}, no existe");
            }
         } catch (Exception ex) {
            Console.WriteLine(ex.Message);
         } finally {
            pool.ReleaseConnecction(connection);
         }
      }

      // Poder Tener una opcion para agregar un nuevo usuario y se pueda asiganar el salario y la fecha de ingreso por default el dia de hoy (25 puntos)
      private static void AddNewUser(DatabaseConnectionPool pool)
      {
         MySqlConnection connection = pool.GetConnection();
         MySqlTransaction transaction = connection.BeginTransaction();

         try {
            Console.Write("Ingresa el login: ");
            string login = Console.ReadLine() ?? "";
            Console.Write("Ingresa el nombre: ");
            string nombre = Console.ReadLine() ?? "";
            Console.Write("Ingresa el paterno: ");
            string paterno = Console.ReadLine() ?? "";
            Console.Write("Ingresa el materno: ");
            string materno = Console.ReadLine() ?? "";
            Console.Write("Ingresa el nuevo salario: ");
            int sueldo = Convert.ToInt32(Console.ReadLine());

            string query = "INSERT INTO usuarios (login, nombre, paterno, materno) VALUES (@login, @nombre, @paterno, @materno)";
            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@nombre", nombre);
            command.Parameters.AddWithValue("@paterno", paterno);
            command.Parameters.AddWithValue("@materno", materno);

            int rowsAffected = command.ExecuteNonQuery();

            // Guard Clause
            if (rowsAffected <= 0) {
               throw new Exception("No se pudo insertar en la tabla usuarios");
            }

            long userId = command.LastInsertedId;

            query = "INSERT INTO empleados (userId, sueldo, fechaIngreso) VALUES (@userId, @sueldo, @fechaIngreso)";

            command.Parameters.Clear();
            command.CommandText = query;
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@sueldo", sueldo);
            command.Parameters.AddWithValue("@fechaIngreso", DateTime.Now);

            rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected <= 0) {
               throw new Exception("No se pudo insertar en la tabla empleados");
            }

            transaction.Commit();
            Console.WriteLine("\nUsuario agregado correctamente");
         } catch (Exception ex) {
            transaction.Rollback();
            Console.WriteLine(ex.Message);
         } finally {
            transaction.Dispose();
            pool.ReleaseConnecction(connection);
         }
      }
   }
}