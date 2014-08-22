using EventLogManager.Command;
using Telerik.JustMock;
using Xunit;
using Assert = Xunit.Assert;
using EventLogManagerString = EventLogManager.ResponseString;

namespace EventLogManager.Test.UnitTests.EventLogCommandLogicTests
{
   public class CreateSourceTests
   {
      private readonly IEventLogCommand _eventLogCommand;
      private readonly CommandLogic _eventLogCommandLogic;

      public CreateSourceTests()
      {
         _eventLogCommand = Mock.Create<IEventLogCommand>();
         _eventLogCommandLogic = new CommandLogic( _eventLogCommand );
      }

      [Fact]
      public void CreateSource_WithNoArguments_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string programCommand = "CreateSource";
         string expectedValue = string.Format( EventLogManagerString.MissingArgument, programCommand );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( new string[] { programCommand } );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }

      [Fact]
      public void CreateSource_WithInvalidEventLogName_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string eventLogThatDoesNotExist = "EventLogThatDoesNotExist";
         string expectedValue = string.Format( EventLogManagerString.EventLogDoesNotExist, eventLogThatDoesNotExist );
         string[] argumentArray = { "CreateSource", "NewEventSourceName", eventLogThatDoesNotExist };

         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( Arg.AnyString ) ).Returns( false );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }

      [Fact]
      public void CreateSource_WithEventLogName_CallsDoesEventLogExistOnce()
      {
         // Arrange
         string[] argumentArray = { "CreateSource", "NewEventSourceName", "SomeExistingEventLog" };

         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( Arg.AnyString ) ).Returns( true ).OccursOnce();

         // Act
         _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogCommand );
      }

      [Fact]
      public void CreateSource_CallsDoesEventSourceExistOnce()
      {
         // Arrange
         string[] argumentArray = { "CreateSource", "SomeEventSourceName", "SomeEventLogName" };

         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( true ).OccursOnce();

         // Act
         _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogCommand );
      }

      [Fact]
      public void CreateSource_CalledWithEventSourceThatAlreadyExists_ReturnsErrorMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string validEventLogName = "validEventLog";
         string eventSourceThatAlreadyExists = "eventSourceThatAlreadyExists";
         string expectedValue = string.Format( EventLogManagerString.EventSourceAlreadyExists, eventSourceThatAlreadyExists );
         string[] argumentArray = { "CreateSource", eventSourceThatAlreadyExists, validEventLogName };

         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( true );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedValue, returnValue );
      }

      [Fact]
      public void CreateSource_CalledWithValidArguments_CallsCreateEventSourceWithCorrectArgumentsOnce()
      {
         // Arrange
         string eventSourceName = "SomeEventSourceName";
         string eventLogName = "SomeEventLogName";
         string[] argumentArray = { "CreateSource", eventSourceName, eventLogName };

         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( false );
         Mock.Arrange( () => _eventLogCommand.CreateEventSource( eventSourceName, eventLogName ) ).OccursOnce();

         // Act
         _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Mock.Assert( _eventLogCommand );
      }

      [Fact]
      public void CreateSource_CalledWithValidArguments_ReturnsSuccessMessage()
      {
         // Arrange
         string returnValue = string.Empty;
         string eventSourceName = "SomeEventSourceName";
         string eventLogName = "SomeEventLogName";
         string expectedResult = string.Format( EventLogManagerString.EventSourceCreated, eventSourceName, eventLogName );
         string[] argumentArray = { "CreateSource", eventSourceName, eventLogName };

         Mock.Arrange( () => _eventLogCommand.DoesEventLogExist( Arg.AnyString ) ).Returns( true );
         Mock.Arrange( () => _eventLogCommand.DoesEventSourceExist( Arg.AnyString ) ).Returns( false );

         // Act
         returnValue = _eventLogCommandLogic.ProcessCommand( argumentArray );

         // Assert
         Assert.Equal( expectedResult, returnValue );
      }
   }
}
