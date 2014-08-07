using System.Collections.ObjectModel;
using EventLogManager.Connection;
using Telerik.JustMock;
using Xunit;
using EventLogManagerString = EventLogManager.ResponseString;

namespace EventLogManager.Test
{
   public class EventLogCommanderTests
   {
      private IEventLogConnection _eventLogConnection;
      private EventLogCommander _eventLogCommander;

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
      public void ListArgument_ReturnsListOfEventLogs()
      {
         // Arrange
         string returnValue = string.Empty;
         var simulatedEventLogs = new Collection<string>() { "SomeEventLog", "AnotherEventLog", "YetAnotherEventLog" };

         Mock.Arrange( () => _eventLogConnection.GetEventLogs() ).Returns( simulatedEventLogs );

         // Act
         returnValue = _eventLogCommander.ProcessCommand( new[] { "List" } );

         // Assert
         foreach( var eventLog in simulatedEventLogs )
         {
            Assert.Contains( eventLog, returnValue );
         }
      }


   }
}
