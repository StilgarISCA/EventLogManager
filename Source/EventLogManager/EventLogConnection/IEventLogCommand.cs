
using System.Collections.ObjectModel;

namespace EventLogManager.Command
{
   public interface IEventLogCommand
   {
      Collection<string> GetEventLogs();
      Collection<string> GetEventLogSources( string eventLogName );
   }
}
