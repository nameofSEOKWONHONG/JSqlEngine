using System.Collections.Concurrent;

namespace JSqlEngine;

public class JSqlCore
{
    //[쿼리타이틀]_[날짜]_[순번].jsql
    //[GET_RESERVATION]_[20230927]_[1].jsql
    private readonly string _rootPath;
    public JSqlCore(string rootPath)
    {
        _rootPath = rootPath;
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

    public async Task InitAsync()
    {
        var dirs = Directory.GetDirectories(_rootPath);
        foreach (var dir in dirs)
        {
            var result = await SearchDirAsync(dir);
            foreach (var item in result)
            {
                _jsqlFileInfos.TryAdd(item.FileName, item);
            }
        }
    }

    public async Task ReReadAsync()
    {
        var dirs = Directory.GetDirectories(_rootPath);
        foreach (var dir in dirs)
        {
            var result = await SearchDirAsync(dir);
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

    private async Task<List<JSqlFileInfo>> SearchDirAsync(string dir)
    {
        List<JSqlFileInfo> jsqls = new();
        var files = Directory.GetFiles(dir, "*.jsql");
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            var name = fileInfo.Name;
            var date = fileInfo.LastWriteTime;
            var jsql = await ReadJSqlAsync(file);
            jsqls.Add(new JSqlFileInfo()
            {
                FileName = name,
                FileDate = date.ToString("yyyy-MM-dd HH:mm:ss"),
                Sql = jsql
            });
        }

        return jsqls;
    }

    private async Task<string> ReadJSqlAsync(string file)
    {
        return await File.ReadAllTextAsync(file);
    }
}