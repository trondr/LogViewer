namespace LogViewer.Library.Module.Services.RandomLog
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
            if(IsRandomConnectionString(connectionString))
            {
                return _randomLogItemConnectionFactory.GetRandomLogItemConnection(connectionString);
            }
            var message = string.Format("Invalid random connection string '{0}'. Valid {1}", connectionString, HelpString);
            throw new InvalidConnectionStringException(message);
        }

       public string HelpString { get; set; } = "Random connection string format 'random'";

       private bool IsRandomConnectionString(string connectionString)
        {
            return connectionString.StartsWith("random");
        }        
    }
}