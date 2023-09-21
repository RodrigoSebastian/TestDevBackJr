using MySql.Data.MySqlClient;

// Se implemento el patron de diseño Object Pooling, para tener un mejor control de la cantidad sobre
// la cantidad de conexiones que se pueden realizar a la base de datos  -> Enfoque multihilo
// Tambien podria implementarse SINGLETON para tener una sola instancia de la conexion a la base de datos -> Enfoque solo un hilo
namespace Prueba2 {
   public class DatabaseConnectionPool
   {
      private Queue<MySqlConnection> pool;
      private int maxPoolSize;

      public DatabaseConnectionPool(string connectionString, int maxPoolSize) {
         this.maxPoolSize = maxPoolSize;
         this.pool = new Queue<MySqlConnection>(maxPoolSize);
         for (int i = 0; i < maxPoolSize; i++) {
            MySqlConnection connection = new MySqlConnection(connectionString);
            pool.Enqueue(connection);
         }
      }      

      public MySqlConnection GetConnection()
      {
         lock (pool) {
            if (pool.Count > 0) {
               MySqlConnection connection = pool.Dequeue();
               connection.Open();
               return connection;
            } else {
               throw new InvalidOperationException("No existen conexiones disponibles");
            }
         }
      }

      public void ReleaseConnecction(MySqlConnection connection) {
         lock (pool) {
            if (pool.Count < maxPoolSize) {
               connection.Close();
               pool.Enqueue(connection);
            } else {
                connection.Dispose();
            }
         }
      }
   }
}