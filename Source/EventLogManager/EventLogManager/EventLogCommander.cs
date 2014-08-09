using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using EventLogManager.Connection;

namespace EventLogManager
{
   public class EventLogCommander
   {
      private readonly IEventLogConnection _eventLogConnection;

      public EventLogCommander()
      {
         _eventLogConnection = new EventLogConnection();
      }

      public EventLogCommander( IEventLogConnection eventLogConnection )
      {
         _eventLogConnection = eventLogConnection;
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
            case "HELP":
               {
                  returnMessage = ResponseString.UseageStatement;
                  break;
               }
            case "LIST":
               {
                  // Request for Event Log Sources
                  if( args.Length > 1 )
                  {
                     if( !string.IsNullOrWhiteSpace( args[1] ) )
                     {
                        string eventLog = args[1];
                        Collection<string> eventLogSources = _eventLogConnection.GetEventLogSources( eventLog );
                        returnMessage = ConvertCollectionToNewLineDelimitedString( eventLogSources );
                        break;
                     }
                  }
                  // Get List of event Logs
                  Collection<string> eventLogs = _eventLogConnection.GetEventLogs();
                  returnMessage = ConvertCollectionToNewLineDelimitedString( eventLogs );
                  break;
               }
            default:
               {
                  string argumentString = string.Join( " ", args );
                  returnMessage = string.Format( CultureInfo.CurrentCulture, ResponseString.UnknownCommand, argumentString );
                  break;
               }

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
