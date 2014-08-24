using EventLogManager.Command;
using EventLogManager.Command.Exceptions;
using Xunit;

namespace EventLogManager.Test.IntegrationTests
{
   public class EventLogCommandTests
   {
      [Fact]
      public void GetEventLogSources_WithEventLogThatDoesNotExist_ThrowsEventLogNotFoundException()
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
      public void CreateEventSource_WithEventLogThatDoesNotExist_ThrowsEventLogNotFoundException()
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

      [Fact]
      public void DeleteEventsource_WithEventSourceThatDoesNotExist_ThrowsEventSourceNotFoundException()
      {
         // Arrange
         var eventLogCommands = new EventLogCommand();
         string eventSourceThatDoesNotExist = "AnyEventSourceThatDoesNotExist";

         // Assert
         Assert.Throws<EventSourceNotFoundException>(
            // Act
            delegate
            {
               eventLogCommands.DeleteEventSource( eventSourceThatDoesNotExist );
            } );
      }
   }
}
