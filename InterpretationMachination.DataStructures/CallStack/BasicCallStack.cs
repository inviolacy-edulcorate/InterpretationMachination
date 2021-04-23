using System;
using System.Collections.Generic;

namespace InterpretationMachination.DataStructures.CallStack
{
    public class BasicCallStack
    {
        public BasicCallStack()
        {
            Stack = new Stack<StackFrame>();
        }

        public StackFrame Top => Stack.Peek();

        private Stack<StackFrame> Stack { get; }

        public object this[string index]
        {
            get
            {
                foreach (var stackFrame in Stack)
                {
                    if (!stackFrame.Data.ContainsKey(index.ToUpper()))
                        continue;

                    return stackFrame[index.ToUpper()];
                }

                // TODO: Replace with better message. Value not on stack.
                throw new InvalidOperationException();
            }
            set
            {
                foreach (var stackFrame in Stack)
                {
                    if (!stackFrame.Data.ContainsKey(index.ToUpper()))
                        continue;

                    stackFrame[index.ToUpper()] = value;
                    
                    return;
                }

                // TODO: Replace with better message. Value not on stack.
                throw new InvalidOperationException();
            }
        }

        public StackFrame Pop()
        {
            return Stack.Pop();
        }

        public void Push(StackFrame frame)
        {
            Stack.Push(frame);
        }
    }
}