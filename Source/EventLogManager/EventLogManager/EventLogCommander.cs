
using System;

namespace EventLogManager
{
   [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses" )]
   public static class EventLogCommander
   {
      public static string ProcessCommand( string[] args )
      {
         if( args == null || args.Length <= 0 )
         {
            return ResponseString.UseageStatement;
         }

         throw new NotImplementedException();
      }
   }
}
