namespace MoqProxy.Tests.Stubs
{
    public interface ITestService
    {
        void ActionNoArguments();
        void GenericActionNoArguments<T>();

        int FuncNoArguments();
        int GenericFuncNoArguments<T>();
    }
}
