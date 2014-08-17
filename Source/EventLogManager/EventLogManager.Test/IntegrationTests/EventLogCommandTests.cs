using EventLogManager.Command;
using EventLogManager.Command.Exceptions;
using Xunit;

namespace EventLogManager.Test.IntegrationTests
{
   public class EventLogCommandTests
   {
      [Fact]
      public void GetEventLogSources_CalledWithEventLogThatDoesNotExist_ThrowsEventLogCommandLogNotFoundException()
      {
         // Arrange
         var eventLogCommands = new EventLogCommand();
         string eventLogThatDoesNotExist = "EventLogThatDoesNotExist";

         // Assert
         Assert.Throws<EventLogNotFoundException>(
            // Act
            delegate
            {
               eventLogCommands.GetEventLogSources( eventLogThatDoesNotExist );
            } );
      }

      [Fact]
      public void CreateEventSource_CalledWithEventLogThatDoesNotExist_ThrowsEventLogCommandLogNotFoundException()
      {
         // Arrange
         var eventLogCommands = new EventLogCommand();
         string eventLogThatDoesNotExist = "EventLogThatDoesNotExist";
         string newEventSourceName = "NewEventSourceName";

         // Assert
         Assert.Throws<EventLogNotFoundException>(
            // Act
            delegate
            {
               eventLogCommands.CreateEventSource( newEventSourceName, eventLogThatDoesNotExist );
            } );
      }
   }
}
