using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSWD.WmallPos.POS.FX.Win.Interfaces;

namespace WSWD.WmallPos.POS.FX.Win.Data
{
    public class FrameBaseData : IFrameBaseData<FrameBaseDataChangedEventArgs>
    {
        List<IObserver<FrameBaseDataChangedEventArgs>> m_observers;
        static FrameBaseData m_globalData;
        public static FrameBaseData Current
        {
            get
            {
                if (m_globalData == null)
                {
                    m_globalData = new FrameBaseData()
                    {
                        m_observers = new List<IObserver<FrameBaseDataChangedEventArgs>>()
                    };
                }

                return m_globalData;
            }
        }

        /// <summary>
        /// Update data to observer
        /// </summary>
        /// <param name="dataItem"></param>
        /// <param name="dataValue"></param>
        public void OnDataChanged(FrameBaseDataItem dataItem, object dataValue)
        {
            var e = new FrameBaseDataChangedEventArgs()
            {
                ChangedItem = dataItem
            };

            if (dataItem != FrameBaseDataItem.AllItem)
            {
                var pi = e.GetType().GetProperty(dataItem.ToString());
                string propName = pi.Name;
                pi.SetValue(e, dataValue, null);
            }

            foreach (var obs in m_observers)
            {
                obs.Update(this, e);
            }
        }

        public void Attach(IObserver<FrameBaseDataChangedEventArgs> observer)
        {
            m_observers.Add(observer);
        }

        public void Detach(IObserver<FrameBaseDataChangedEventArgs> observer)
        {
            m_observers.Remove(observer);
        }
    }
}
