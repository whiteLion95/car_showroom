using System;
using System.Collections.Generic;

namespace PajamaNinja.Common
{
    public class AnalyticsEventItem
    {
        public string Message;
        public Dictionary<string, object> Properties;
    }

    public class AnalyticsEventsBuffer
    {
        public List<AnalyticsEventItem> Items { get; private set; }
        public int MaxSize { get; private set; }
        public int CurrentSize => Items.Count;

        public AnalyticsEventsBuffer(int maxSize)
        {
            MaxSize = maxSize;
            Items = new List<AnalyticsEventItem>(maxSize);
        }

        public void AddItem(AnalyticsEventItem item)
        {
            if (Items.Count < MaxSize)
                Items.Add(item);
        }

        public void AddItem(string message) => AddItem(new AnalyticsEventItem() { Message = message, Properties = null });

        public void AddItem(string message, Dictionary<string, object> properties) => AddItem(new AnalyticsEventItem() { Message = message, Properties = properties });

        public AnalyticsEventItem RemoveItem() 
        { 
            if (Items.Count > 0)
            {
                var item = Items[0];
                Items.RemoveAt(0);
                return item;
            }

            return null;
        }

    }
}
