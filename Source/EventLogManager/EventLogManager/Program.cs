using System;

namespace EventLogManager
{
   class Program
   {
      static void Main( string[] args )
      {
         var eventLogCommandLogic = new CommandLogic();

         Console.WriteLine( eventLogCommandLogic.ProcessCommand( args ) );
      }
   }
}
