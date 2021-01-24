using System;
using System.Threading.Tasks;
using ZxTap2Wav.Net;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var tape = Tape.Create("../../../test.tap");
            tape.SaveWav("../../../test.wav");
        }
    }
}