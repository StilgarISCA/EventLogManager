using System.Collections.ObjectModel;
using EventLogManager.Connection;
using Telerik.JustMock;
using Xunit;
using EventLogManagerString = EventLogManager.ResponseString;

namespace EventLogManager.Test
{
   public class EventLogCommanderTests
   {
      private readonly IEventLogConnection _eventLogConnection;
      private readonly EventLogCommander _eventLogCommander;

      public EventLogCommanderTests()
      {
         _eventLogConnection = Mock.Create<IEventLogConnection>();
         _eventLogCommander = new EventLogCommander( _eventLogConnection );
      }

      [Fact]
      public void InvokedWithZeroArguments_ReturnsUsageString()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] emptyArgumentArray = { };

         // Act
         returnValue = _eventLogCommander.ProcessCommand( emptyArgumentArray );

         // Assert
         Assert.Equal( EventLogManagerString.UseageStatement, returnValue );
      }

      [Fact]
      public void HelpArgument_ReturnsUsageString()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] argumentArray = { "Help" };

         // Act
         returnValue = _eventLogCommander.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( EventLogManagerString.UseageStatement, returnValue );
      }

      [Fact]
      public void ListArgument_ReturnsListOfEventLogs()
      {
         // Arrange
         string returnValue = string.Empty;
         var simulatedEventLogs = new Collection<string>() { "SomeEventLog", "AnotherEventLog", "YetAnotherEventLog" };
         string[] argumentArray = { "List" };

         Mock.Arrange( () => _eventLogConnection.GetEventLogs() ).Returns( simulatedEventLogs );

         // Act
         returnValue = _eventLogCommander.ProcessCommand( argumentArray );

         // Assert
         foreach( var eventLog in simulatedEventLogs )
         {
            Assert.Contains( eventLog, returnValue );
         }
      }

      [Fact]
      public void ListWithEventLogName_DoesNotCallGetEventLogs()
      {
         // Arrange
         var simulatedEventLogs = new Collection<string>() { "SomeEventLog", "AnotherEventLog", "YetAnotherEventLog" };
         var simulatedEventLogSources = new Collection<string>() { "SomeEventLogSource", "AnotherEventLogSource", "YetAnotherEventLogSource" };
         string[] argumentArray = { "List", "NameOfEventLog" };

         Mock.Arrange( () => _eventLogConnection.GetEventLogSources( Arg.AnyString ) ).Returns( simulatedEventLogSources );
         Mock.Arrange( () => _eventLogConnection.GetEventLogs() ).Returns( simulatedEventLogs ).OccursNever();

         // Act
         _eventLogCommander.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogConnection );
      }

      [Fact]
      public void ListWithEventLogName_ReturnsEventSourcesForGivenEventLog()
      {
         string returnValue = string.Empty;
         var simulatedEventLogSources = new Collection<string>() { "SomeEventLogSource", "AnotherEventLogSource", "YetAnotherEventLogSource" };
         string simulatedEventLogName = "NameOfEventLog";
         string[] argumentArray = { "List", simulatedEventLogName };

         Mock.Arrange( () => _eventLogConnection.GetEventLogSources( simulatedEventLogName ) ).Returns( simulatedEventLogSources );

         // Act
         returnValue = _eventLogCommander.ProcessCommand( argumentArray );

         // Assert
         foreach( var eventLogSource in simulatedEventLogSources )
         {
            Assert.Contains( eventLogSource, returnValue );
         }
      }
   }
}
