﻿using System.Linq;
using ClassicAssist.Data.Organizer;
using ClassicAssist.Resources;
using UOC = ClassicAssist.UO.Commands;

namespace ClassicAssist.Data.Macros.Commands
{
    public static class OrganizerCommands
    {
        [CommandsDisplay( Category = nameof( Strings.Agents ) )]
        public static bool Organizing()
        {
            OrganizerManager manager = OrganizerManager.GetInstance();

            bool organizing = manager.Items.Any( o => o.IsRunning() );

            return organizing;
        }

        [CommandsDisplay( Category = nameof( Strings.Agents ) )]
        public static void Organizer( string name, object sourceContainer = null, object destinationContainer = null )
        {
            OrganizerManager manager = OrganizerManager.GetInstance();

            OrganizerEntry entry = manager.Items.FirstOrDefault( oa => oa.Name.ToLower().Equals( name.ToLower() ) );

            if ( entry == null )
            {
                UOC.SystemMessage( string.Format( Strings.Organizer___0___not_found___, name ) );
                return;
            }

            if ( sourceContainer != null && destinationContainer != null )
            {
                int sourceContainerSerial = AliasCommands.ResolveSerial( sourceContainer );
                int destinatinContainerSerial = AliasCommands.ResolveSerial( destinationContainer );

                manager.Organize( entry, sourceContainerSerial, destinatinContainerSerial ).Wait();
            }
            else
            {
                manager.Organize( entry ).Wait();
            }
        }
    }
}