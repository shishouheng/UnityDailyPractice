using System;
using System.Collections.Concurrent;
using UnityEngine;

public class HookUtils
{
    // 使用 ConcurrentDictionary 存储钩子信息，保证线程安全
    private static readonly ConcurrentDictionary<string, HookInfo> hookInfos = new ConcurrentDictionary<string, HookInfo>();

    public static void Begin(string hookName)
    {
        // 在此处开始性能分析等操作
        // 仅添加hookName到hookInfos
        hookInfos.TryAdd(hookName, new HookInfo { StartTime = DateTime.UtcNow });
        Debug.Log("Hook Begin: " + hookName);
    }

    public static void End(string hookName)
    {
        // 移除已结束的钩子信息
        if (hookInfos.TryRemove(hookName, out HookInfo hookInfo))
        {
            TimeSpan elapsedTime = DateTime.UtcNow - hookInfo.StartTime;
            Debug.Log($"Hook End: {hookName}, Elapsed Time: {elapsedTime.TotalMilliseconds} ms");

            // 实际项目中，您可以在此处结束性能分析并将结果发送回服务端
        }
        else
        {
            Debug.LogError("No matching hook found to end: " + hookName);
        }
    }

    public static void ToMessage()
    {
        foreach (var hookInfo in hookInfos.Values)
        {
            Debug.Log($"Hook Name: {hookInfo.HookName}, Start Time: {hookInfo.StartTime}");
        }
    }

    public class HookInfo
    {
        public string HookName { get; set; }
        public DateTime StartTime { get; set; }
    }
}