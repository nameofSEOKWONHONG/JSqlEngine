using System.Timers;
using Timer = System.Timers.Timer;

namespace JSqlEngine;

/// <summary>
/// 싱글톤으로 동작해야 함.
/// </summary>
public class JSql : IDisposable
{
    private readonly JSqlCore _jSqlCore;
    private readonly Timer _timer;
    private bool _isWorking = false;

    public string this[string name] => _jSqlCore.GetJSql(name);
    
    private JSql(string rootPath)
    {
        _jSqlCore = new JSqlCore(rootPath);
        _timer = new Timer(10 * 1000);
        _timer.Elapsed += TimerOnElapsed;
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        if (_isWorking == false)
        {
            _isWorking = true;
            
            _jSqlCore.ReReadAsync()
                .GetAwaiter()
                .GetResult();

            _isWorking = false;
        }
    }

    public static JSql Create(string rootPath)
    {
        return new JSql(rootPath);
    }

    public async Task InitAsync()
    {
        await _jSqlCore.InitAsync();
    }

    public void ReadFor10Second()
    {
        _timer.Start();
    }

    public void Dispose()
    {
        _timer.Elapsed -= TimerOnElapsed;
        _timer.Stop();
        _timer.Dispose();
    }
}