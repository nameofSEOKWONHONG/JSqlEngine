using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace JSqlEngine;

public class JSqlReader
{
    private readonly string _rootPath;
    public JSqlReader(IOptions<JSqlOption> options)
    {
        _rootPath = options.Value.Path;
    }

    private readonly ConcurrentDictionary<string, JSqlFileInfo> _jsqlFileInfos = new();

    public string GetJSql(string fileName)
    {
        if (_jsqlFileInfos.TryGetValue(fileName, out var info))
        {
            return info.Sql;
        }

        return string.Empty;
    }

    public void Initialize()
    {
        var rootFiles = SearchJSqlFiles(_rootPath);
        foreach (var item in rootFiles)
        {
            _jsqlFileInfos.TryAdd(item.FileName, item);
        }
        
        var dirs = Directory.GetDirectories(_rootPath);
        foreach (var dir in dirs)
        {
            var result = SearchJSqlFiles(dir);
            foreach (var item in result)
            {
                _jsqlFileInfos.TryAdd(item.FileName, item);
            }
        }
    }

    public void Reload()
    {
        var rootFiles = SearchJSqlFiles(_rootPath);
        foreach (var item in rootFiles)
        {
            _jsqlFileInfos.TryAdd(item.FileName, item);
        }
        
        var dirs = Directory.GetDirectories(_rootPath);
        foreach (var dir in dirs)
        {
            var result = SearchJSqlFiles(dir);
            foreach (var item in result)
            {
                var exist = _jsqlFileInfos.FirstOrDefault(m => m.Key == item.FileName);
                if (string.IsNullOrWhiteSpace(exist.Key))
                {
                    _jsqlFileInfos.TryAdd(item.FileName, item);    
                }
                else
                {
                    if (item.FileDate == exist.Value.FileDate) continue;
                    if (_jsqlFileInfos.TryUpdate(exist.Key, item, exist.Value))
                    {
                        //
                    }
                }
            }
        }
    }

    private List<JSqlFileInfo> SearchJSqlFiles(string dir)
    {
        List<JSqlFileInfo> jsqls = new();
        var files = Directory.GetFiles(dir, "*.jsql");
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            var name = fileInfo.Name;
            var date = fileInfo.LastWriteTime;
            var jsql = ReadJSqlFile(file);
            jsqls.Add(new JSqlFileInfo()
            {
                FileName = name,
                FileDate = date.ToString("yyyy-MM-dd HH:mm:ss"),
                Sql = jsql
            });
        }

        return jsqls;
    }

    private string ReadJSqlFile(string file)
    {
        return File.ReadAllText(file);
    }
}