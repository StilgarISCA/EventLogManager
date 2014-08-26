using System.Collections.ObjectModel;
using EventLogManager.Command;
using EventLogManager.Command.Exceptions;
using Telerik.JustMock;
using Xunit;

namespace EventLogManager.Test.UnitTests.EventLogCommandLogicTests
{
   public class ListTests
   {
      private readonly IEventLogCommand _eventLogCommand;
      private readonly CommandLogic _eventLogCommandLogic;

      public ListTests()
      {
         _eventLogCommand = Mock.Create<IEventLogCommand>();
         _eventLogCommandLogic = new CommandLogic( _eventLogCommand );
      }

      [Fact]
      public void List_WithEventLogName_DoesNotCallGetEventLogs()
      {
         // Arrange
         var simulatedEventLogs = new Collection<string>() { "SomeEventLog", "AnotherEventLog", "YetAnotherEventLog" };
         var simulatedEventLogSources = new Collection<string>() { "SomeEventLogSource", "AnotherEventLogSource", "YetAnotherEventLogSource" };
         string[] argumentArray = { "List", "NameOfEventLog" };

         Mock.Arrange( () => _eventLogCommand.GetEventLogSources( Arg.AnyString ) ).Returns( simulatedEventLogSources );
         Mock.Arrange( () => _eventLogCommand.GetEventLogs() ).Returns( simulatedEventLogs ).OccursNever();

         // Act
         _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogCommand );
      }

      [Fact]
      public void List_WithEventLogName_ReturnsEventSourcesForGivenEventLog()
      {
         // Arrange
         string returnValue = string.Empty;
         var simulatedEventLogSources = new Collection<string>() { "SomeEventLogSource", "AnotherEventLogSource", "YetAnotherEventLogSource" };
         string simulatedEventLogName = "NameOfEventLog";
         string[] argumentArray = { "List", simulatedEventLogName };

         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.GetEventLogSources( simulatedEventLogName ) ).Returns( simulatedEventLogSources );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         foreach( var eventLogSource in simulatedEventLogSources )
         {
            Assert.Contains( eventLogSource, returnValue );
         }
      }

      [Fact]
      public void List_WithEventLogName_CallsDoesEventLogExistOnce()
      {
         string returnValue = string.Empty;
         string simulatedEventLogName = "NameOfEventLog";
         var simulatedEventLogSources = new Collection<string>() { "SomeEventLogSource", "AnotherEventLogSource", "YetAnotherEventLogSource" };

         string[] argumentArray = { "List", simulatedEventLogName };

         Mock.Arrange( () => _eventLogCommand.GetEventLogSources( Arg.AnyString ) ).Returns( simulatedEventLogSources );
         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( simulatedEventLogName ) ).Returns( true ).OccursOnce();

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogCommand );
      }

      [Fact]
      public void List_WithInvalidEventLogName_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string eventLogThatDoesNotExist = "EventLogThatDoesNotExist";
         string expectedValue = string.Format( ResponseString.EventLogDoesNotExist, eventLogThatDoesNotExist );
         string[] argumentArray = { "List", eventLogThatDoesNotExist };

         Mock.Arrange( () => _eventLogCommand.GetEventLogSources( Arg.AnyString ) ).Throws<EventLogNotFoundException>();
         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( eventLogThatDoesNotExist ) ).Returns( false );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }
   }
}
