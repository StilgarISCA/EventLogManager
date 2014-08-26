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
   }
}
