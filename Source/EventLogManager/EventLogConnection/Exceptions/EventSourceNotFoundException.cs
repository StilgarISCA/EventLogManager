using System;
using System.Runtime.Serialization;

namespace EventLogManager.Command.Exceptions
{
   [Serializable]
   public class EventSourceNotFoundException : Exception
   {
      public EventSourceNotFoundException()
      {
      }

      public EventSourceNotFoundException( string message )
         : base( message )
      {
      }

      public EventSourceNotFoundException( string message, Exception inner )
         : base( message, inner )
      {
      }

      protected EventSourceNotFoundException(
         SerializationInfo info,
         StreamingContext context )
         : base( info, context )
      {
      }
   }
}
