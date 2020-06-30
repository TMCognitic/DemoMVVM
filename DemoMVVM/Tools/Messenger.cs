using System;
using System.Collections.Generic;
using System.Text;

namespace DemoMVVM.Tools
{
    public class Messenger<TMessage>
    {
        private static Messenger<TMessage> _instance;

        public static Messenger<TMessage> Instance
        {
            get
            {
                return _instance ?? (_instance = new Messenger<TMessage>());
            }
        }

        private Action<TMessage> _broadcast;
        private Dictionary<string, Action<TMessage>> _boxes;

        private Messenger()
        {
            _boxes = new Dictionary<string, Action<TMessage>>();
        }

        public void Register(Action<TMessage> method)
        {
            _broadcast += method;
        }

        public void Register(string name, Action<TMessage> method)
        {
            if (!_boxes.ContainsKey(name))
                _boxes.Add(name, null);

            _boxes[name] += method;
        }

        public void Unregister(Action<TMessage> method)
        {
            _broadcast -= method;
        }

        public void Unregister(string name, Action<TMessage> method)
        {
            if (!_boxes.ContainsKey(name))
                return;

            _boxes[name] -= method;
        }

        public void Send(TMessage message)
        {
            _broadcast?.Invoke(message);
        }

        public void Send(string name, TMessage message)
        {
            if (!_boxes.ContainsKey(name))
                throw new InvalidOperationException();

            _boxes[name]?.Invoke(message);
        }
    }
}
