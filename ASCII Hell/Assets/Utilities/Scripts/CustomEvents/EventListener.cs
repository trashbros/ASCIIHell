using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomEvents
{
    public class EventListener
    {

        /// < summary > event handler delegation </summary >
        public delegate void EventHandler(EventArgs eventArgs);
        /// < summary > set of event handlers </summary >
        public EventHandler eventHandler;

        /// < summary > Call all added events </summary >
        public void Invoke(EventArgs eventArgs)
        {
            if (eventHandler != null) eventHandler.Invoke(eventArgs);
        }

        /// < summary > Clean up all event delegations </summary >
        public void Clear()
        {
            eventHandler = null;
        }

    }
}