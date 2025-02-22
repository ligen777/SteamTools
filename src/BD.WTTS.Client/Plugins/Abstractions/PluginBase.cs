using dotnetCampus.Ipc.Pipes;
using static BD.WTTS.Services.IPCSubProcessModuleService.Constants;

namespace BD.WTTS.Plugins.Abstractions;

public abstract partial class PluginBase<TPlugin> : IPlugin where TPlugin : PluginBase<TPlugin>
{
    readonly Lazy<string> mAppDataDirectory;
    readonly Lazy<string> mCacheDirectory;

    public PluginBase()
    {
        Instance = (TPlugin)this;
        mAppDataDirectory = new(() => GetPluginsDirectory(Name, IOPath.AppDataDirectory));
        mCacheDirectory = new(() => GetPluginsDirectory(Name, IOPath.CacheDirectory));
    }

    public virtual IEnumerable<TabItemViewModel>? GetMenuTabItems() => null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected (Action<IServiceCollection>? @delegate, bool isInvalid, string name) GetConfiguration<TSettings>(bool directoryExists) where TSettings : class, ISettings<TSettings>, new()
    {
        var isInvalid = ISettings<TSettings>.Load(directoryExists, out var @delegate, mAppDataDirectory.Value);
        return (@delegate, isInvalid, TSettings.Name);
    }

    public virtual IEnumerable<(Action<IServiceCollection>? @delegate, bool isInvalid, string name)>? GetConfiguration(bool directoryExists) => null;

    public virtual ValueTask OnInitializeAsync() => ValueTask.CompletedTask;

    public virtual void ConfigureDemandServices(
        IServiceCollection services,
        Startup startup)
    {

    }

    public virtual void ConfigureRequiredServices(
        IServiceCollection services,
        Startup startup)
    {

    }

    public virtual void OnAddAutoMapper(IMapperConfigurationExpression cfg)
    {

    }

    public virtual void OnUnhandledException(Exception ex, string name, bool? isTerminating = null)
    {

    }

    public virtual bool ExplicitHasValue()
    {
        return true;
    }

    public virtual ValueTask OnExit()
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// 将参数解析为字符串
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static string DecodeArgs(string args)
        => HttpUtility.UrlDecode(args);

    /// <summary>
    /// 将参数解析为字符串数组
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected static string[] DecodeToArrayArgs(string args)
        => HttpUtility.UrlDecode(args).Split(' ', StringSplitOptions.RemoveEmptyEntries);

    public virtual async Task<int> RunSubProcessMainAsync(string moduleName, string pipeName, string processId, string encodedArgs)
    {
        var subProcessBootConfiguration = GetSubProcessBootConfiguration(encodedArgs ?? string.Empty);
        if (subProcessBootConfiguration == default)
            return (int)CommandExitCode.GetSubProcessBootConfigurationFail;

        var exitCode = await IPCSubProcessService.MainAsync(moduleName, Name,
            subProcessBootConfiguration.configureServices,
            subProcessBootConfiguration.configureIpcProvider,
            new[] { pipeName, processId });
        return exitCode;
    }

    /// <summary>
    /// 获取子进程 IPC 程序启动配置
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    protected virtual (Action<IServiceCollection>? configureServices, Action<IpcProvider>? configureIpcProvider) GetSubProcessBootConfiguration(string args)
    {
        return default;
    }

    public virtual ValueTask OnPeerConnected(bool isReconnected)
    {
        return ValueTask.CompletedTask;
    }
}
