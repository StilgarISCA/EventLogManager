using System.Collections.ObjectModel;
using System.Globalization;
using EventLogManager.Command;
using Telerik.JustMock;
using Xunit;
using Assert = Xunit.Assert;

namespace EventLogManager.Test.UnitTests.EventLogCommandLogicTests
{
   public class CommandTests
   {
      private readonly IEventLogCommand _eventLogCommand;
      private readonly CommandLogic _eventLogCommandLogic;

      public CommandTests()
      {
         _eventLogCommand = Mock.Create<IEventLogCommand>();
         _eventLogCommandLogic = new CommandLogic( _eventLogCommand );
      }

      [Fact]
      public void InvokeCommand_WithZeroArguments_ReturnsUsageString()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] emptyArgumentArray = { };

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( emptyArgumentArray );

         // Assert
         Assert.Equal( ResponseString.UseageStatement, returnValue );
      }

      [Fact]
      public void InvokeCommand_WithOneInvalidArgument_ReturnsUnknownCommandWithArgument()
      {
         // Arrange
         string returnValue = string.Empty;
         string badArgument = "commandThatDoesNotExist";
         string[] argumentArray = { badArgument };
         string expectedOutput = string.Format( CultureInfo.CurrentCulture, ResponseString.UnknownCommand, badArgument );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedOutput, returnValue );
      }

      [Fact]
      public void InvokeCommand_WithOneInvalidArgument_ReturnsUnknownCommandWithArguments()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] argumentArray = { "commandThatDoesNotExist", "someOtherGarbageCommand" };
         string argumentString = string.Join( " ", argumentArray );
         string expectedOutput = string.Format( CultureInfo.CurrentCulture, ResponseString.UnknownCommand, argumentString );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedOutput, returnValue );
      }

      [Fact]
      public void Invoke_WithHelpArgument_ReturnsUsageString()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] argumentArray = { "Help" };

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( ResponseString.UseageStatement, returnValue );
      }

      [Fact]
      public void Invoke_WithListArgument_ReturnsListOfEventLogs()
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

   }
}
