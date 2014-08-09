using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Win32;

namespace EventLogManager.Connection
{
   /// <summary>
   /// Encapsulate logic for managing Windows Event Logs
   /// </summary>
   public class EventLogConnection : IEventLogConnection
   {
      /// <summary>
      /// Look at the event log and retrieve all the event log names
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

      /// <summary>
      /// Looks in the registry to retrieve a collection of all event sources
      /// for a given event log
      /// </summary>
      /// <param name="eventLogName">event log to list sources of</param>
      /// <returns>Collection of event source names</returns>
      public Collection<string> GetEventLogSources( string eventLogName )
      {
         // TODO: Handle exceptions thrown by OpenSubKey
         // http://msdn.microsoft.com/en-us/library/microsoft.win32.registrykey.getsubkeynames(v=vs.110).aspx

         var eventSourceNames = new List<string>();
         string registryLocation = string.Format( CultureInfo.InvariantCulture, @"SYSTEM\CurrentControlSet\Services\Eventlog\{0}", eventLogName );

         using( RegistryKey registryKey = Registry.LocalMachine.OpenSubKey( registryLocation ) )
         {
            if( registryKey == null )
            {
               return new Collection<string>( eventSourceNames );
            }

            eventSourceNames.AddRange( registryKey.GetSubKeyNames() );
         }

         return new Collection<string>( eventSourceNames );
      }
   }
}
