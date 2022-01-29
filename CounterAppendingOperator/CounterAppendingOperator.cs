using BatchRenamingCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace CounterAppendingOperatorPlugin
{
    public class CounterAppendingOperator : FileRenamingOperator
    {
        protected Int64 _start;
        protected Int64 _step;

        public Int64 Start
        {
            get { return _start; }
            set { _start = value;
                NotifyPropertyChanged("Start");
            }
        }
        public Int64 Step
        {
            get { return _step; }
            set
            {
                _step= value;
                NotifyPropertyChanged("Step");
            }
        }

        public override string MagicWord => "CounterAppender";
        public override string Description => "Append a counter";

        public override void Rename(ICollection<FileNameBuilder> builders)
        {
            Int64 current = Start;
            int numOfDigits = (Start + (builders.Count-1)*Step).ToString().Length;
            foreach(FileNameBuilder file in builders)
            {
                file.Name += '_' + current.ToString().PadLeft(numOfDigits, '0');
                current += Step;
            }
        }
        public override FileRenamingOperator Clone()
        {
            return new CounterAppendingOperator() { Start = this.Start, Step = this.Step };
        }
    }
}
