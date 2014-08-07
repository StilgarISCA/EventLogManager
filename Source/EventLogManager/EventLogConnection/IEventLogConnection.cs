
using System.Collections.ObjectModel;

namespace EventLogManager.Connection
{
   public interface IEventLogConnection
   {
      Collection<string> GetEventLogs();
   }
}
