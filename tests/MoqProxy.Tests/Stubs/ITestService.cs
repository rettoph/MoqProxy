namespace MoqProxy.Tests.Stubs
{
    public interface ITestService
    {
        void ActionNoArguments();
        void GenericActionNoArguments<T>();
        void ActionWithArguments(int arg1, string arg2);
        void GenericActionWithArguments<T1, T2>(T1 arg1, T2 arg2);
        void ActionWithRefArgument(ref int arg1);
        void ActionWithOutArgument(out int arg1);
        void ActionWithInArgument(in int arg1);

        int FuncNoArguments();
        int GenericFuncNoArguments<T>();
        int FuncWithArguments(int arg1, string arg2);
        T1 GenericFuncWithArguments<T1, T2>(T1 arg1, T2 arg2);
    }
}
