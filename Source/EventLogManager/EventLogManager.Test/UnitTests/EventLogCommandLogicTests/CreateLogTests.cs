using EventLogManager.Command;
using Telerik.JustMock;
using Xunit;

namespace EventLogManager.Test.UnitTests.EventLogCommandLogicTests
{
   public class CreateLogTests
   {
      private readonly IEventLogCommand _eventLogCommand;
      private readonly CommandLogic _eventLogCommandLogic;

      public CreateLogTests()
      {
         _eventLogCommand = Mock.Create<IEventLogCommand>();
         _eventLogCommandLogic = new CommandLogic( _eventLogCommand );
      }

      [Fact]
      public void CreateLog_WithNoArguments_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string programCommand = "CreateLog";
         string expectedValue = string.Format( ResponseString.MissingArgument, programCommand );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( new string[] { programCommand } );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }

      [Fact]
      public void CreateLog_WithEventSourceThatDoesNotExist_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string eventSourceThatDoesNotExist = "SomeEventSourceThatDoesNotExist";
         string[] argumentArray = { "CreateLog", "SomeValidEventLogName", eventSourceThatDoesNotExist };
         string expectedValue = string.Format( ResponseString.EventSourceDoesNotExist, eventSourceThatDoesNotExist );

         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( eventSourceThatDoesNotExist ) ).Returns( false );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }

      [Fact]
      public void CreateLog_WithEventLogThatAlreadyExists_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string eventLogThatAlreadyExists = "SomeEventLogThatAlreadyExists";
         string[] argumentArray = { "CreateLog", eventLogThatAlreadyExists, "someValidEventSource" };
         string expectedValue = string.Format( ResponseString.EventLogAlreadyExists, eventLogThatAlreadyExists );

         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( eventLogThatAlreadyExists ) ).Returns( true );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }
   }
}
