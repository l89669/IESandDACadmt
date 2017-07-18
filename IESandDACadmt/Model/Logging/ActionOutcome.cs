using System;

namespace IESandDACadmt.Model.Logging
{
    public class ActionOutcome
    {
        private bool _success;

        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        private String _message;

        public String Message
        {
            get { return _message; }
            set {_message = value; }
        }

        public ActionOutcome()
        {
            _success = false;
            _message = "InitialValue";
        }
        
    }
}
