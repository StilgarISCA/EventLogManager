using System.Collections.ObjectModel;

namespace EventLogManager.Connection
{
   public class EventLogConnection : IEventLogConnection
   {
      public Collection<string> GetEventLogs()
      {
         throw new System.NotImplementedException();
      }
   }
}
