using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EventLogManager.Connection
{
   /// <summary>
   /// Encapsulate logic for managing Windows Event Logs
   /// </summary>
   public class EventLogConnection : IEventLogConnection
   {
      /// <summary>
      /// Gets a collection of event log names
      /// </summary>
      /// <returns>Collection of event log names</returns>
      public Collection<string> GetEventLogs()
      {
         // TODO: Handle SystemException which can be thrown by EventLog.GetEventLogs()
         // http://msdn.microsoft.com/en-us/library/74e2ybbs(v=vs.110).aspx

         var eventLogNames = new Collection<string>();
         EventLog[] eventLogs = EventLog.GetEventLogs();

         foreach( EventLog eventLog in eventLogs )
         {
            eventLogNames.Add( eventLog.Log );
         }

         return eventLogNames;
      }


   }
}
