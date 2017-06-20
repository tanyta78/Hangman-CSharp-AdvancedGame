using System;
using System.Drawing;
using System.Threading;

public class PleaseWait : IDisposable
{
    public bool stopped { get; set; }

    public PleaseWait()
    {
        Thread t = new Thread(new ThreadStart(workerThread));
        t.IsBackground = true;
        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        stopped = false;
    }
    public void Dispose()
    {
        stopThread();
    }
    private void stopThread()
    {
        stopped = true;
    }
    private void workerThread()
    {
        int i = 1;
        while (!stopped)
        {
            if (i == 4)
            {
                i = 1;
            }
            Console.Clear();
            Console.WriteLine("Loading" + new string('.', i));
            Thread.Sleep(500);
            i++;
        }
    }
}