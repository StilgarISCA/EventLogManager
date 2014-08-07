using System;

namespace EventLogManager
{
   class Program
   {
      static void Main( string[] args )
      {
         var eventLogCommander = new EventLogCommander();

         Console.WriteLine( eventLogCommander.ProcessCommand( args ) );
      }
   }
}
