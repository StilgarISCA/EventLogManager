
using System.Collections.ObjectModel;

namespace EventLogManager.Command
{
   public interface IEventLogCommand
   {
      bool DoesEventLogExist( string eventLogName );
      Collection<string> GetEventLogs();
      Collection<string> GetEventLogSources( string eventLogName );
   }
}
