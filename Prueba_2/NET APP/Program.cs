using System.Text;
using MySql.Data.MySqlClient;

namespace Prueba2 {
   class Program {

      static void Main(string[] args) {
         DatabaseConnectionPool pool = new DatabaseConnectionPool("SERVER=localhost;DATABASE=nuxiba;USERNAME=root;PASSWORD=;", 5);

         ListUsers(pool);
         GenerateCSV(pool);
         UpdateSalary(pool, 2, 1000);
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
         } finally {
            pool.ReleaseConnecction(connection);
         }
      }

      // Generar un archivo csv con las siguienets campos con su información(Login, Nombre completo, sueldo, fecha Ingreso) (25 puntos)
      private static void GenerateCSV(DatabaseConnectionPool pool)
      {
         MySqlConnection connection = pool.GetConnection();
         try {
            Console.WriteLine("\nGerando CSV");

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
            Console.WriteLine("CSV generado\n");
         } finally {
            pool.ReleaseConnecction(connection);
         }
      }

      // Poder actualizar el salario del algun usuario especifico (10 puntos)
      private static void UpdateSalary(DatabaseConnectionPool pool, decimal newSalary, int userID)
      {
         MySqlConnection connection = pool.GetConnection();
         try {
            string query = "UPDATE empleados SET sueldo = @salary WHERE userID = @userID";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@salary", newSalary);
            command.Parameters.AddWithValue("@userID", userID);
            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0) {
               Console.WriteLine($"Usuario {userID}, actualizado correctamente");
            } else {
               Console.WriteLine($"Usuario {userID}, no existe");
            }
         } finally {
            pool.ReleaseConnecction(connection);
         }
      }

      // Poder Tener una opcion para agregar un nuevo usuario y se pueda asiganar el salario y la fecha de ingreso por default el dia de hoy (25 puntos)
   }
}