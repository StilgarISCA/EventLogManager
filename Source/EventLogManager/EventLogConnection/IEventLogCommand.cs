﻿
using System.Collections.ObjectModel;

namespace EventLogManager.Command
{
   public interface IEventLogCommand
   {
      void CreateEventSource( string newEventSourceName, string targetEventLogName );
      bool DoesEventLogExist( string eventLogName );
      bool DoesEventSourceExist( string eventSourceName );
      Collection<string> GetEventLogs();
      Collection<string> GetEventLogSources( string eventLogName );
   }
}
