using System;
using Xunit;
using EventLogManagerString = EventLogManager.ResponseString;

namespace EventLogManager.Test
{
   public class EventLogCommanderTests : IDisposable
   {
      [Fact]
      public void InvokedWithZeroArguments_ReturnsUsageString()
      {
         // Arrange
         string returnValue = string.Empty;
         string[] emptyArgumentArray = { };

         // Act
         returnValue = EventLogCommander.ProcessCommand( emptyArgumentArray );

         // Assert
         Assert.Equal( EventLogManagerString.UseageStatement, returnValue );
      }

      public void Dispose()
      {
      }
   }
}
