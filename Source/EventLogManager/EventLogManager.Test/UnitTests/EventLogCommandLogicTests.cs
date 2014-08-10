using System.Collections.ObjectModel;
using System.Globalization;
using EventLogManager.Command;
using Telerik.JustMock;
using Xunit;
using EventLogManagerString = EventLogManager.ResponseString;

namespace EventLogManager.Test.UnitTests
{
   public class EventLogCommandLogicTests
   {
      private readonly IEventLogCommand _eventLogCommand;
      private readonly CommandLogic _eventLogCommandLogic;

      public EventLogCommandLogicTests()
      {
         _eventLogCommand = Mock.Create<IEventLogCommand>();
         _eventLogCommandLogic = new CommandLogic( _eventLogCommand );
      }

      [Fact]
      public void InvokedWithZeroArguments_ReturnsUsageString()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] emptyArgumentArray = { };

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( emptyArgumentArray );

         // Assert
         Assert.Equal( EventLogManagerString.UseageStatement, returnValue );
      }

      [Fact]
      public void OneInvalidArgument_ReturnsUnknownCommandWithArgument()
      {
         // Arrange
         string returnValue = string.Empty;
         string badArgument = "commandThatDoesNotExist";
         string[] argumentArray = { badArgument };
         string expectedOutput = string.Format( CultureInfo.CurrentCulture, EventLogManagerString.UnknownCommand, badArgument );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedOutput, returnValue );
      }

      [Fact]
      public void OneInvalidArguments_ReturnsUnknownCommandWithArguments()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] argumentArray = { "commandThatDoesNotExist", "someOtherGarbageCommand" };
         string argumentString = string.Join( " ", argumentArray );
         string expectedOutput = string.Format( CultureInfo.CurrentCulture, EventLogManagerString.UnknownCommand, argumentString );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedOutput, returnValue );
      }

      [Fact]
      public void HelpArgument_ReturnsUsageString()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] argumentArray = { "Help" };

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

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

         Mock.Arrange( () => _eventLogCommand.GetEventLogs() ).Returns( simulatedEventLogs );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

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

         Mock.Arrange( () => _eventLogCommand.GetEventLogSources( Arg.AnyString ) ).Returns( simulatedEventLogSources );
         Mock.Arrange( () => _eventLogCommand.GetEventLogs() ).Returns( simulatedEventLogs ).OccursNever();

         // Act
         _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogCommand );
      }

      [Fact]
      public void ListWithEventLogName_ReturnsEventSourcesForGivenEventLog()
      {
         string returnValue = string.Empty;
         var simulatedEventLogSources = new Collection<string>() { "SomeEventLogSource", "AnotherEventLogSource", "YetAnotherEventLogSource" };
         string simulatedEventLogName = "NameOfEventLog";
         string[] argumentArray = { "List", simulatedEventLogName };

         Mock.Arrange( () => _eventLogCommand.GetEventLogSources( simulatedEventLogName ) ).Returns( simulatedEventLogSources );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         foreach( var eventLogSource in simulatedEventLogSources )
         {
            Assert.Contains( eventLogSource, returnValue );
         }
      }
   }
}
