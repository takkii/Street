using System;
using System.Threading;

namespace street
{
    public class ClassWithDelegate
    {
        public delegate int DelegateThatReturnsInt();

        public DelegateThatReturnsInt TheDelegate;


        public void Run()
        {
            for (;;)
            {
                Thread.Sleep(500);
                if (TheDelegate != null)
                {
                    foreach (var @delegate
                        in TheDelegate.GetInvocationList())
                    {
                        var del = (DelegateThatReturnsInt) @delegate;
                        del.BeginInvoke(
                            ResultsReturned,
                            del);
                    }
                }
            }

            // ReSharper disable once FunctionNeverReturns
        }

        private static void ResultsReturned(IAsyncResult iar)
        {
            DelegateThatReturnsInt del =
                (DelegateThatReturnsInt) iar.AsyncState;
            int result = del.EndInvoke(iar);
            Console.WriteLine("デリゲートの実行結果:{0}", result);
        }
    }

    public class FirstSubscriber
    {
        private int _myCounter;

        public void Subscribe(ClassWithDelegate theClassWithDelegate)
        {
            theClassWithDelegate.TheDelegate +=
                DisplayCounter;
        }

        public int DisplayCounter()
        {
            Console.WriteLine("DisplayCounterの処理中です...");
            Thread.Sleep(10000);
            Console.WriteLine("DisplayCounterの処理が完了しました...");
            return ++_myCounter;
        }
    }

    public class SecondSubscriber
    {
        public int MyCounter;

        public void SubScribe(ClassWithDelegate theClassWithDelegate)
        {
            theClassWithDelegate.TheDelegate +=
                Doubler;
        }

        public int Doubler()
        {
            return MyCounter += 2;
        }
    }

    public class Himekuri
    {
        public static void Main()
        {
            try
            {
                ClassWithDelegate theClassWithDelegate =
                    new ClassWithDelegate();

                FirstSubscriber fs = new FirstSubscriber();
                fs.Subscribe(theClassWithDelegate);

                SecondSubscriber ss = new SecondSubscriber();
                ss.SubScribe(theClassWithDelegate);

                theClassWithDelegate.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e);
            }
        }
    }
}
