using System.Collections.ObjectModel;
using EventLogManager.Connection;
using Telerik.JustMock;
using Telerik.JustMock.Helpers;
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
         string[] arguemntArray = { "List" };

         Mock.Arrange( () => _eventLogConnection.GetEventLogs() ).Returns( simulatedEventLogs );

         // Act
         returnValue = _eventLogCommander.ProcessCommand( arguemntArray  );

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
         string returnValue = string.Empty;
         var simulatedEventLogs = new Collection<string>() { "SomeEventLog", "AnotherEventLog", "YetAnotherEventLog" };
         string[] arguemntArray = { "List", "NameOfEventLog" };

         Mock.Arrange( () => _eventLogConnection.GetEventLogs() ).Returns( simulatedEventLogs ).OccursNever();

         // Act
         returnValue = _eventLogCommander.ProcessCommand( arguemntArray );

         // Assert
        Mock.Assert( _eventLogConnection );
      }
   }
}
