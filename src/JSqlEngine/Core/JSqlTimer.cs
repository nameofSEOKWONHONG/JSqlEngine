using System.Timers;
using Timer = System.Timers.Timer;

namespace JSqlEngine;

/// <summary>
/// 싱글톤으로 동작해야 함.
/// </summary>
public class JSqlTimer : IDisposable
{
    private readonly JSqlReader _jSqlReader;
    private readonly Timer _timer;
    private bool _isWorking = false;

    public string this[string name] => _jSqlReader.GetJSql(name);
    
    public JSqlTimer(JSqlReader jSqlReader)
    {
        _jSqlReader = jSqlReader;
        _timer = new Timer(10 * 1000);
        _timer.Elapsed += TimerOnElapsed;
    }

    private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
    {
        if (_isWorking == false)
        {
            _isWorking = true;
            _jSqlReader.Reload();
            _isWorking = false;
        }
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