namespace github.trondr.LogViewer.Library.Module.Services.RandomLog
{

   public class RandomLogItemLogItemConnectionStringParser: IRandomLogItemConnectionStringParser
    {
       private readonly IRandomLogItemConnectionFactory _randomLogItemConnectionFactory;
       
        public RandomLogItemLogItemConnectionStringParser(IRandomLogItemConnectionFactory randomLogItemConnectionFactory)
        {
            _randomLogItemConnectionFactory = randomLogItemConnectionFactory;
        }

       public bool CanParse(string connectionString)
        {
            return IsRandomConnectionString(connectionString);
        }

        public ILogItemConnection Parse(string connectionString)
        {
            return _randomLogItemConnectionFactory.GetRandomLogItemConnection(connectionString);
        }

        private bool IsRandomConnectionString(string connectionString)
        {
            return connectionString.StartsWith("random");
        }        
    }
}