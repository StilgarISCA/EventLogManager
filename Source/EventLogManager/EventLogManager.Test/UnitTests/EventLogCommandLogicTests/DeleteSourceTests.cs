using EventLogManager.Command;
using Telerik.JustMock;
using Xunit;
using Assert = Xunit.Assert;
using EventLogManagerString = EventLogManager.ResponseString;

namespace EventLogManager.Test.UnitTests.EventLogCommandLogicTests
{
   public class DeleteSourceTests
   {
      private readonly IEventLogCommand _eventLogCommand;
      private readonly CommandLogic _eventLogCommandLogic;

      public DeleteSourceTests()
      {
         _eventLogCommand = Mock.Create<IEventLogCommand>();
         _eventLogCommandLogic = new CommandLogic( _eventLogCommand );
      }

      [Fact]
      public void DeleteSource_WithNoArguments_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string programCommand = "DeleteSource";
         string expectedValue = string.Format( EventLogManagerString.MissingArgument, programCommand );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( new string[] { programCommand } );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }

      [Fact]
      public void DeleteSource_WithValidArguments_CallsDoesEventLogExistOnce()
      {
         // Arrange
         string[] argumentArray = { "DeleteSource", "SomeExistingEventSource" };

         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( true );

         // Act
         _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogCommand );
      }


      [Fact]
      public void DeleteSource_WithEventSourceThatDoesNotExist_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string eventSourceThatDoesNotExist = "someEventSourceThatDoesNotExist";
         string expectedValue = string.Format( EventLogManagerString.EventSourceDoesNotExist, eventSourceThatDoesNotExist );
         string[] argumentArray = { "DeleteSource", eventSourceThatDoesNotExist };

         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( false );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }

      [Fact]
      public void DeleteSource_WithValidArguments_CallsDeleteEventSourceWithCorrectArgumentsOnce()
      {
         // Arrange
         string eventSourceName = "SomeEventSourceName";
         string eventLogName = "SomeEventLogName";
         string[] argumentArray = { "DeleteSource", eventSourceName, eventLogName };

         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.DeleteEventSource( eventSourceName ) ).OccursOnce();

         // Act
         _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogCommand );
      }

      [Fact]
      public void DeteSource_WithValidArguments_ReturnsSuccessMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string eventSourceName = "SomeEventSourceName";
         string expectedResult = string.Format( EventLogManagerString.EventSourceDeleted, eventSourceName );
         string[] argumentArray = { "DeleteSource", eventSourceName };

         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( true );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedResult, returnValue );
      }
   }
}
