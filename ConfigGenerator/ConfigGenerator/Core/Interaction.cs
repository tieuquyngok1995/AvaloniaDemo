using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ConfigGenerator.Core
{
    /// <summary>
    /// ReactiveUIフレームワークのInteractionパターンを簡易実装したクラスです。
    /// https://www.reactiveui.net/docs/handbook/interactions/
    /// </summary>
    public sealed class Interaction<TInput, TOutput> : IDisposable, ICommand
    {
        // 登録されたインタラクションハンドラーへの参照
        private Func<TInput, Task<TOutput>>? _handler;

        /// <summary>
        /// 非同期でインタラクションを実行します。Viewから提供された結果を返します。
        /// </summary>
        /// <param name="input">入力パラメータ</param>
        /// <returns>インタラクションの結果</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Task<TOutput> HandleAsync(TInput input)
        {
            if (_handler is null)
            {
                throw new InvalidOperationException("Handler wasn't registered");
            }

            return _handler(input);
        }

        /// <summary>
        /// インタラクションにハンドラーを登録します。
        /// </summary>
        /// <param name="handler">登録するハンドラー</param>
        /// <returns>不要になった場合にメモリを解放するためのIDisposableオブジェクト</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IDisposable RegisterHandler(Func<TInput, Task<TOutput>> handler)
        {
            if (_handler is not null)
            {
                throw new InvalidOperationException("Handler was already registered");
            }

            _handler = handler;
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            return this;
        }

        /// <summary>
        /// ハンドラーの登録を解除し、リソースを解放します。
        /// </summary>
        public void Dispose()
        {
            _handler = null;
        }

        /// <summary>
        /// コマンドが実行可能かどうかを返します。
        /// </summary>
        public bool CanExecute(object? parameter) => _handler is not null;

        /// <summary>
        /// コマンドを実行します。
        /// </summary>
        public void Execute(object? parameter) => HandleAsync((TInput?)parameter!);

        /// <summary>
        /// コマンドの実行可否が変更されたときに発生します。
        /// </summary>
        public event EventHandler? CanExecuteChanged;
    }
}
