using MySql.Data.MySqlClient;

namespace Prueba2 {
   class Program {

      static void Main(string[] args) {
         DatabaseConnectionPool pool = new DatabaseConnectionPool("SERVER=localhost;DATABASE=nuxiba;USERNAME=root;PASSWORD=;", 5);

         ListUsers(pool);
      }

      // Listar top 10 usuarios de la base antes creada (10 puntos)
      private static void ListUsers(DatabaseConnectionPool pool) {
         MySqlConnection connection = pool.GetConnection();
         try {
            string query = "SELECT * FROM usuarios LIMIT 10";
            MySqlCommand command = new MySqlCommand(query, connection);
            
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read()) {
               Console.WriteLine(reader.GetString(0) + " " + reader.GetString(1));
            }
         } finally {
            pool.ReleaseConnecction(connection);
         }
      }
      // Generar un archivo csv con las siguienets campos con su información(Login, Nombre completo, sueldo, fecha Ingreso) (25 puntos)

      // Poder actualizar el salario del algun usuario especifico (10 puntos)
      // Poder Tener una opcion para agregar un nuevo usuario y se pueda asiganar el salario y la fecha de ingreso por default el dia de hoy (25 puntos)
      // PLUS: Si conoces algún patrón de diseño de software no dudes en usarlo (+ 10 puntos)
   }
}