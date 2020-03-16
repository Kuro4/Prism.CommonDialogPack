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
        /// 現在の値
        /// </summary>
        public T Current
        {
            get { return this.current; }
            private set 
            {
                SetProperty(ref this.current, value);
                this.CanGoForward = this.Backward.Any();
                this.CanGoBackward = this.Forward.Any();
            }
        }

        private bool canGoForward = false;
        /// <summary>
        /// 前進可能かどうか
        /// </summary>
        public bool CanGoForward
        {
            get { return this.canGoForward; }
            private set { SetProperty(ref this.canGoForward, value); }
        }

        private bool canGoBackward = false;
        /// <summary>
        /// 後退可能かどうか
        /// </summary>
        public bool CanGoBackward
        {
            get { return this.canGoBackward; }
            private set { SetProperty(ref this.canGoBackward, value); }
        }

        /// <summary>
        /// 前進履歴
        /// </summary>
        private Stack<T> Forward { get; }
        /// <summary>
        /// 後退履歴
        /// </summary>
        private Stack<T> Backward { get; }

        public History()
        {
            this.Forward = new Stack<T>();
            this.Backward = new Stack<T>();
        }

        public History(int capacity)
        {
            this.Forward = new Stack<T>(capacity);
            this.Backward = new Stack<T>(capacity);
        }

        /// <summary>
        /// 履歴に登録する
        /// </summary>
        /// <param name="value"></param>
        public void Entry(T value)
        {
            if (this.Current is null)
            {
                this.Current = value;
                return;
            }
            this.Forward.Push(this.Current);
            // 後退履歴を消去
            if (this.Backward.Any())
            {
                this.Backward.Clear();
            }
            this.Current = value;
        }
        /// <summary>
        /// 後退する
        /// </summary>
        public void GoBackward()
        {
            if (!this.CanGoBackward) return;
            this.Backward.Push(this.Current);
            var value = this.Forward.Pop();
            this.Current = value;
        }
        /// <summary>
        /// 前進する
        /// </summary>
        public void GoForward()
        {
            if (!this.CanGoForward) return;
            this.Forward.Push(this.Current);
            var value = this.Backward.Pop();
            this.Current = value;
        }
        /// <summary>
        /// 履歴をクリアする
        /// </summary>
        public void Clear()
        {
            this.Forward.Clear();
            this.Backward.Clear();
            this.Current = default;
        }

    }
}
