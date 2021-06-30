using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prism.CommonDialogPack.Models
{
    public class History<T> : BindableBase
    {
        private T current;
        /// <summary>
        /// Current value.
        /// </summary>
        public T Current
        {
            get { return this.current; }
            private set 
            {
                SetProperty(ref this.current, value);
                this.CanRedo = this.After.Any();
                this.CanUndo = this.Before.Any();
            }
        }

        private bool canRedo = false;
        /// <summary>
        /// Can redo.
        /// </summary>
        public bool CanRedo
        {
            get { return this.canRedo; }
            private set { SetProperty(ref this.canRedo, value); }
        }

        private bool canUndo = false;
        /// <summary>
        /// Can undo.
        /// </summary>
        public bool CanUndo
        {
            get { return this.canUndo; }
            private set { SetProperty(ref this.canUndo, value); }
        }

        /// <summary>
        /// Before.
        /// </summary>
        private Stack<T> Before { get; }
        /// <summary>
        /// After.
        /// </summary>
        private Stack<T> After { get; }

        /// <summary>
        /// Initialize a new instance of the <see cref="History{T}"/> class that is empty and has the default initial capacity.
        /// </summary>
        public History()
        {
            this.Before = new Stack<T>();
            this.After = new Stack<T>();
        }
        /// <summary>
        /// Initialize a new instance of the <see cref="History{T}"/> class that is empty has the specified initial capacity or the default initial capacity, whichever is greater.
        /// </summary>
        /// <param name="capacity"></param>
        public History(int capacity)
        {
            this.Before = new Stack<T>(capacity);
            this.After = new Stack<T>(capacity);
        }
        /// <summary>
        /// Add to history.
        /// </summary>
        /// <param name="value"></param>
        public void Entry(T value)
        {
            if (this.Current is null)
            {
                this.Current = value;
                return;
            }
            this.Before.Push(this.Current);
            if (this.After.Any())
            {
                this.After.Clear();
            }
            this.Current = value;
        }
        /// <summary>
        /// Returns the next undo value.
        /// </summary>
        /// <returns></returns>
        public T PeekUndo()
        {
            return this.Before.Peek();
        }
        /// <summary>
        /// Returns the next redo value.
        /// </summary>
        /// <returns></returns>
        public T PeekRedo()
        {
            return this.After.Peek();
        }
        /// <summary>
        /// Undo.
        /// </summary>
        public void Undo()
        {
            if (!this.CanUndo) return;
            this.After.Push(this.Current);
            var value = this.Before.Pop();
            this.Current = value;
        }
        /// <summary>
        /// Redo.
        /// </summary>
        public void Redo()
        {
            if (!this.CanRedo) return;
            this.Before.Push(this.Current);
            var value = this.After.Pop();
            this.Current = value;
        }
        /// <summary>
        /// Clear history.
        /// </summary>
        public void Clear()
        {
            this.Before.Clear();
            this.After.Clear();
            this.Current = default;
        }
    }
}
