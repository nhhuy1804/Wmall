using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSWD.WmallPos.POS.FX.Win.Interfaces;
using WSWD.WmallPos.FX.Shared;
using WSWD.WmallPos.FX.Shared.Data;

namespace WSWD.WmallPos.POS.FX.Win.Data
{
    public class PostMessageHandler : IPostMessage<OPOSKeyEventArgs>
    {        
        List<IObserver<OPOSKeyEventArgs>> m_observers;

        static PostMessageHandler m_handler;

        public static PostMessageHandler Current
        {
            get
            {
                if (m_handler == null)
                {
                    m_handler = new PostMessageHandler()
                    {
                        m_observers = new List<IObserver<OPOSKeyEventArgs>>()
                    };
                }

                return m_handler;
            }
        }

        public void PostEvent(OPOSKeyEventArgs e)
        {
            foreach (var obs in m_observers)
            {
                obs.Update(this, e);
                if (e.IsHandled)
                {
                    break;
                }
            }
        }

        #region IPostMessage<OPOSKeyEventArgs> Members

        public void Attach(IObserver<OPOSKeyEventArgs> observer)
        {
            m_observers.Add(observer);
        }

        public void Detach(IObserver<OPOSKeyEventArgs> observer)
        {
            m_observers.Remove(observer);            
        }

        #endregion
    }
}
