﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using EventLogManager.Command;

namespace EventLogManager
{
   public class CommandLogic
   {
      private readonly IEventLogCommand _eventLogCommand;

      public CommandLogic()
      {
         _eventLogCommand = new EventLogCommand();
      }

      public CommandLogic( IEventLogCommand eventLogCommand )
      {
         _eventLogCommand = eventLogCommand;
      }

      public string ProcessCommand( string[] args )
      {
         if( args == null || args.Length <= 0 )
         {
            return ResponseString.UseageStatement;
         }

         string returnMessage = string.Empty;
         string command = args[0].ToUpperInvariant();

         switch( command )
         {
            case "HELP": // Display help
               {
                  returnMessage = ResponseString.UseageStatement;
                  break;
               }
            case "LIST": // List event logs or event sources for given log
               {
                  returnMessage = ProcessListCommand( args );
                  break;
               }
            case "CREATELOG": // Create a new event log
               {
                  if( args.Length <= 2 )
                  {
                     return string.Format( CultureInfo.CurrentCulture, ResponseString.MissingArgument, args[0] );
                  }

                  string newEventLog = args[1];
                  string defaultEventSource = args[2];

                  if( _eventLogCommand.DoesEventLogExist( newEventLog ) )
                  {
                     return string.Format( CultureInfo.CurrentCulture, ResponseString.EventLogAlreadyExists, newEventLog );
                  }

                  if( !_eventLogCommand.DoesEventSourceExist( defaultEventSource ) )
                  {
                     return string.Format( CultureInfo.CurrentCulture, ResponseString.EventSourceDoesNotExist, defaultEventSource );
                  }

                  _eventLogCommand.CreateEventLog( newEventLog, defaultEventSource );
                  returnMessage = string.Empty; //string.Format( CultureInfo.CurrentCulture, ResponseString.  )
                  break;
               }
            case "CREATESOURCE": // Create a new event source in given event log
               {
                  returnMessage = ProcessCreateSourceCommand( args );
                  break;
               }
            case "DELETESOURCE": // Delete an event source
               {
                  returnMessage = ProcessDeleteSourceCommand( args );
                  break;
               }
            default: // Unknown argument(s)
               {
                  string argumentString = string.Join( " ", args );
                  returnMessage = string.Format( CultureInfo.CurrentCulture, ResponseString.UnknownCommand, argumentString );
                  break;
               }

         }

         return returnMessage;
      }

      private string ProcessDeleteSourceCommand( string[] args )
      {
         if( args.Length <= 1 )
         {
            return string.Format( CultureInfo.CurrentCulture, ResponseString.MissingArgument, args[0] );
         }

         string eventSourceToDelete = args[1];
         if( !_eventLogCommand.DoesEventSourceExist( eventSourceToDelete ) )
         {
            return string.Format( CultureInfo.CurrentCulture, ResponseString.EventSourceDoesNotExist, eventSourceToDelete );
         }

         _eventLogCommand.DeleteEventSource( eventSourceToDelete );
         return string.Format( CultureInfo.CurrentCulture, ResponseString.EventSourceDeleted, eventSourceToDelete );
      }

      private string ProcessCreateSourceCommand( string[] args )
      {
         if( args.Length <= 2 )
         {
            return string.Format( CultureInfo.CurrentCulture, ResponseString.MissingArgument, args[0] );
         }

         string newEventSource = args[1];
         string targetEventLog = args[2];

         if( !_eventLogCommand.DoesEventLogExist( targetEventLog ) )
         {
            return string.Format( CultureInfo.CurrentCulture, ResponseString.EventLogDoesNotExist, targetEventLog );
         }

         if( _eventLogCommand.DoesEventSourceExist( newEventSource ) )
         {
            return string.Format( CultureInfo.CurrentCulture, ResponseString.EventSourceAlreadyExists, newEventSource );
         }

         _eventLogCommand.CreateEventSource( newEventSource, targetEventLog );
         return string.Format( CultureInfo.CurrentCulture, ResponseString.EventSourceCreated, newEventSource, targetEventLog );
      }

      private string ProcessListCommand( string[] args )
      {
         string returnMessage = string.Empty;
         // Request for Event Log Sources
         if( args.Length > 1 )
         {
            if( !string.IsNullOrWhiteSpace( args[1] ) )
            {
               string eventLog = args[1];
               if( !_eventLogCommand.DoesEventLogExist( eventLog ) )
               {
                  returnMessage = string.Format( CultureInfo.CurrentCulture, ResponseString.EventLogDoesNotExist, eventLog );
               }
               else
               {
                  Collection<string> eventLogSources = _eventLogCommand.GetEventLogSources( eventLog );
                  returnMessage = ConvertCollectionToNewLineDelimitedString( eventLogSources );
               }
            }
            // TODO: Missing else case here for whitespace arguments
         }
         else
         {
            // Get List of event Logs
            Collection<string> eventLogs = _eventLogCommand.GetEventLogs();
            returnMessage = ConvertCollectionToNewLineDelimitedString( eventLogs );
         }
         return returnMessage;
      }

      /// <summary>
      /// Converts collections into new line delimited strings
      /// </summary>
      /// <param name="collection">Collection to convert</param>
      /// <returns>New line delimited string, String.Empty on null or empty collection</returns>
      // TODO: Move this into a utility library and write tests
      private static string ConvertCollectionToNewLineDelimitedString( Collection<string> collection )
      {
         var formattedString = new StringBuilder();

         foreach( string item in collection ?? new Collection<string>() )
         {
            formattedString.AppendLine( item );
         }

         return formattedString.ToString();
      }
   }
}
